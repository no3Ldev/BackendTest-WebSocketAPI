using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTestWebSocket.Models
{
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime SessionExpiry { get; set; }
    }

    public class LoginParam
    {
        public string Command { get; set; }
        public string UsernameOrEmail { get; set; }
        public string Challenge { get; set; }
    }

    public class LoginResponse : LoginParam
    {
        public bool Success { get; set; }
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public int Validity { get; set; }
        public string Remarks { get; set; }
    }
}
