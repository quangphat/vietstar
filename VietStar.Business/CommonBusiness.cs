using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.FileProfile;
using VietStar.Entities.Infrastructures;
using VietStar.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static VietStar.Entities.Commons.Enums;
using VietStar.Business.Infrastructures;
using System.IO;
using Dapper;
using NPOI.SS.UserModel;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace VietStar.Business
{
    public class CommonBusiness : BaseBusiness, ICommonBusiness
    {
        protected readonly ICommonRepository _rpCommon;
        protected readonly IFileProfileRepository _rpFile;
        protected readonly IPartnerRepository _rpPartner;
        protected readonly ILocationRepository _rpLocation;
        protected readonly IProductRepository _rpProduct;
        protected readonly IEmployeeRepository _rpEmployee;
        protected IServiceProvider _svProvider;
        protected SystemConfig _systemConfig;
        public CommonBusiness(ICommonRepository commonRepository,
            IFileProfileRepository fileProfileRepository,
            IPartnerRepository partnerRepository,
            IEmployeeRepository employeeRepository,
            IProductRepository productRepository,
            ILocationRepository locationRepository,
            IServiceProvider serviceProvider,
            IOptions<SystemConfig> config,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCommon = commonRepository;
            _rpFile = fileProfileRepository;
            _svProvider = serviceProvider;
            _rpPartner = partnerRepository;
            _rpLocation = locationRepository;
            _rpProduct = productRepository;
            _rpEmployee = employeeRepository;
            _systemConfig = config.Value;
        }
        public async Task<List<OptionSimple>> GetPartnerscheckDupAsync()
        {
            return await _rpCommon.GetListForCheckCustomerDuplicateAsync();
        }
        public async Task<List<OptionSimple>> GetPartnersAsync()
        {
            var result = await _rpPartner.GetsAync(_process.User.OrgId);
            return result;
        }
        public async Task<List<OptionSimple>> GetProvincesAsync()
        {
            var result = await _rpLocation.GetProvincesAync();
            return result;
        }
        public async Task<List<OptionSimple>> GetDistrictsAsync(int provinceId)
        {
            var result = await _rpLocation.GetDistrictsAync(provinceId);
            return result;
        }
        public async Task<List<OptionSimple>> GetStatusList(string profileType)
        {
            if (_process.User.OrgId == 2)
            {
                profileType = _process.User.isAdmin ? profileType : _process.User.RoleCode;
            }
            var result = await _rpCommon.GetProfileStatusByRoleCode(profileType, _process.User.OrgId, _process.User.RoleCode);
            return result;
        }



        public async Task<List<OptionSimple>> GetProductsAsync(int partnerId)
        {
            var result = await _rpProduct.GetByPartnerIdsAync(partnerId, _process.User.OrgId);
            return result;
        }

        public async Task<List<OptionSimple>> GetSalesAsync()
        {
            IEmployeeBusiness bizEmployee = _svProvider.GetService<IEmployeeBusiness>();
            var result = await bizEmployee.GetSalesAsync();
            return result;
        }

        public async Task<List<OptionSimple>> GetCouriersAsync()
        {
            var result = await _rpEmployee.GetCouriersAsync(_process.User.OrgId);
            return result;
        }

        public async Task<string> ExportData<TRequest, TData>(Func<TRequest, Task<List<TData>>> funcGetData,
            TRequest request,
            string folder,
            string profileType,
            int rowIndex =2)
            where TData : Pagination
            where TRequest : ExportRequestModelBase
        {
            if (string.IsNullOrWhiteSpace(profileType))
            {
                return ToResponse<string>(string.Empty, "ProfileType không hợp lệ");
            }
            bool exists = System.IO.Directory.Exists(folder);
            if (!exists)
                System.IO.Directory.CreateDirectory(folder);
            string fileName = $"{ DateTime.Now.ToString("yyyy/MM/dd").Replace('/', '_')}_{Guid.NewGuid().ToString()}_{_process.User.Id}_{profileType}.xlsx";
            string fullPath = $"{folder}\\{_systemConfig.ExportFolder}\\{fileName}";

            if (!File.Exists($"{folder}\\{_systemConfig.ExportTemplate}\\{profileType}.xlsx"))
            {
                return ToResponse<string>(string.Empty, "Không tìm thấy file mẫu");
            }

            var exportFrameWorks = await _rpCommon.GetExportFrameworkByTypeAsync(profileType, _process.User.OrgId);
            if (exportFrameWorks == null || !exportFrameWorks.Any())
            {
                return ToResponse<string>(string.Empty, "Không có dữ liệu ExportFramework");
            }
            long totalPage = 1;
            using (var stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                Byte[] info = System.IO.File.ReadAllBytes($"{folder}\\{_systemConfig.ExportTemplate}\\{profileType}.xlsx");
                stream.Write(info, 0, info.Length);
                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    string nameSheet = "Sheet1";
                    ExcelOOXML excelOOXML = new ExcelOOXML(archive);
                    
                    for (int page = 0; page < totalPage; page++)
                    {
                        request.page = page + 1;
                        request.limit = 1000;
                        var datas = await funcGetData(request);

                        if (datas == null || !datas.Any())
                        {
                            return ToResponse<string>(string.Empty, "Không có dữ liệu");
                        }
                        totalPage = (long)Math.Ceiling((decimal)datas[0].TotalRecord / request.limit);
                        if (page == 0)
                            excelOOXML.InsertRow(nameSheet, rowIndex, datas[0].TotalRecord - 1, true);
                        foreach (var item in datas)
                        {
                            foreach (var rowInfo in exportFrameWorks)
                            {
                                excelOOXML.SetCellData(nameSheet, rowInfo.ColPosition + rowIndex, GetValue(item, rowInfo.FieldName));
                            }
                            rowIndex++;
                        }
                    }

                    archive.Dispose();
                }
                stream.Dispose();
            }
            return $"/media/download-export?fileName={fileName}&filePath={fullPath}";
        }

        public async Task<List<DynamicParameters>> ReadXlsxFileAsync(MemoryStream stream, ProfileType profileType, int ignoreRow, int minRowRequire)
        {

            var importExelFrameWork = await _rpCommon.GetImportFrameworkByTypeAsync((int)profileType);
            if (importExelFrameWork == null)
                return ToResponse<List<DynamicParameters>>(null, "Không tìm thấy importExelFrameWork");
            //return null;

            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            var param = new DynamicParameters();
            var pars = new List<DynamicParameters>();
            int skipCell = 0;
            if (sheet.PhysicalNumberOfRows - ignoreRow > _systemConfig.ImportMaxRow)
            {
                return ToResponse<List<DynamicParameters>>(null, $"Số dòng của file không được nhiều hơn {_systemConfig.ImportMaxRow}");
            }
            for (int i = ignoreRow; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    param = new DynamicParameters();
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.Cells.Count > 1)
                        {
                            bool isNullRow = row.Cells.Count < minRowRequire ? true : false;
                            if (isNullRow)
                                continue;
                        }

                        foreach (var col in importExelFrameWork)
                        {
                            try
                            {
                                if (row.GetCell(col.Position) == null)
                                {
                                    param.Add(col.Name, string.Empty);
                                    skipCell += 1;
                                }
                                else
                                {
                                    param.Add(col.Name, TryGetValueFromCell(row.Cells[col.Position - skipCell].ToString(), col.ValueType));
                                }

                            }
                            catch (Exception e)
                            {
                                param = null;
                            }

                        }
                        if (param != null)
                        {
                            skipCell = 0;
                            pars.Add(param);

                        }

                    }
                }
                catch
                {

                }

            }
            return pars;
        }
    }
}
