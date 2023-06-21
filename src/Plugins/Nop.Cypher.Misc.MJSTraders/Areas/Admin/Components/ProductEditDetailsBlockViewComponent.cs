using Microsoft.AspNetCore.Mvc;
using Nop.Cypher.Misc.MJSTraders.Areas.Admin.Model;
using Nop.Cypher.Misc.MJSTraders.Services.ProductCustomDetails;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using System.Threading.Tasks;

namespace Nop.Cypher.Misc.MJSTraders.Areas.Admin.Components
{
    [ViewComponent(Name = "ProductEditDetailsBlock")]
    public class ProductEditDetailsBlockViewComponent : NopViewComponent
    {
        #region fields

        private readonly IProductCustomDetailService _productCustomDetailService;

        #endregion

        #region ctor

        public ProductEditDetailsBlockViewComponent(IProductCustomDetailService productCustomDetailService)
        {
            _productCustomDetailService = productCustomDetailService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            //product
            var product = (ProductModel)additionalData;

            if (product == null)
                return Content("");

            if (product.Id == 0)
                return Content("");

            var pDetail = await _productCustomDetailService.GetProductCustomDetailByProductIdAsync(product.Id);

            var model = new ProductDetailCustomModel()
            {
                ProductId = product.Id,
                ProductUnit = pDetail?.ProductUnit,
                ProductPiecePerUnit = pDetail?.ProductPiecePerUnit
            };

            return View(model);
        }

        #endregion
    }
}