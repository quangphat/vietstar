using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IConfigBusiness
    {
        Task<List<IDictionary<string, object>>> ExcuteQueryAsync(StringModel model);
    }
}
