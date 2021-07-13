using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Web.Services.Client
{
    public interface IClientApp
    {
        Task<ReturnStatus> DeleteAsync(string url, string token = "", string headerValue = "application/json");
        Task<ReturnStatus> GetAsync<T>(string url, string token = "", string headerValue = "application/json");
        Task<ReturnStatus> LoginAsync(Login data, string url);
        Task<ReturnStatus> PostAsync<T>(object data, string url, string token = "", string headerValue = "application/json");
        Task<ReturnStatus> PutAsync<T>(object data, string url, string token = "", string headerValue = "application/json");
        Task<string> Upload(IFormFile image);
    }
}
