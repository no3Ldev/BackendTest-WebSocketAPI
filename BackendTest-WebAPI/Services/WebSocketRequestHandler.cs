using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTestWebAPI.Services
{
    public class WebSocketRequestHandler : WebSocketHandler
    {
        private readonly Dictionary<string, string> _commands;
        private readonly IConfiguration _config;
        private static HttpClient _client;

        public WebSocketRequestHandler(WebSocketManager connectionManager, IConfiguration config) : base(connectionManager)
        {
            //command, API URI
            _commands = new Dictionary<string, string>
            {
                { "emailVerification", "api/Verify" },
                { "register", "api/Register" },
                { "checkUsername", "api/Available" },
                { "checkEmail", "api/Available" },
                { "loginSalt", "api/Authenticate" },
                { "login", "api/Login" }
            };

            _config = config;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_config.GetSection("AppSettings").GetValue<string>("base_address"))
            };
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = _connectionManager.GetId(socket);
            var content = Encoding.UTF8.GetString(buffer, 0, result.Count);

            try
            {
                var jsonObject = JObject.Parse(content);
                var response = new HttpResponseMessage();

                if (jsonObject.ContainsKey("command"))
                {
                    if (jsonObject.TryGetValue("command", StringComparison.CurrentCultureIgnoreCase, out JToken jtCommand))
                    {
                        var client = new HttpClient();
                        var param = new StringContent(content, Encoding.UTF8, "application/json");
                        var jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var commandKey = jtCommand.Value<string>();

                        if (_commands.ContainsKey(commandKey))
                        {
                            if (_commands.TryGetValue(commandKey, out string apiUri))
                            {
                                response = await _client.PostAsync(apiUri, param);

                                if (response.IsSuccessStatusCode)
                                {
                                    var strReponse = await response.Content.ReadAsStringAsync();
                                    await SendMessage(socketId, strReponse);
                                }
                            }
                        }
                    }
                }
            }
            catch (JsonReaderException) { } //invalid json, ignore request
            catch (Exception) { throw; }
        }
    }
}
