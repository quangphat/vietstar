using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using VietStar.Business.Interfaces;
using VietStar.Entities.Infrastructures;

namespace VietStar.Client.Controllers
{
    [Authorize]
    [Route("media")]
    public class MediaController : VietStarBaseController
    {
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IHostingEnvironment _hosting;
        public MediaController(CurrentProcess currentProcess
            , IHostingEnvironment hosting
            , IMediaBusiness mediaBusiness):base(currentProcess)
        {
            _bizMedia = mediaBusiness;
            _hosting = hosting;
        }
        [HttpGet("GetFileType/{profileType}/{profileId}")]
        public async Task<IActionResult> GetFileType(string profileType, int profileId = 0)
        {
            var result = await _bizMedia.GetProfileFileTypeByTypeAsync(profileType, profileId, _hosting.ContentRootPath);
            return ToResponse(result);
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> Upload(int key, int profileType, int profileId,int fileId, string guidId)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizMedia.UploadFileAsync(file, key:key,fileId:fileId ,guildId: guidId,profileId: profileId,profileType: profileType,rootPath: _hosting.ContentRootPath);
            return Json(result);
        }

        [HttpPost("UploadMCredit")]
        public async Task<IActionResult> UploadMCredit(int key, 
            int profileId,
            int fileId,
            string guidId,
            string docCode,
            int docId, 
            int groupId,
            string mcId = null)
        {
            var file = Request.Form.Files.FirstOrDefault();
            var result = await _bizMedia.UploadFileMcreditAsync(file, rootPath: _hosting.ContentRootPath, key: key, fileId: fileId, guildId: guidId, profileId: profileId, 
                documentName:string.Empty, documentCode: docCode,documentId:docId, groupId:groupId, mcId:mcId);
            return Json(result);
        }

        [HttpPost("delete/{fileId}/{guidId}")]
        public async Task<IActionResult> Delete(int fileId, string guidId)
        {
            
            var result = await _bizMedia.DeleteByIdAsync(fileId, guidId);
            return ToResponse(result);
        }

        [HttpGet("download-template")]
        public IActionResult DownloadTemplateFile(string fileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(_hosting.ContentRootPath + "\\wwwroot\\ImportTemplate\\" + fileName);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = fileName;
            return response;
        }

        [HttpGet("download-export")]
        public IActionResult DownloadExportFile(string fileName, string filePath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new FileContentResult(fileBytes, "application/octet-stream");
            response.FileDownloadName = fileName;
            return response;
        }
    }
}