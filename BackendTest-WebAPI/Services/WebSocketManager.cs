using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BackendTestWebAPI.Services
{
    public class WebSocketManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets;

        public WebSocketManager()
        {
            _sockets = new ConcurrentDictionary<string, WebSocket>();
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(Guid.NewGuid().ToString(), socket);
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out WebSocket socket);

            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
        }
    }
}
