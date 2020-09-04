using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface ILogRepository
    {
        Task InsertLog(string name, string content);
        Task InsertLogFromException(string name, Exception e);
    }
}

