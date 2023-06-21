-- CREATED NEW TABLE FOR OPTIMIZE SALES QUOTE
-- 12/12/2020
CREATE TABLE [dbo].[NC_MJST_SalesQuotationCustomer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[EmailCC] [nvarchar](max) NULL,
 CONSTRAINT [PK_NC_MJST_SalesQuotationCustomer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--ADD COLUMN 'SalesQuotationCustomerId' IN [NC_MJST_SalesQuotation] TABLE
-- 12/12/2020
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'SalesQuotationCustomerId')
BEGIN
	ALTER TABLE [dbo].[NC_MJST_SalesQuotation] ADD SalesQuotationCustomerId int NOT NULL DEFAULT 0
END
GO

--UPDATE SUBJECT FOR MJSTraders.SendSalesQuotationCustomerNotification MESSAGE TEMPLATE
-- 14/12/2020
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[MessageTemplate]') AND NAME = 'MJSTraders.SendSalesQuotationCustomerNotification')
BEGIN
	UPDATE [dbo].[MessageTemplate] 
	SET "Subject"='SALES QUOTATION - %MJST.Title%'
	WHERE "Name"='MJSTraders.SendSalesQuotationCustomerNotification'
END
GO

--ADD COLUMN 'SendFromEntity' IN [NC_MJST_SalesQuotation] TABLE
-- 14/12/2020
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'SendFromEntity')
BEGIN
	ALTER TABLE [dbo].[NC_MJST_SalesQuotation] ADD SendFromEntity nvarchar(MAX) NOT NULL DEFAULT 'MJSTraders'
END
GO

--INSERT DATA INTO 'NC_MJST_SalesQuotationCustomer' FROM 'NC_MJST_SalesQuotation'
--UPDATE 'SalesQuotationCustomerId' BASED ON 'NC_MJST_SalesQuotationCustomer' 'Id'
INSERT INTO NC_MJST_SalesQuotationCustomer
SELECT "Name",Email,EmailCC
FROM (
   SELECT *,
          row_number() OVER (PARTITION BY Email ORDER BY Id) AS row_number
   FROM NC_MJST_SalesQuotation
   ) AS ROWS
WHERE row_number = 1

MERGE INTO NC_MJST_SalesQuotation
   USING NC_MJST_SalesQuotationCustomer
      ON NC_MJST_SalesQuotation.Email = NC_MJST_SalesQuotationCustomer.Email
WHEN MATCHED 
THEN
   UPDATE 
      SET SalesQuotationCustomerId = NC_MJST_SalesQuotationCustomer.Id;


--Rename EmailCC to Company
-- 28/04/2021
sp_rename 'NC_MJST_SalesQuotationCustomer.EmailCC', 'Company', 'COLUMN';
go
sp_rename 'NC_MJST_SalesQuotation.Detail', 'Company', 'COLUMN';
go

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'Designation')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add Designation nvarchar(max) NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'ReferenceNumber')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add ReferenceNumber nvarchar(max) NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'DeliveryTerms')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add DeliveryTerms nvarchar(max) NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'DeliveryCharges')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add DeliveryCharges decimal NOT NULL DEFAULT 0
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'PaymentTerms')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add PaymentTerms nvarchar(max) NULL
END
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'ValidUntilUtc')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add ValidUntilUtc datetime2(7) NULL
END
GO

-- Update company in sales customer 

-- Declare and open a keyset-driven cursor.  
DECLARE @email nvarchar(max);
DECLARE update_customer_name CURSOR FOR 
	select Email  from  customer where Id in (select Customer_Id from Customer_CustomerRole_Mapping where CustomerRole_Id = 3)
	
OPEN update_customer_name;  
  
FETCH NEXT FROM update_customer_name INTO @email  

WHILE @@FETCH_STATUS = 0  
BEGIN  
	Declare @cm nvarchar(max)
	set @cm = (select [Value] from GenericAttribute where KeyGroup= 'Customer' and [Key]= 'Company' and EntityId = (SELECT Id from Customer where Email = @email) )

	update [NC_MJST_SalesQuotationCustomer] set Company = @cm where Email= @email

    FETCH NEXT FROM update_customer_name INTO @email 
END 

CLOSE update_customer_name 
DEALLOCATE update_customer_name
GO

--ADD COLUMN 'CreatedBy' IN [NC_MJST_SalesQuotation] TABLE
-- 2021/05/26
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = object_id('[NC_MJST_SalesQuotation]') AND NAME = 'CreatedBy')
BEGIN
	alter table [dbo].[NC_MJST_SalesQuotation] add CreatedBy nvarchar(MAX) NULL
END
GO