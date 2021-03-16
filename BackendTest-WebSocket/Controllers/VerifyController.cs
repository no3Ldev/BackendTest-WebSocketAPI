using BackendTestWebSocket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class VerifyController : BaseController
    {
        public VerifyController(IConfiguration config, AppDbContext context) : base(config, context) { }

        public async Task<VerificationResponse> Post(VerificationParam param)
        {
            var response = new VerificationResponse
            {
                Command = param.Command,
                Success = false
            };

            try
            {
                if (param.Command != "emailVerification")
                {
                    response.Remarks = "Invalid command";
                    return response;
                }

                if (_context.Users.Any(u => u.Username == param.Username || u.Email == param.Email))
                {
                    response.Remarks = "Username/email has already been used";
                    return response;
                }

                var verifications = _context.Verifications //get current verifications of the day
                    .OrderByDescending(a => a.Id)
                    .Where(a => a.Username == param.Username && a.Timestamp.Date == DateTime.Now.Date)
                    .AsNoTracking()
                    .ToList();

                var maxVerifications = int.Parse(GetSettings("verification_max"));
                
                if (verifications.Count >= maxVerifications)
                {
                    response.Remarks = "Verification request limit exceeded";
                    return response; 
                }

                var ctrlHash = new HashController();
                var newVerificationCode = ctrlHash.Get(param.Email, DateTime.Now.ToString()).Substring(0, 6).ToUpper(); //get first 6 characters only
                var validity = int.Parse(GetSettings("verification_expiry")); //in seconds

                var auth = new Verification()
                {
                    Username = param.Username,
                    VerificationCode = newVerificationCode,
                    Timestamp = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddSeconds(validity),
                    Used = false
                };

                _context.Verifications.Add(auth);

                await _context.SaveChangesAsync().ConfigureAwait(false);

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Remarks = ex.Message;
                return response;
            }
        }

        public async Task<VerificationResponse> Get(VerificationParam param)
        {
            var response = new VerificationResponse
            {
                Command = param.Command,
                Success = false
            };

            var verification = await _context.Verifications //get latest verification code
                .OrderByDescending(a => a.Id)
                .Where(a => a.Username == param.Username && a.Timestamp >= DateTime.Today && a.ExpirationTime >= DateTime.Now)
                .FirstOrDefaultAsync();

            if (verification != null)
            {
                response.Success = true;
                response.Remarks = verification.VerificationCode;
            }

            return response;
        }
    }
}
