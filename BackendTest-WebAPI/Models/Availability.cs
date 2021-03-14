namespace BackendTestWebAPI.Models
{
    public class AvailabilityParam
    {
        public string Command { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class AvailabilityResponse : AvailabilityParam
    {
        public bool Available { get; set; }
    }
}
