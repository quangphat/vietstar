using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Profile;

namespace VietStar.Business
{
    public abstract class BaseBusiness
    {
        public readonly CurrentProcess _process;
        protected IMapper _mapper;
        public BaseBusiness(IMapper mappr, CurrentProcess process)
        {
            _process = process;
            //var config = new MapperConfiguration(x =>
            //{
            //    x.CreateMap<ProfileAdd, ProfileAddSql>()
            //    .ForMember(a => a.ID, b => b.MapFrom(c => c.Id))
            //    .ForMember(a => a.Ma_Ho_So, b => b.MapFrom(c => c.Code))
            //    .ForMember(a => a.Ten_Khach_Hang, b => b.MapFrom(c => c.CustomerName))
            //    .ForMember(a => a.CMND, b => b.MapFrom(c => c.Cmnd))
            //    .ForMember(a => a.CMNDDay, b => b.MapFrom(c => c.CmndDay))
            //    .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.BirthDay))
            //    .ForMember(a => a.Dia_Chi, b => b.MapFrom(c => c.Address))
            //    .ForMember(a => a.Ma_Khu_Vuc, b => b.MapFrom(c => c.DistrictId))
            //    .ForMember(a => a.SDT, b => b.MapFrom(c => c.Phone))
            //    .ForMember(a => a.Gioi_Tinh, b => b.MapFrom(c => c.Gender))
            //    .ForMember(a => a.SDT2, b => b.MapFrom(c => c.SalePhone))
            //    .ForMember(a => a.Ngay_Nhan_Don, b => b.MapFrom(c => c.ReceiveDate))
            //    .ForMember(a => a.San_Pham_Vay, b => b.MapFrom(c => c.ProductId))
            //    .ForMember(a => a.Co_Bao_Hiem, b => b.MapFrom(c => c.IsInsurrance))
            //    .ForMember(a => a.So_Tien_Vay, b => b.MapFrom(c => c.LoanAmount))
            //    .ForMember(a => a.Ma_Trang_Thai, b => b.MapFrom(c => c.Status))
            //    .ForMember(a => a.Han_Vay, b => b.MapFrom(c => c.Period))
            //    .ForMember(a => a.Ghi_Chu, b => b.MapFrom(c => c.Comment))
            //    .ForMember(a => a.Courier_Code, b => b.MapFrom(c => c.CourierId))
            //    ;
               
            //});
            _mapper = mappr;//config.CreateMapper();
        }
        protected void AddError(string errorMessage, params object[] traceKeys)
        {
            _process.AddError(errorMessage, traceKeys);
        }
        protected bool CheckIsNotLogin()
        {
            if (string.IsNullOrWhiteSpace(_process.User.UserName))
            {
                AddError(Errors.error_login_expected);
                return true;
            }
            return false;
        }
        protected T ToResponse<T>(BaseResponse<T> result)
        {
            if(!result.success)
            {
                AddError(result.error);
                return result.data;
            }
            return result.data;
        }
        protected T ToResponse<T>(T data, string error)
        {
            if(!string.IsNullOrWhiteSpace(error))
            {
                AddError(error);
            }
            return data;
        }

        protected object TryGetValueFromCell(string value, string type)
        {
            if (type == "string")
                return value;
            if (type == "int")
            {
                if (string.IsNullOrWhiteSpace(value))
                    return 0;
                try
                {
                    return Convert.ToInt32(value);
                }
                catch
                {
                    return 0;
                }
            }

            return string.Empty;
        }

        protected string GetValue<T>(T model, string fieldName)
        {
            var prop = model.GetType().GetProperty(fieldName);
            if(prop!=null)
            {
                return prop.GetValue(model) ==null ? string.Empty : prop.GetValue(model).ToString();
            }
            return string.Empty;
            
        }

    }
}
