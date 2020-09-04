using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.FileProfile;
using VietStar.Entities.ViewModels;

namespace VietStar.Business.Interfaces
{
    public interface IMediaBusiness
    {
        Task<List<FileProfile>> GetProfileFileTypeByTypeAsync(string profileType, int profileId = 0, string webRootPath = null, string mcId = null);
        Task<bool> DeleteByIdAsync(int fileId, string guidId);
        Task<object> UploadFileAsync(IFormFile file, int key, int fileId, string guildId, int profileId, int profileType, string rootPath);
        Task<object> UploadFileMcreditAsync(IFormFile file,
            string rootPath,
            int key,
            int fileId,
            string guildId,
            int profileId,
            string documentName,
            string documentCode,
            int documentId,
            int groupId,
            string mcId = null);
        Task<FileModel> UploadAsync(Stream stream, string key, string name, string webRootPath, string folder);

        Task<string> ProcessFilesToSendToMC(int portalProfileId, string rootPath);


    }

}
