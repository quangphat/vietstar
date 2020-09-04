using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Product;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using System.IO;
using VietStar.Entities.Commons;

namespace VietStar.Business
{
    public class ProductBusiness : BaseBusiness, IProductBusiness
    {
        protected readonly IProductRepository _rpProduct;
        protected IServiceProvider _svProvider;
        public ProductBusiness(IProductRepository productRepository,
             IServiceProvider svProvider ,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpProduct = productRepository;
            _svProvider = svProvider;
        }
        public async Task<ProductIndexModel> GetByIdAync(int id)
        {
            var result = await _rpProduct.GetByIdAync(id);
            if(result==null)
            {
                return ToResponse<ProductIndexModel>(null, Errors.notfound);
            }
            return result;
        }

        public async Task<bool> DeleteByIdAync(int id)
        {
            var result = await _rpProduct.DeleteByIdAsync(id, _process.User.Id);
            
            return ToResponse(result);
        }

        public async Task<DataPaging<List<ProductIndexModel>>> GetsAsync(
            int page = 1
           , int limit = 10
           , int partnerId = 0
           )
        {
            var response = await _rpProduct.GetsAync(_process.User.Id, page, limit, partnerId);
            if (response == null || !response.Any())
                return DataPaging.Create(response, 0);
            return DataPaging.Create(response, response.FirstOrDefault().TotalRecord);
        }

        public async Task<int> CreateAsync(ProductCreateModel model)
        {
            if (model == null)
            {
                return ToResponse(0, Errors.invalid_data);
            }
            if (model.PartnerId <= 0)
            {
                return ToResponse(0, "Vui lòng chọn đối tác");
            }
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                return ToResponse(0, "Vui lòng nhập tên sản phẩm");
            }
            return ToResponse(await _rpProduct.CreateAsync(model, _process.User.Id, _process.User.OrgId));
        }

        public async Task<bool> UpdateAsync(ProductUpdateModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return ToResponse(false, Errors.invalid_data);
            }
            if (model.PartnerId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn đối tác");
            }
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                return ToResponse(false, "Vui lòng nhập tên sản phẩm");
            }
            return ToResponse(await _rpProduct.UpdateAsync(model, _process.User.Id));
        }

        public async Task<string> InsertFromFileAsync(IFormFile file)
        {
            if (file == null)
                return ToResponse(string.Empty, Errors.file_cannot_be_null);
            var bizCommon = _svProvider.GetService<ICommonBusiness>();
            var inputParams = null as List<DynamicParameters>;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                inputParams = await bizCommon.ReadXlsxFileAsync(stream, Entities.Commons.Enums.ProfileType.Product, 1,1);
            }
            if (inputParams == null)
            {
                return $"Đã import thành công {0} dòng";
            }

            var result = await _rpProduct.InsertManyByParameterAsync(inputParams, _process.User.Id, _process.User.OrgId);
            return $"Đã import thành công {result} dòng";
        }
    }
}
