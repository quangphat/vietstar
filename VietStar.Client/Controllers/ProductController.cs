using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Product;

namespace VietStar.Client.Controllers
{
    [Authorize]
    [Route("Product")]
    public class ProductController : VietStarBaseController
    {
        protected readonly IProductBusiness _bizProduct;
        public ProductController(IProductBusiness productBusiness, CurrentProcess process) : base(process)
        {
            _bizProduct = productBusiness;
        }
        [MyAuthorize(Permissions = "product")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(
            int page = 1
            , int limit = 20
            ,int partnerId =0
            )
        {
            var result = await _bizProduct.GetsAsync( page, limit, partnerId);

            return ToResponse(result);
        }

        [MyAuthorize(Permissions ="product,product.write")]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [MyAuthorize(Permissions = "product,product.write")]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ProductCreateModel model)
        {
            var result = await _bizProduct.CreateAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "product,product.write")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _bizProduct.GetByIdAync(id);
            if (product == null)
                return Redirect("/Product/Index");
            return View(product);
        }

        [MyAuthorize(Permissions = "product,product.write")]
        [HttpPost("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductUpdateModel model)
        {
            var result = await _bizProduct.UpdateAsync(model);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "product,product.import")]
        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizProduct.InsertFromFileAsync(file);
            return ToResponse(result);
        }

        [MyAuthorize(Permissions = "product,product.write")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bizProduct.DeleteByIdAync(id);
            return ToResponse(result);
        }
    }
}