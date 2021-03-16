namespace BackendTestWebSocket.Models
{
    public class HashParam
    {
        public string Command { get; set; }
        public string Text { get; set; }
        public string Key { get; set; }
    }

    public class HashResponse : HashParam
    {
        public bool Success { get; set; }
        public string Output { get; set; }
    }
}
