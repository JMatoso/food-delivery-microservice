using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FoodDelivery.Web.Services.Client
{
    public interface IClientApp
    {
        Task<ReturnStatus> DeleteAsync(string url, string headerValue, string token);
        Task<ReturnStatus> GetAsync<T>(string url, string headerValue, string token);
        Task<ReturnStatus> LoginAsync(Login data, string url);
        Task<ReturnStatus> PostAsync<T>(object data, string url, string headerValue, string token);
        Task<object> PutAsync<T>(object data, string url, string headerValue, string token);
        Task<string> Upload(IFormFile image);
    }

    public class ClientApp : IClientApp
    {
        private readonly HttpClient _client;
        private readonly ILogger<ClientApp> _logger;
        private readonly IWebHostEnvironment _web;
        public ClientApp(
            HttpClient client,
            ILogger<ClientApp> logger,
            IWebHostEnvironment web)
        {
            _client = client;
            _logger = logger;
            _web = web;
        }

        #region Configure
        void ConfigureHeaders(string value, string token)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add
            (
                new MediaTypeWithQualityHeaderValue(value == string.Empty ? "application/json" : value)
            );

            if(!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
        #endregion

        #region Actions
        public async Task<ReturnStatus> GetAsync<T>(string url, string headerValue, string token)
        {
            try
            {
                ConfigureHeaders(headerValue, token);

                var responseMessage = await _client.GetAsync(url);
                string content = responseMessage.Content.ReadAsStringAsync().Result;

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ReturnStatus 
                    {
                        Code = (int)responseMessage.StatusCode,
                        Returned = JsonConvert.DeserializeObject<T>(content)                       
                    };
                }

                return new ReturnStatus { Code = (int)responseMessage.StatusCode };
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new ReturnStatus { Code = 400 };
            }
        }

        public async Task<ReturnStatus> PostAsync<T>(object data, string url, string headerValue, string token)
        {
            try
            {
                ConfigureHeaders(headerValue, token);

                HttpResponseMessage responseMessage = await _client.PostAsync(url,
                    new StringContent(
                        JsonConvert.SerializeObject((T)data), Encoding.UTF8, "application/json"));

                string content = responseMessage.Content.ReadAsStringAsync().Result;

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ReturnStatus 
                    {
                        Code = (int)responseMessage.StatusCode,
                        Returned = content                       
                    };
                }

                return new ReturnStatus { Code = (int)responseMessage.StatusCode };
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new ReturnStatus { Code = 400 };
            }
        }

        public async Task<ReturnStatus> LoginAsync(Login data, string url)
        {
            try
            {
                ConfigureHeaders("", "");

                HttpResponseMessage responseMessage = await _client.PostAsync(url,
                    new StringContent(
                        JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

                string content = responseMessage.Content.ReadAsStringAsync().Result;

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ReturnStatus
                    {
                        Returned = JsonConvert.DeserializeObject<TokenReturned>(content),
                        Code = (int)HttpStatusCode.OK
                    };
                }

                return new ReturnStatus { Code = (int)responseMessage.StatusCode };
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new ReturnStatus { Code = 400 };
            }
        }

        public async Task<object> PutAsync<T>(object data, string url, string headerValue, string token)
        {
            try
            {
                ConfigureHeaders(headerValue, token);

                HttpResponseMessage responseMessage = await _client.PutAsync(url,
                    new StringContent(
                        JsonConvert.SerializeObject((T)data), Encoding.UTF8, "application/json"));

                string content = responseMessage.Content.ReadAsStringAsync().Result;

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return (int)HttpStatusCode.OK;
                }

                return (int)responseMessage.StatusCode;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return 400;
            }
        }

        public async Task<ReturnStatus> DeleteAsync(string url, string headerValue, string token)
        {
            try
            {
                ConfigureHeaders(headerValue, token);

                var responseMessage = await _client.DeleteAsync(url);
                string content = responseMessage.Content.ReadAsStringAsync().Result;

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ReturnStatus 
                    {
                        Code = (int)responseMessage.StatusCode,
                        Returned = content                       
                    };
                }

                return new ReturnStatus { Code = (int)responseMessage.StatusCode };
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return new ReturnStatus { Code = 400 };
            }
        }

        public async Task<string> Upload(IFormFile image)
        {
            string uniqueFileName = null;

            if(image != null)
            {
                string path = Path.Combine(_web.WebRootPath, "images/");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(path, uniqueFileName);

                using(var fs = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fs);
                }
            }

            return uniqueFileName;
        }

        #endregion
    }
}
