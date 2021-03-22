using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendTestWebSocket.Models
{
    public class Verification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string VerificationCode { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool Used { get; set; }
    }

    public class VerificationParam
    {
        public string Command { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }

    public class VerificationResponse
    {
        public string Command { get; set; }
        public bool Success { get; set; }
        public string Remarks { get; set; }
    }
}
