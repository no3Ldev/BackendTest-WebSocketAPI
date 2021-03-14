using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;

namespace BackendTestWebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public BaseController(IConfiguration config)
        {
            _config = config;
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
