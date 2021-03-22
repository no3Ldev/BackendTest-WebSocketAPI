using BackendTestWebSocket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace BackendTestWebSocket.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _config;
        protected readonly AppDbContext _context;

        public BaseController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        protected string GetSettings(string key)
        {
            return _config.GetSection("AppSettings").GetValue<string>(key);
        }

        protected string ObjectToJsonString(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        protected StringContent ObjectToJsonContent(object value)
        {
            return new StringContent(JsonConvert.SerializeObject(value));
        }

        protected ObjectResult ErrorCode(string message)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}
