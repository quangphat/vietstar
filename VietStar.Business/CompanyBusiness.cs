using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Company;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Note;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class CompanyBusiness : BaseBusiness, ICompanyBusiness
    {
        protected readonly ICompanyRepository _rpCompany;
        protected readonly INoteRepository _rpNote;
        public CompanyBusiness(ICompanyRepository companyRepository,
            INoteRepository noteRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCompany = companyRepository;
            _rpNote = noteRepository;
        }

        public async Task<CompanySql> GetByIdAsync(int id)
        {
            return await _rpCompany.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(CompanyEditModel model)
        {
            if (model == null)
            {
                return ToResponse(false,Errors.invalid_data);
            }
            var company = _mapper.Map<CompanySql>(model);
            var result = await _rpCompany.UpdateAsync(company, _process.User.Id);
            if(result.success)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var note = new NoteAddModel
                    {
                        Content = model.LastNote,
                        ProfileId = model.Id,
                        ProfileTypeId = (int)NoteType.Company,
                        UserId = _process.User.Id
                    };
                    await _rpNote.AddNoteAsync(note);
                }
            }
           

            return ToResponse(result);
        }

        public async Task<DataPaging<List<CompanyIndexModel>>> SearchsAsync(string freeText, int page, int limit)
        {
            var datas = await _rpCompany.GetsAsync(freeText, page, limit);
            if (datas == null)
            {
                return DataPaging.Create(datas, 0);
            }
            return DataPaging.Create(datas, datas.FirstOrDefault().TotalRecord);
        }
        public async Task<int> CreateAsync(CompanyAddModel model)
        {
            if(model==null)
            {
                return ToResponse(0, Errors.invalid_data);
            }
            var company = _mapper.Map<CompanySql>(model);
            var response = await _rpCompany.CreateAsync(company, _process.User.Id);
            if(response.success)
            {
                if (!string.IsNullOrWhiteSpace(model.LastNote))
                {
                    var note = new NoteAddModel
                    {
                        Content = model.LastNote,
                        ProfileId = response.data,
                        ProfileTypeId = (int)NoteType.Company,
                        UserId = _process.User.Id
                    };
                    await _rpNote.AddNoteAsync(note);
                }
            }
            return ToResponse(response);
        }
    }
}
