using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VietStar.Entities.HttpModels;
using VietStar.Repository.Interfaces;

namespace HttpService
{
    public static class CoreApiClient
    {
        public static async Task<ExternalApiResponseModel<T>> GetAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", string headerContentType = null, object param = null, ILogRepository rpLog = null)
        {
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, headerContentType, param, rpLog: rpLog);
            return response;
        }
        public static async Task<ExternalApiResponseModel<T>> PostAsync<T>(this HttpClient httpClient, HttpRequestMessage requestMessage
            , string baseUrl, string path = "/", string headerContentType = null, object param = null, object data = null, ILogRepository rpLog = null)
        {
            requestMessage.Method = HttpMethod.Post;
            var response = await httpClient.Call<T>(requestMessage, baseUrl, path, headerContentType, param, data, rpLog: rpLog);
            return response;
        }
        private static async Task<ExternalApiResponseModel<T>> Call<T>(this HttpClient httpClient,
             HttpRequestMessage requestMessage, string baseUrl, string path = "/", string headerContentType = null, object param = null, object data = null, ILogRepository rpLog = null)
        {
            //if (param != null)
            //    path = path.AddQuery(param);
            if (path[0] != '/')
                path = $"/{path}";
            var url = $"{baseUrl}{path}";
            requestMessage.RequestUri = new Uri(url);
            string json = null;

            HttpContent content = null;

            if (data != null)
                if (data is string)
                    content = new StringContent((string)data, Encoding.UTF8, "application/json");
                else if (data is IDictionary<string, object>)
                {
                    var formData = new MultipartFormDataContent();

                    foreach (var pair in data as IDictionary<string, object>)
                        if (pair.Value is byte[])
                            formData.Add(new ByteArrayContent(pair.Value as byte[]), pair.Key, pair.Key);
                        else
                            formData.Add(new StringContent(pair.Value.ToString()), pair.Key);

                    content = formData;
                }
                else
                {
                    json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    //json = "{\"str\":\"ddddddddddd\",\"status\":\"0\",\"page\":1,\"type\":\"draft\",\"token\":\"7ac1e4d882b98a0086080706e354cf4c\"} ";
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            try
            {
                if (rpLog != null)
                    await rpLog.InsertLog($"mcredit-{path}", json);
            }
            catch
            {

            }
            var signature = string.Empty;
            var originalData = string.Empty;
            if (requestMessage.Method == HttpMethod.Get)
            {
                var list = new List<string>();

                if (url.Contains("?"))
                    foreach (var q in url.Split('?')[1].Split('&'))
                        if (q.Contains("="))
                            list.Add(q.Split('=')[1]);

                originalData = string.Join(string.Empty, list);
            }
            else if (data != null)
                originalData = json;

            if (string.IsNullOrWhiteSpace(originalData))
                originalData = string.Empty;

            if (headerContentType != "multipart/form-data")
                requestMessage.Content = content;
            if (!string.IsNullOrWhiteSpace(headerContentType))
            {
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(headerContentType);
            }
            var newRequest = await requestMessage.CloneAsync();
            try
            {
                using (var response = await httpClient.SendAsync(newRequest).ConfigureAwait(false))
                {
                    if (response == null)
                    {
                        return new VietStar.Entities.HttpModels.ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, "Không có dữ liệu");
                    }
                    var responseData = response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Found
                        ? response.Headers.Location.AbsoluteUri
                        : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var result = JsonConvert.DeserializeObject<T>(responseData);

                    return new ExternalApiResponseModel<T>(result, response.StatusCode, null);
                }
            }
            catch (Exception e)
            {
                return new ExternalApiResponseModel<T>(default(T), HttpStatusCode.NoContent, string.IsNullOrWhiteSpace(e.Message) ? e.InnerException.Message : e.Message);
            }
        }
        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync().ConfigureAwait(false),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        public static async Task<HttpContent> CloneAsync(this HttpContent content)
        {
            if (content == null) return null;

            var ms = new MemoryStream();
            await content.CopyToAsync(ms).ConfigureAwait(false);
            ms.Position = 0;

            var clone = new StreamContent(ms);
            foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
            {
                clone.Headers.Add(header.Key, header.Value);
            }
            return clone;
        }
        //public static string AddQuery(this string path, object obj)
        //{
        //    if (path == null || obj == null)
        //        return path;

        //    return QueryHelpers.AddQueryString(path, obj.ToKeyPairs().ToDictionary(m => m.Key, m => m.Value.ToString()));
        //}
        public static IEnumerable<KeyValuePair<string, object>> ToKeyPairs(this object obj)
        {
            if (obj == null)
                yield break;

            foreach (var property in obj.GetType().GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                    yield return new KeyValuePair<string, object>(property.Name, value);
            }
        }
    }
}
