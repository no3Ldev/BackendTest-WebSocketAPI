using BackendTestWebSocket.Controllers;
using BackendTestWebSocket.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BackendTestWebSocket.Services
{
    public class WebSocketRequestHandler : WebSocketHandler
    {
        private readonly IConfiguration _config;

        public WebSocketRequestHandler(WebSocketManager connectionManager, IConfiguration config) : base(connectionManager)
        {
            _config = config;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer, AppDbContext dbContext)
        {
            var socketId = _connectionManager.GetId(socket);
            var strContent = Encoding.UTF8.GetString(buffer, 0, result.Count);

            try
            {
                var jsonObject = JObject.Parse(strContent);
                var response = new HttpResponseMessage();

                if (jsonObject.ContainsKey("command"))
                {
                    if (jsonObject.TryGetValue("command", StringComparison.CurrentCultureIgnoreCase, out JToken jtCommand))
                    {
                        var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var commandKey = jtCommand.Value<string>();
                        var strResponse = string.Empty;

                        switch (commandKey)
                        {
                            case "emailVerification":
                                var ctrlVerify = new VerifyController(_config, dbContext);
                                var paramVerify = JsonSerializer.Deserialize<VerificationParam>(strContent, jsonOptions);
                                var responseVerify = await ctrlVerify.Post(paramVerify);
                                strResponse = JsonConvert.SerializeObject(responseVerify);
                                break;

                            case "getVerificationCode":
                                var ctrlVerificationCode = new VerifyController(_config, dbContext);
                                var paramVerificationCode = JsonSerializer.Deserialize<VerificationParam>(strContent, jsonOptions);
                                var responseVerificationCode = await ctrlVerificationCode.Get(paramVerificationCode);
                                strResponse = JsonConvert.SerializeObject(responseVerificationCode);
                                break;

                            case "register":
                                var ctrlRegister = new RegisterController(_config, dbContext);
                                var paramRegister = JsonSerializer.Deserialize<RegistrationParam>(strContent, jsonOptions);
                                var responseRegister = await ctrlRegister.Post(paramRegister);
                                strResponse = JsonConvert.SerializeObject(responseRegister);
                                break;

                            case "checkUsername":
                                var ctrlAvailable = new AvailableController(_config, dbContext);
                                var paramAvailable = JsonSerializer.Deserialize<AvailabilityParam>(strContent, jsonOptions);
                                var responseAvailable = await ctrlAvailable.Post(paramAvailable);
                                strResponse = JsonConvert.SerializeObject(responseAvailable);
                                break;

                            case "checkEmail":
                                var ctrlAvailable2 = new AvailableController(_config, dbContext);
                                var paramAvailable2 = JsonSerializer.Deserialize<AvailabilityParam>(strContent, jsonOptions);
                                var responseAvailable2 = await ctrlAvailable2.Post(paramAvailable2);
                                strResponse = JsonConvert.SerializeObject(responseAvailable2);
                                break;

                            case "loginSalt":
                                var ctrlAuthenticate = new AuthenticateController(_config, dbContext);
                                var paramAuthenticate = JsonSerializer.Deserialize<AuthenticationParam>(strContent, jsonOptions);
                                var responseAuthenticate = await ctrlAuthenticate.Post(paramAuthenticate);
                                strResponse = JsonConvert.SerializeObject(responseAuthenticate);
                                break;

                            case "hash":
                                var ctrlHash = new HashController();
                                var paramHash = JsonSerializer.Deserialize<HashParam>(strContent, jsonOptions);
                                var responseHash = ctrlHash.Get(paramHash);
                                strResponse = JsonConvert.SerializeObject(responseHash);
                                break;

                            case "login":
                                var ctrlLogin = new LoginController(_config, dbContext);
                                var paramLogin = JsonSerializer.Deserialize<LoginParam>(strContent, jsonOptions);
                                var responseLogin = await ctrlLogin.Post(paramLogin);
                                strResponse = JsonConvert.SerializeObject(responseLogin);
                                break;
                        }

                        if (!string.IsNullOrEmpty(strResponse))
                        {
                            await SendMessage(socketId, strResponse);
                        }
                    }
                }
            }
            catch (JsonReaderException) { } //invalid json, ignore request
            catch (Exception) { throw; }
        }
    }
}
