using BackendTestWebSocket.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BackendTestWebSocket.Controllers
{
    public class HashController : ControllerBase
    {
        public string Get(string text, string key)
        {
            return Get(new HashParam() { Text = text, Key = key }).Output;
        }

        public HashResponse Get(HashParam param)
        {
            var response = new HashResponse
            {
                Command = param.Command,
                Text = param.Text,
                Key = param.Key,
                Success = false
            };

            var encoding = new ASCIIEncoding();
            var textBytes = encoding.GetBytes(param.Text);
            var keyBytes = encoding.GetBytes(param.Key);

            Byte[] hashBytes;

            using (var hash = new HMACSHA256(keyBytes))
            {
                hashBytes = hash.ComputeHash(textBytes);
            }

            response.Success = true;
            response.Output = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return response;
        }
    }
}
