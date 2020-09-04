using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IProfileNotificationRepository
    {
        Task<BaseResponse<bool>> CreateAsync(int profileId);
    }
}

