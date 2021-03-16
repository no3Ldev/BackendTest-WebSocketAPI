using BackendTestWebSocket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class RegisterController : BaseController
    {
        private readonly AppDbContext _context;

        public RegisterController(IConfiguration config, AppDbContext context) : base(config)
        {
            _context = context;
        }

        public async Task<ActionResult<RegistrationResponse>> Post(RegistrationParam param)
        {
            try
            {
                if (param.Command != "register")
                    return BadRequest();

                var response = new RegistrationResponse
                {
                    Command = param.Command,
                    Username = param.Username,
                    Success = false
                };

                if (string.IsNullOrWhiteSpace(param.Username))
                {
                    response.Remarks = "Username is required";
                    return response;
                }

                if (!Regex.IsMatch(param.Username, @"^[a-zA-Z0-9]+$")) //accept only A-Z, a-z, 0-9
                {
                    response.Remarks = "Invalid characters found in Username";
                    return response;
                }

                if (_context.Users.Any(u => u.Username == param.Username || (u.Email == param.Email && !string.IsNullOrEmpty(param.Email)))) //must be unique
                {
                    response.Remarks = "Username/email has already been used";
                    return response;
                }

                if (!_context.Verifications
                    .Any(v => 
                         v.Username == param.Username && 
                         v.VerificationCode == param.VerificationCode &&
                         v.Timestamp >= DateTime.Today && 
                         v.ExpirationTime >= DateTime.Now && !v.Used))
                {
                    response.Remarks = "Verification code not found or no longer valid";
                    return response;
                }

                var ctrlHash = new HashController();
                var passwordHash = ctrlHash.Get(param.Password, GetSettings("superSecretKey"));
                var password2Hash = ctrlHash.Get(param.Password2, GetSettings("superSecretKey"));
                var password3Hash = ctrlHash.Get(param.Password3, GetSettings("superSecretKey"));

                var user = new User
                {
                    Username = param.Username,
                    DisplayName = param.DisplayName,
                    Password = passwordHash,
                    Password2 = password2Hash,
                    Password3 = password3Hash,
                    Email = param.Email,
                    VerificationCode = param.VerificationCode
                };

                _context.Users.Add(user);

                var verification = _context.Verifications
                    .Where(v => v.Username == param.Username && v.VerificationCode == param.VerificationCode && v.Timestamp >= DateTime.Today)
                    .First();

                verification.Used = true;
                _context.Entry(verification).State = EntityState.Modified;

                await _context.SaveChangesAsync().ConfigureAwait(false);

                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorCode(ex.Message);
            }
        }
    }
}
