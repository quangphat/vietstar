using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.Commons;
using VietStar.Entities.ViewModels;

namespace VietStar.Repository.Interfaces
{
    public interface IPartnerRepository
    {
        Task<List<OptionSimple>> GetsAync(int orgId);
    }
}

