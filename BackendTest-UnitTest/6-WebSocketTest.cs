using BackendTestWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BackendTestUnitTest
{
    //TODO: Unresolved
    //[TestClass]
    public class F_WebSocketTest : _BaseClass
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            BaseInitialize();
        }

        [TestMethod]
        public async Task PostWebSocketRequest()
        {
            var param = new AuthenticationParam
            {
                Command = "loginSalt",
                Username = "johndoe"
            };

            var socket = new ClientWebSocket();
            var https_port = 5001;

            await socket.ConnectAsync(new System.Uri($"wss://localhost:{https_port}/ws"), CancellationToken.None);
            await socket.SendAsync(JsonSerializer.SerializeToUtf8Bytes(param), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            BaseDispose();
        }
    }
}
