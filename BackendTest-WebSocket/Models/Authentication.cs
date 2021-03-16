using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTestWebSocket.Models
{
    public class Authentication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public int Validity { get; set; }
        public string Salt { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime ExpirationTime { get; set; }
    }

    public class AuthenticationParam
    {
        public string Command { get; set; }
        public string Username { get; set; } //UsernameOrEmail
    }

    public class AuthenticationResponse : AuthenticationParam
    {
        public int Validity { get; set; }
        public string Salt { get; set; }
        public bool Success { get; set; }
        public string Remarks { get; set; }
    }
}
