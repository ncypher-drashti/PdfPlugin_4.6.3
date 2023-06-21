using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;
using Nop.Core.Infrastructure;
using Nop.Cypher.Misc.MJSTraders.Domain;
using Nop.Cypher.Misc.MJSTraders.Services.SalesQuotations;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Services.Messages
{
    public partial class MJSTradersWorkFlowMessageService : IMJSTradersWorkFlowMessageService
    {
        #region Fields

        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly ILocalizationService _localizationService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ITokenizer _tokenizer;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICustomerService _customerService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly INopFileProvider _fileProvider;
        private readonly IDownloadService _downloadService;
        private readonly ISalesQuotationService _salesQuotationService;

        #endregion

        #region Ctor

        public MJSTradersWorkFlowMessageService(IStoreContext storeContext,
            ILanguageService languageService,
            IMessageTemplateService messageTemplateService,
            ILocalizationService localizationService,
            IEmailAccountService emailAccountService,
            EmailAccountSettings emailAccountSettings,
            IMessageTokenProvider messageTokenProvider,
            ITokenizer tokenizer,
            IEventPublisher eventPublisher,
            ICustomerService customerService,
            IWorkflowMessageService workflowMessageService,
            IGenericAttributeService genericAttributeService,
            INopFileProvider fileProvider,
            IDownloadService downloadService,
            ISalesQuotationService salesQuotationService)
        {
            _storeContext = storeContext;
            _languageService = languageService;
            _messageTemplateService = messageTemplateService;
            _localizationService = localizationService;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
            _messageTokenProvider = messageTokenProvider;
            _tokenizer = tokenizer;
            _eventPublisher = eventPublisher;
            _customerService = customerService;
            _workflowMessageService = workflowMessageService;
            _genericAttributeService = genericAttributeService;
            _fileProvider = fileProvider;
            _downloadService = downloadService;
            _salesQuotationService = salesQuotationService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get active message templates by the name
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>List of message templates</returns>
        protected virtual async Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        /// <summary>
        /// Get EmailAccount to use with a message templates
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>EmailAccount</returns>
        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (await _emailAccountService.GetEmailAccountByIdAsync(emailAccountId) ?? await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
                               (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();
            return emailAccount;
        }

        /// <summary>
        /// Ensure language is active
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Return a value language identifier</returns>
        protected virtual async Task<int> EnsureLanguageIsActiveAsync(int languageId, int storeId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguagesAsync(storeId: storeId)).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends a approve PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public async Task<IList<int>> SendCustomerApprovePoNumberNotificationAsync(PONumber poNumber, int languageId)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var customer = await _customerService.GetCustomerByIdAsync(poNumber.CustomerId);
            var messageTemplates = await GetActiveMessageTemplatesAsync(MJSTradersEmailTemplates.ApprovePONumberCustomerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens shop feature request
            var commonTokens = new List<Token>();
            commonTokens.Add(new Token("PONumber.CustomerName", await _customerService.GetCustomerFullNameAsync(customer)));
            commonTokens.Add(new Token("PONumber.CompanyName", await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute)));

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //Replace subject and body tokens
                string subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
                var subjectReplaced = _tokenizer.Replace(subject, commonTokens, false);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                var toEmail = customer.Email;
                var toName = await _customerService.GetCustomerFullNameAsync(customer);

                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName, subject: subjectReplaced);
            }).ToListAsync();
        }

        /// <summary>
        /// Sends a delete PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<IList<int>> SendCustomerDeletePoNumberNotificationAsync(PONumber poNumber, int languageId)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(MJSTradersEmailTemplates.DeletePONumberCustomerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens shop feature request
            var commonTokens = new List<Token>();
            commonTokens.Add(new Token("MJST.Id", poNumber.Id));
            commonTokens.Add(new Token("MJST.Title", poNumber.Title));

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
                var customer = await _customerService.GetCustomerByIdAsync(poNumber.CustomerId);
                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //Replace subject and body tokens
                string subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
                var subjectReplaced = _tokenizer.Replace(subject, commonTokens, false);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                var toEmail = customer.Email;
                var toName = await _customerService.GetCustomerFullNameAsync(customer);

                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName, subject: subjectReplaced);
            }).ToListAsync();
        }

        /// <summary>
        /// Sends a submit PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<IList<int>> SendCustomerUploadPoNumberNotificationAsync(PONumber poNumber, int languageId)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(MJSTradersEmailTemplates.UploadPONumberCustomerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens shop feature request
            var commonTokens = new List<Token>();
            commonTokens.Add(new Token("MJST.Id", poNumber.Id));
            commonTokens.Add(new Token("MJST.Title", poNumber.Title));
            commonTokens.Add(new Token("MJST.CreateOnUtc", poNumber.CreateOnUtc));

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
                var customer = await _customerService.GetCustomerByIdAsync(poNumber.CustomerId);
                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //Replace subject and body tokens
                string subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
                var subjectReplaced = _tokenizer.Replace(subject, commonTokens, false);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                var toEmail = customer.Email;
                var toName = await _customerService.GetCustomerFullNameAsync(customer);

                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName, subject: subjectReplaced);
            }).ToListAsync();
        }

        /// <summary>
        /// Sends a submit PO number notification
        /// </summary>
        /// <param name="poNumber">PO number</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<IList<int>> SendAdminUploadPoNumberNotificationAsync(PONumber poNumber, int languageId)
        {
            if (poNumber == null)
                throw new ArgumentNullException(nameof(poNumber));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var customer = await _customerService.GetCustomerByIdAsync(poNumber.CustomerId);
            var messageTemplates = await GetActiveMessageTemplatesAsync(MJSTradersEmailTemplates.UploadPONumberAdminNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens shop feature request
            var commonTokens = new List<Token>();
            commonTokens.Add(new Token("PONumber.Id", poNumber.Id));
            commonTokens.Add(new Token("PONumber.CustomerName", await _customerService.GetCustomerFullNameAsync(customer)));
            commonTokens.Add(new Token("PONumber.CompanyName", await _genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CompanyAttribute)));

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //Replace subject and body tokens
                string subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
                var subjectReplaced = _tokenizer.Replace(subject, commonTokens, false);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                var toEmail = emailAccount.Email;
                var toName = emailAccount.DisplayName;

                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName, subject: subjectReplaced);
            }).ToListAsync();
        }

        /// <summary>
        /// Sends a sales quotation notification
        /// </summary>
        /// <param name="salesQuotation">Sales quotation</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<IList<int>> SendSalesQuotationCustomerNotificationAsync(SalesQuotation salesQuotation, int languageId, string attachmentFilePath = null, string attachmentFileName = null)
        {
            if (salesQuotation == null)
                throw new ArgumentNullException(nameof(salesQuotation));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync(MJSTradersEmailTemplates.SendSalesQuotationCustomerNotification, store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens shop feature request
            var commonTokens = new List<Token>
            {
                new Token("MJST.Title", salesQuotation.Title),
                new Token("SalesQuotation.CustomerName", salesQuotation.Name)
            };

            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);
                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //Replace subject and body tokens
                string subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
                var subjectReplaced = _tokenizer.Replace(subject, commonTokens, false);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                var toEmail = salesQuotation.Email;
                var toName = salesQuotation.Name;

                return await this.SendNotificationAsync(salesQuotation, messageTemplate, emailAccount, languageId, tokens, toEmail, toName, salesQuotation.EmailCC, attachmentFilePath, attachmentFileName, subject: subjectReplaced);
            }).ToListAsync();
        }


        /// <summary>
        /// Send notification
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="tokens">Tokens</param>
        /// <param name="toEmailAddress">Recipient email address</param>
        /// <param name="toName">Recipient name</param>
        /// <param name="toEmailCC">Recipient cc email address</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name</param>
        /// <param name="replyToEmailAddress">"Reply to" email</param>
        /// <param name="replyToName">"Reply to" name</param>
        /// <param name="fromEmail">Sender email. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="fromName">Sender name. If specified, then it overrides passed "emailAccount" details</param>
        /// <param name="subject">Subject. If specified, then it overrides subject of a message template</param>
        /// <returns>Queued email identifier</returns>
        public virtual async Task<int> SendNotificationAsync(SalesQuotation salesQuotation, MessageTemplate messageTemplate,
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName, string toEmailAddressCC,
            string attachmentFilePath = null, string attachmentFileName = null,
            string replyToEmailAddress = null, string replyToName = null,
            string fromEmail = null, string fromName = null, string subject = null)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            if (emailAccount == null)
                throw new ArgumentNullException(nameof(emailAccount));

            //retrieve localized message template data
            var bcc = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.BccEmailAddresses, languageId);
            if (string.IsNullOrEmpty(subject))
                subject = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Subject, languageId);
            var body = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.Body, languageId);

            //Replace subject and body tokens
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);

            //limit name length
            toName = CommonHelper.EnsureMaximumLength(toName, 300);

            //if (!string.IsNullOrEmpty(bcc) && !string.IsNullOrEmpty(toEmailAddressCC))
            //{
            //    bcc = bcc + ","+ toEmailAddressCC;
            //}
            var email = new QueuedEmail
            {
                Priority = QueuedEmailPriority.High,
                From = !string.IsNullOrEmpty(fromEmail) ? fromEmail : emailAccount.Email,
                FromName = !string.IsNullOrEmpty(fromName) ? fromName : emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                ReplyTo = replyToEmailAddress,
                ReplyToName = replyToName,
                CC = toEmailAddressCC,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                AttachmentFilePath = attachmentFilePath,
                AttachmentFileName = attachmentFileName,
                AttachedDownloadId = messageTemplate.AttachedDownloadId,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                DontSendBeforeDateUtc = !messageTemplate.DelayBeforeSend.HasValue ? null
                : (DateTime?)(DateTime.UtcNow + TimeSpan.FromHours(messageTemplate.DelayPeriod.ToHours(messageTemplate.DelayBeforeSend.Value)))
            };

            //_queuedEmailService.InsertQueuedEmail(email);

            #region send email

            var message = new MailMessage
            {
                //from, to, reply to
                From = new MailAddress(email.From, fromName)
            };
            message.To.Add(new MailAddress(email.To, toName));
            if (!string.IsNullOrEmpty(email.ReplyTo))
            {
                message.ReplyToList.Add(new MailAddress(email.ReplyTo, replyToName));
            }

            var bccEmail = string.IsNullOrWhiteSpace(email.Bcc)
                            ? null
                            : email.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var ccEmail = string.IsNullOrWhiteSpace(email.CC)
                        ? null
                        : email.CC.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //BCC
            if (bcc != null)
            {
                foreach (var address in bccEmail.Where(bccValue => !string.IsNullOrWhiteSpace(bcc)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            //CC
            if (ccEmail != null)
            {
                foreach (var address in ccEmail.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = email.Subject;
            message.Body = email.Body;
            message.IsBodyHtml = true;

            ////headers
            //if (headers != null)
            //    foreach (var header in headers)
            //    {
            //        message.Headers.Add(header.Key, header.Value);
            //    }

            //create the file attachment for this e-mail message
            if (!string.IsNullOrEmpty(attachmentFilePath) && _fileProvider.FileExists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = _fileProvider.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = _fileProvider.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = _fileProvider.GetLastAccessTime(attachmentFilePath);
                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }

                message.Attachments.Add(attachment);
            }
            //another attachment?
            if (messageTemplate.AttachedDownloadId > 0)
            {
                var download = await _downloadService.GetDownloadByIdAsync(messageTemplate.AttachedDownloadId);
                if (download != null)
                {
                    //we do not support URLs as attachments
                    if (!download.UseDownloadUrl)
                    {
                        var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();
                        fileName += download.Extension;

                        var ms = new MemoryStream(download.DownloadBinary);
                        var attachment = new Attachment(ms, fileName);
                        //string contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
                        //var attachment = new Attachment(ms, fileName, contentType);
                        attachment.ContentDisposition.CreationDate = DateTime.UtcNow;
                        attachment.ContentDisposition.ModificationDate = DateTime.UtcNow;
                        attachment.ContentDisposition.ReadDate = DateTime.UtcNow;
                        message.Attachments.Add(attachment);
                    }
                }
            }

            // quotation attachmenet
            var salesQuotationDocuments = await _salesQuotationService.GetAllSalesQuotationDocumentsAsync(salesQuotationId: salesQuotation.Id);
            foreach (var salesDownload in salesQuotationDocuments)
            {
                var download = await _downloadService.GetDownloadByIdAsync(salesDownload.DownloadId);

                if (download != null)
                {
                    //we do not support URLs as attachments
                    if (!download.UseDownloadUrl)
                    {
                        var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id.ToString();
                        fileName += download.Extension;

                        var ms = new MemoryStream(download.DownloadBinary);
                        var attachment = new Attachment(ms, fileName);
                        //string contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : "application/octet-stream";
                        //var attachment = new Attachment(ms, fileName, contentType);
                        attachment.ContentDisposition.CreationDate = DateTime.UtcNow;
                        attachment.ContentDisposition.ModificationDate = DateTime.UtcNow;
                        attachment.ContentDisposition.ReadDate = DateTime.UtcNow;
                        message.Attachments.Add(attachment);
                    }
                }
            }

            //send email
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password);
                smtpClient.Send(message);
            }

            #endregion

            return email.Id;
        }


        #endregion
    }
}