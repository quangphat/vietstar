using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.CheckDup;
using VietStar.Entities.Company;
using VietStar.Entities.Courier;
using VietStar.Entities.Employee;
using VietStar.Entities.Mcredit;
using VietStar.Entities.Profile;

namespace VietStar.Entities.Infrastructures
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileAdd, ProfileAddSql>()
               .ForMember(a => a.ID, b => b.MapFrom(c => c.Id))
               .ForMember(a => a.Ma_Ho_So, b => b.MapFrom(c => c.Code))
               .ForMember(a => a.Ten_Khach_Hang, b => b.MapFrom(c => c.CustomerName))
               .ForMember(a => a.CMND, b => b.MapFrom(c => c.Cmnd))
               .ForMember(a => a.CMNDDay, b => b.MapFrom(c => c.CmndDay))
               .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.BirthDay))
               .ForMember(a => a.Dia_Chi, b => b.MapFrom(c => c.Address))
               .ForMember(a => a.Ma_Khu_Vuc, b => b.MapFrom(c => c.DistrictId))
               .ForMember(a => a.SDT, b => b.MapFrom(c => c.Phone))
               .ForMember(a => a.Gioi_Tinh, b => b.MapFrom(c => c.Gender))
               .ForMember(a => a.SDT2, b => b.MapFrom(c => c.SalePhone))
               .ForMember(a => a.Ngay_Nhan_Don, b => b.MapFrom(c => c.ReceiveDate))
               .ForMember(a => a.San_Pham_Vay, b => b.MapFrom(c => c.ProductId))
               .ForMember(a => a.Co_Bao_Hiem, b => b.MapFrom(c => c.IsInsurrance))
               .ForMember(a => a.So_Tien_Vay, b => b.MapFrom(c => c.LoanAmount))
               .ForMember(a => a.Ma_Trang_Thai, b => b.MapFrom(c => c.Status))
               .ForMember(a => a.Han_Vay, b => b.MapFrom(c => c.Period))
               .ForMember(a => a.Ghi_Chu, b => b.MapFrom(c => c.Comment))
               .ForMember(a => a.Ho_So_Cua_Ai, b => b.MapFrom(c => c.SaleId))
               .ForMember(a => a.Courier_Code, b => b.MapFrom(c => c.CourierId));

            CreateMap<ProfileDetail, ProfileEditView>()
               .ForMember(a => a.Id, b => b.MapFrom(c => c.ID))
               .ForMember(a => a.Code, b => b.MapFrom(c => c.Ma_Ho_So))
               .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Ten_Khach_Hang))
               .ForMember(a => a.Cmnd, b => b.MapFrom(c => c.CMND))
               .ForMember(a => a.CmndDay, b => b.MapFrom(c => c.CMNDDay))
               .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.BirthDay))
               .ForMember(a => a.Address, b => b.MapFrom(c => c.Dia_Chi))
               .ForMember(a => a.DistrictId, b => b.MapFrom(c => c.Ma_Khu_Vuc))
               .ForMember(a => a.ProvinceId, b => b.MapFrom(c => c.ProvinceId))
               .ForMember(a => a.Phone, b => b.MapFrom(c => c.SDT))
               .ForMember(a => a.Gender, b => b.MapFrom(c => c.Gioi_Tinh))
               .ForMember(a => a.SalePhone, b => b.MapFrom(c => c.SDT2))
               .ForMember(a => a.ReceiveDate, b => b.MapFrom(c => c.Ngay_Nhan_Don))
               .ForMember(a => a.ProductId, b => b.MapFrom(c => c.San_Pham_Vay))
               .ForMember(a => a.PartnerId, b => b.MapFrom(c => c.PartnerId))
               .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.Courier_Code))
               .ForMember(a => a.LoanAmount, b => b.MapFrom(c => c.So_Tien_Vay))
               .ForMember(a => a.Status, b => b.MapFrom(c => c.Ma_Trang_Thai))
               .ForMember(a => a.Period, b => b.MapFrom(c => c.Han_Vay))
               .ForMember(a => a.Comment, b => b.MapFrom(c => c.Ghi_Chu))
               .ForMember(a => a.SaleId, b => b.MapFrom(c => c.Ho_So_Cua_Ai))
               .ForMember(a => a.CourierId, b => b.MapFrom(c => c.Courier_Code));
            CreateMap<CheckDupAddModel, CheckDupAddSql>();
            CreateMap<CheckDupEditModel, CheckDupAddSql>();
            CreateMap<CompanyAddModel, CompanySql>();
            CreateMap<CompanyEditModel, CompanySql>();
            CreateMap<CourierAddModel, CourierSql>();
            CreateMap<CourierUpdateModel, CourierSql>();

            #region mcredit
            CreateMap<ProfileGetByIdResponseObj, MCredit_TempProfile>()
             .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Name))
             .ForMember(a => a.MCId, b => b.MapFrom(c => c.Id))
             .ForMember(a => a.Hometown, b => b.MapFrom(c => c.HomeTown))
             .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.Bod))
             .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
             .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
             .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CccdNumber))
             .ForMember(a => a.IssueDate, b => b.MapFrom(c => c.IdDate))
             .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddrSame))
             .ForMember(a => a.ProvinceId, b => b.MapFrom(c => c.CityId))
             .ForMember(a => a.Address, b => b.MapFrom(c => c.LocSignAddr))
             .ForMember(a => a.Hometown, b => b.MapFrom(c => c.HomeTown));

            CreateMap<MCredit_TempProfileAddModel, MCredit_TempProfile>();
            CreateMap<MCredit_TempProfile, MCProfilePostModel>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.CustomerName))
                .ForMember(a => a.HomeTown, b => b.MapFrom(c => c.Hometown))
                .ForMember(a => a.Bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
                .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
                .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
                .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CCCDNumber))
                .ForMember(a => a.IdNumberDate, b => b.MapFrom(c => c.IssueDate.ToShortDateString()))
                .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddr))
                .ForMember(a => a.CityId, b => b.MapFrom(c => c.ProvinceId))
                .ForMember(a => a.ProductCode, b => b.MapFrom(c => c.ProductCode))
                .ForMember(a => a.LoanPeriodCode, b => b.MapFrom(c => c.LoanPeriodCode))
                .ForMember(a => a.LoanMoney, b => b.MapFrom(c => c.LoanMoney.ToString()))
                .ForMember(a => a.LocSignCode, b => b.MapFrom(c => c.LocSignCode))
                .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.IsInsurrance))
                .ForMember(a => a.SaleId, b => b.MapFrom(c => c.SaleId))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
                ;

            CreateMap<MCredit_TempProfileAddModel, MCProfilePostModel>()
              .ForMember(a => a.Name, b => b.MapFrom(c => c.CustomerName))
              .ForMember(a => a.HomeTown, b => b.MapFrom(c => c.Hometown))
              .ForMember(a => a.Bod, b => b.MapFrom(c => c.BirthDay.ToShortDateString()))
              .ForMember(a => a.Phone, b => b.MapFrom(c => c.Phone))
              .ForMember(a => a.IdNumber, b => b.MapFrom(c => c.IdNumber))
              .ForMember(a => a.CCCDNumber, b => b.MapFrom(c => c.CCCDNumber))
              .ForMember(a => a.IdNumberDate, b => b.MapFrom(c => c.IssueDate.ToShortDateString()))
              .ForMember(a => a.IsAddr, b => b.MapFrom(c => c.IsAddr))
              .ForMember(a => a.CityId, b => b.MapFrom(c => c.ProvinceId))
              .ForMember(a => a.ProductCode, b => b.MapFrom(c => c.ProductCode))
              .ForMember(a => a.LoanPeriodCode, b => b.MapFrom(c => c.LoanPeriodCode))
              .ForMember(a => a.LoanMoney, b => b.MapFrom(c => c.LoanMoney.ToString()))
              .ForMember(a => a.LocSignCode, b => b.MapFrom(c => c.LocSignCode))
              .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.IsInsurrance))
              .ForMember(a => a.SaleId, b => b.MapFrom(c => c.SaleId))
              .ForMember(a => a.Status, b => b.MapFrom(c => c.Status));
            CreateMap<CheckSaleObj, UpdateSaleModel>()
               .ForMember(a => a.SaleName, b => b.MapFrom(c => c.name))
               .ForMember(a => a.SaleNumber, b => b.MapFrom(c => c.idNumber))
               .ForMember(a => a.SaleId, b => b.MapFrom(c => c.id));

            CreateMap<UserCreateModel, UserSql>();
            CreateMap<UserEditModel, UserSql>();

            #endregion
        }
    }
}
