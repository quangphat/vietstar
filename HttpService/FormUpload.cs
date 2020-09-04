using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.HttpModels;
using VietStar.Repository.Interfaces;

namespace HttpService
{
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static async Task<ExternalApiResponseModel<T>> MultipartFormPost<T>(string postUrl,
            string userAgent, 
            Dictionary<string, object> postParameters, 
            string headerkey, string headervalue, 
            string token, 
            ILogRepository rpLog = null)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);


            using (var webResponse = await PostForm<T>(postUrl, userAgent, contentType, formData, headerkey, headervalue, token))
            {
                if (webResponse == null)
                {
                    return new ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, "No data");
                }
                if (webResponse.StatusCode == HttpStatusCode.Moved || webResponse.StatusCode == HttpStatusCode.Found)
                {
                    return new ExternalApiResponseModel<T>(default(T), webResponse.StatusCode, webResponse.ResponseUri.AbsoluteUri);
                }

                using (StreamReader responseReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var response = await responseReader.ReadToEndAsync();
                    var result = JsonConvert.DeserializeObject<T>(response);
                    return new ExternalApiResponseModel<T>(result, webResponse.StatusCode, null);
                }

            }
        }
        private static async Task<HttpWebResponse> PostForm<T>(string postUrl, string userAgent, string contentType, byte[] formData, string headerkey, string headervalue, string token)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                return null;
            }

            // Set up the request properties.  
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            request.Headers.Add(headerkey, headervalue);
            using (Stream requestStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            var response = (await request.GetResponseAsync()) as HttpWebResponse;
            return response;
        }
        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {

                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter) // to check if parameter if of file type   
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream  
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.  
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline  
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]  
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

    }
    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileParameter(byte[] file) : this(file, null) { }
        public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
        public FileParameter(byte[] file, string filename, string contenttype)
        {
            File = file;
            FileName = filename;
            ContentType = contenttype;
        }
    }
}
