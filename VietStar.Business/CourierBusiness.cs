using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Commons;
using VietStar.Entities.Courier;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Note;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using static VietStar.Entities.Commons.Enums;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using System.IO;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;

namespace VietStar.Business
{
    public class CourierBusiness : BaseBusiness, ICourierBusiness
    {
        protected readonly ICourierRepository _rpCourier;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly INoteRepository _rpNote;
        protected readonly IServiceProvider _svProvider;
        protected readonly IGroupRepository  _rpGroup;

        public CourierBusiness(ICourierRepository courierRepository,
            IEmployeeRepository employeeRepository,
            INoteRepository noteRepository,
            IServiceProvider svProvider,
            IGroupRepository groupRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCourier = courierRepository;
            _rpEmployee = employeeRepository;
            _rpNote = noteRepository;
            _svProvider = svProvider;
            _rpGroup = groupRepository;
        }

        public async Task<DataPaging<List<CourierIndexModel>>> GetsAsync(string freeText,
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var response = await _rpCourier.GetsAsync(freeText ,fromDate.Value, toDate.Value,dateType ,status,page, limit,assigneeId,groupId,provinceId,saleCode, _process.User.Id);
            if (response == null || !response.Any())
                return DataPaging.Create(response, 0);
            return DataPaging.Create(response, response.FirstOrDefault().TotalRecord);
        }

        public async Task<string> ExportAsync(string contentRootPath,
            string freeText,
            DateTime? fromDate
            , DateTime? toDate
            , int dateType = 2
            , string status = null
            , int page = 1
            , int limit = 10
            , int assigneeId = 0
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var request = new ExportRequestModel
            {
                freeText = freeText,
                fromDate = fromDate.Value,
                toDate = toDate.Value,
                dateType = dateType,
                status = status,
                page = page,
                limit = limit,
                groupId = groupId,
                assigneeId = assigneeId,
                provinceId = provinceId,
                saleCode = saleCode,
                userId = _process.User.Id
            };
            var bizCommon = _svProvider.GetService<ICommonBusiness>();

            var result = await bizCommon.ExportData<ExportRequestModel, CourierIndexModel>(GetDatasAsync, request, contentRootPath, "courier", 2);
            return result;
        }

        public async Task<List<CourierIndexModel>> GetDatasAsync(ExportRequestModel request)
        {
            var result = await _rpCourier.GetsAsync(request.freeText,
                request.fromDate,
                request.toDate,
                request.dateType,
                request.status,
                request.page,
                request.limit,
                request.assigneeId,
                request.groupId,
                request.provinceId,
                request.saleCode,
                request.userId);
            return result;
        }

        public async Task<int> CreateAsync(CourierAddModel model)
        {
            var profile = _mapper.Map<CourierSql>(model);
            profile.CreatedBy = _process.User.Id;
            profile.Status = (int)ProfileStatus.New;
            var response = await _rpCourier.CreateAsync(profile);
            if (!response.success)
                return ToResponse(response);
            var sale = await _rpEmployee.GetEmployeeByCodeAsync(model.SaleCode.ToString().Trim(), _process.User.Id);
            if (sale == null)
            {
                return ToResponse(0, "Sale không tồn tại, vui lòng kiểm tra lại");
            }
            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {

                var note = new NoteAddModel
                {
                    Content = model.LastNote,
                    ProfileId = response.data,
                    UserId = _process.User.Id,
                    ProfileTypeId = (int)NoteType.Courier
                };
                await _rpNote.AddNoteAsync(note);

            }
            return ToResponse(response);
        }

        public async Task<bool> UpdateAsync(CourierUpdateModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                return ToResponse(false, "Tên khách hàng không được để trống");
            }
            if (model.AssignId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn courier");
            }
            var existProfile = await _rpCourier.GetByIdAsync(model.Id);
            if (existProfile == null)
            {
                return ToResponse(false, "Hồ sơ không tồn tại");
            }
            bool isAdmin = _process.User.RoleCode =="admin" ? true :false;
            if (existProfile.Status == (int)ProfileStatus.Cancel)
            {
                if (!isAdmin)
                {
                    return ToResponse(false, "Bạn không có quyền, vui lòng liên hệ Admin");
                }
            }
            var profile = _mapper.Map<CourierSql>(model);
            profile.UpdatedBy = _process.User.Id;
            var response = await _rpCourier.UpdateAsync(profile);
            return ToResponse(response);
        }

        public async Task<CourierIndexModel> GetByIdAsync(int id)
        {
            var result = await _rpCourier.GetByIdAsync(id);
            return result;
        }

        public async Task<bool> InsertFromFileAsync(IFormFile file)
        {
            if (file == null)
                return ToResponse(false, Errors.file_cannot_be_null);
            var bizCommon = _svProvider.GetService<ICommonBusiness>();
            var inputParams = null as List<DynamicParameters>;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var results = await ReadXlsxFile(stream, _process.User.Id);
                if (results == null || !results.Any())
                    return ToResponse(false, "Không thành công");
                var importResult = await _rpCourier.ImportAsync(results);
                return ToResponse(importResult);
            }
            
        }

        private async Task<List<CourierSql>> ReadXlsxFile(MemoryStream stream, int createBy)
        {
            var workBook = WorkbookFactory.Create(stream);
            var sheet = workBook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();
            var hasData = rows.MoveNext();
            var param = new DynamicParameters();
            var pars = new List<DynamicParameters>();
            int count = 0;
            var hosos = new List<CourierSql>();
            for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                try
                {
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.Cells.Count > 1)
                        {
                            bool isNullRow = row.Cells.Count < 3 ? true : false;
                        }
                        var hoso = new CourierSql()
                        {
                            CustomerName = row.Cells[0] != null ? row.Cells[0].ToString() : "",
                            Phone = row.Cells[1] != null ? row.Cells[1].ToString() : "",
                            Cmnd = row.Cells[2] != null ? row.Cells[2].ToString() : "",
                            LastNote = row.Cells[4] != null ? row.Cells[4].ToString() : "",
                            ProvinceId = row.Cells[5] != null ? Convert.ToInt32(row.Cells[5].ToString()) : 0,
                            DistrictId = row.Cells[6] != null ? Convert.ToInt32(row.Cells[6].ToString()) : 0,
                            SaleCode = row.Cells[7] != null ? row.Cells[7].ToString().Trim().ToLower() : string.Empty,
                            Status = (int)ProfileStatus.New,
                            CreatedBy = createBy
                        };
                        var strAssignee = row.Cells[3] != null ? row.Cells[3].ToString() : "";
                        var assigneeIdsStr = string.IsNullOrWhiteSpace(strAssignee) ? new List<string>() : strAssignee.Split(',').ToList();
                        var assigneeIds = (assigneeIdsStr != null && assigneeIdsStr.Any()) ? assigneeIdsStr.Select(s => Convert.ToInt32(s)).ToList() : new List<int>();
                        hoso.AssigneeIds = assigneeIds;
                        hoso.AssignId = assigneeIds.FirstOrDefault();

                        hoso.GroupId = await _rpGroup.GetGroupIdByLeaderIdAsync(hoso.AssignId);
                        hosos.Add(hoso);
                        count++;
                    }
                }
                catch
                {
                    return hosos;
                }

            }
            return hosos;
        }
    }
}
