using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BackendTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {
        [HttpGet("{text}/{key}")]
        public string Get(string text, string key)
        {
            var encoding = new ASCIIEncoding();
            var textBytes = encoding.GetBytes(text);
            var keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
