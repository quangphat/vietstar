using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Infrastructures
{
    public class McreditApi
    {
        public string BaseUrl { get; set; }
        public string XDNCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AuthenToken { get; set; }
        public string AuthenApi { get; set; }
        public string CheckCATApi { get; set; }
        public string CheckDupApi { get; set; }
        public string CheckCICApi { get; set; }
        public string CheckStatusApi { get; set; }
        public string CheckSaleAPI { get; set; }
        public string SearchProfileApi { get; set; }
        public string GetProfileByIdApi { get; set; }
        public string CreateProfileApi { get; set; }
        public string GetFileToUploadApi { get; set; }
        public string UploadFileApi { get; set; }
        public string GetNotesApi { get; set; }
        public string AddNoteApi { get; set; }
    }
}
