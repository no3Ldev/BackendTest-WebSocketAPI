using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackendTestWebAPI.Services
{
    public abstract class WebSocketHandler
    {
        protected WebSocketManager _connectionManager { get; set; }

        public WebSocketHandler(WebSocketManager connections)
        {
            _connectionManager = connections;
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

        public virtual Task OnConnected(WebSocket socket)
        {
            _connectionManager.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await _connectionManager.RemoveSocket(_connectionManager.GetId(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if (socket == null || socket.State != WebSocketState.Open)
                return;

            var buffer = new ArraySegment<byte>(Encoding.ASCII.GetBytes(message), 0, message.Length);

            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task SendMessage(string socketId, string message)
        {
            await SendMessage(_connectionManager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach (var pair in _connectionManager.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessage(pair.Value, message);
            }
        }
    }
}
