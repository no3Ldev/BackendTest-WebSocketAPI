namespace BackendTestWebAPI.Models
{
    public class RegistrationParam
    {
        public string Command { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }

    public class RegistrationResponse
    {
        public string Command { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
        public string Remarks { get; set; }
    }
}
