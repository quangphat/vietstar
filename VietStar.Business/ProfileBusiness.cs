using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Profile;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using Microsoft.Extensions.DependencyInjection;
using static VietStar.Entities.Commons.Enums;
using VietStar.Entities.Commons;

namespace VietStar.Business
{
    public class ProfileBusiness : BaseBusiness, IProfileBusiness
    {
        protected readonly IProfileRepository _rpProfile;
        protected readonly IServiceProvider _svProvider;
        protected readonly IProfileNotificationRepository _rpProfileNoti;
        public ProfileBusiness(IProfileRepository profileRepository,
            IProfileNotificationRepository profileNotificationRepository,
           IServiceProvider svProvider,
        IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpProfile = profileRepository;
            _svProvider = svProvider;
            _rpProfileNoti = profileNotificationRepository;
        }

        public async Task<int> CreateAsync(ProfileAdd model)
        {
            if (ValidateProfile(model) == false)
            {
                return 0;
            }
            if (model.SaleId <= 0)
            {
                model.SaleId = _process.User.Id;
            }
            model.Status = (int)ProfileStatus.Draft;

            var sqlmodel = _mapper.Map<ProfileAddSql>(model);
            var result = await _rpProfile.CreateAsync(sqlmodel, _process.User.Id);
            if (result.data > 0 && !string.IsNullOrWhiteSpace(model.Comment))
            {
                var bzNot = _svProvider.GetService<INoteBusiness>();
                await bzNot.AddNoteAsync(new Entities.Note.NoteAddRequest
                {
                    Content = model.Comment,
                    ProfileId = result.data,
                    ProfileTypeId = (int)NoteType.Common
                });
            }
            return ToResponse(result);
        }
        public async Task<bool> UpdateProfile(ProfileAdd model)
        {
            if (!ValidateProfile(model))
            {
                return false;
            }
            var profile = await _rpProfile.GetByIdAsync(model.Id);
            if (!profile.success)
            {
                return ToResponse(false, Errors.profile_could_not_found_in_portal);
            }

            if (model.SaleId <= 0)
            {
                model.SaleId = _process.User.Id;
            }
            model.Status = model.Status == (int)ProfileStatus.Draft ? (int)ProfileStatus.New : model.Status;
            var sqlmodel = _mapper.Map<ProfileAddSql>(model);
            var result = await _rpProfile.UpdateAsync(sqlmodel, model.Id, _process.User.Id);
            if (result.data == true)
            {
                await _rpProfileNoti.CreateAsync(model.Id);
            }
            return ToResponse<bool>(result.data, result.error);
        }

        public async Task<DataPaging<List<ProfileIndexModel>>> GetsAsync(DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1
            , int limit = 20)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var result = await _rpProfile.GetsAsync(_process.User.Id, fromDate.Value, toDate.Value, dateType, groupId, memberId, status, freeText, sort, sortField, page, limit);
            if (result == null || !result.Any())
            {
                return DataPaging.Create((List<ProfileIndexModel>)null, 0);
            }

            return DataPaging.Create(result, result[0].TotalRecord);

        }
        public async Task<ProfileEditView> GetByIdAsync(int profileId)
        {
            var data = await _rpProfile.GetByIdAsync(profileId);
            var profile = ToResponse(data);
            var result = _mapper.Map<ProfileEditView>(profile);
            return result;
        }

        public async Task<string> ExportAsync(string contentRootPath,DateTime? fromDate
            , DateTime? toDate
            , int dateType = 1
            , int groupId = 0
            , int memberId = 0
            , string status = null
            , string freeText = null
            , string sort = "desc"
            , string sortField = "updatedtime"
            , int page = 1
            , int limit = 20)
        {
            fromDate = fromDate.HasValue ? fromDate.Value.ToStartDateTime() : DateTime.Now.ToStartDateTime();
            toDate = toDate.HasValue ? toDate.Value.ToEndDateTime() : DateTime.Now.ToEndDateTime();
            var request = new ExportRequestModel
            {
                userId = _process.User.Id,
                page = page,
                limit = limit,
                fromDate = fromDate.Value,
                toDate = toDate.Value,
                dateType = dateType,
                groupId = groupId,
                memberId = memberId,
                status = status,
                freeText = freeText,
                sort = sort,
                sortField = sortField
            };
            var bizCommon = _svProvider.GetService<ICommonBusiness>();
            var result = await bizCommon.ExportData<ExportRequestModel, ProfileIndexModel>(GetDatasAsync, request, contentRootPath, "common" , 2);
            return result;
        }

        public async Task<List<ProfileIndexModel>> GetDatasAsync(ExportRequestModel request)
        {
            var result = await _rpProfile.GetsAsync(_process.User.Id, 
                request.fromDate, 
                request.toDate, 
                request.dateType, 
                request.groupId, 
                request.memberId,
                request.status,
                request.freeText,
                request.sort,
                request.sortField, request.page, request.limit);
            return result;
        }

        protected bool ValidateProfile(ProfileAdd model)
        {
            if (model == null)
            {
                AddError(Errors.invalid_data);
                return false;
            }
            if (!string.IsNullOrWhiteSpace(model.Comment) && model.Comment.Length > 200)
            {
                AddError(Errors.note_length_cannot_more_than_200);
                return false;
            }
            if (model.PartnerId <= 0)
            {
                AddError(Errors.missing_partner);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                AddError(Errors.customername_must_not_be_empty);
                return false;
            }
            if (model.ProductId <= 0)
            {
                AddError(Errors.missing_product);
                return false;
            }
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                AddError(Errors.missing_phone);
                return false;
            }
            if (model.ReceiveDate == null)
            {
                AddError(Errors.missing_ngaynhandon);
                return false;

            }
            if (string.IsNullOrWhiteSpace(model.Cmnd))
            {
                AddError(Errors.missing_cmnd);
                return false;

            }
            if (model.DistrictId <= 0)
            {
                AddError(Errors.missing_district);
                return false;

            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                AddError(Errors.missing_diachi);
                return false;

            }


            if (model.LoanAmount <= 0)
            {
                AddError(Errors.missing_money);
                return false;

            }
            if (model.BirthDay == null)
            {
                AddError(Errors.missing_birthday);
                return false;

            }
            if (model.CmndDay == null)
            {
                AddError(Errors.missing_cmnd_day);
                return false;
            }
            return true;
        }
    }
}
