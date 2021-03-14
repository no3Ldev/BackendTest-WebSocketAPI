using BackendTestWebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BackendTestUnitTest
{
    public class _BaseClass
    {
        protected const string CONTENT_TYPE_TEXT = "text/plain; charset=utf-8";
        protected const string CONTENT_TYPE_JSON = "application/json; charset=utf-8";

        protected static WebApplicationFactory<Startup> _factory;
        protected static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        protected static void BaseInitialize()
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder.UseSetting("https_port", "5001"));
        }

        protected static StringContent ObjectToJsonContent(object value)
        {
            var json = JsonConvert.SerializeObject(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        //protected static async Task Send(ClientWebSocket socket, object data)
        //    => await socket.SendAsync(
        //        System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(data), 
        //        WebSocketMessageType.Text, 
        //        true, 
        //        CancellationToken.None);

        //protected static async Task Receive(ClientWebSocket socket)
        //{
        //    var buffer = new ArraySegment<byte>(new byte[2048]);
        //    do
        //    {
        //        WebSocketReceiveResult result;

        //        using var ms = new MemoryStream();

        //        do
        //        {
        //            result = await socket.ReceiveAsync(buffer, CancellationToken.None);
        //            ms.Write(buffer.Array, buffer.Offset, result.Count);
        //        }
        //        while (!result.EndOfMessage);

        //        if (result.MessageType == WebSocketMessageType.Close)
        //            break;

        //        ms.Seek(0, SeekOrigin.Begin);

        //        using var reader = new StreamReader(ms, Encoding.UTF8);

        //        var data = await reader.ReadToEndAsync(); 
        //    }
        //    while (true);
        //}

        protected static void BaseDispose()
        {
            _factory.Dispose();
        }
    }
}
