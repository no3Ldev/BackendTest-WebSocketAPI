using BackendTestWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : BaseController
    {
        private readonly AppDbContext _context;

        public AuthenticateController(IConfiguration config, AppDbContext context) : base(config)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AuthenticationResponse>> Post(AuthenticationParam param)
        {
            try
            {
                if (param.Command != "loginSalt")
                    return BadRequest();

                var response = new AuthenticationResponse
                {
                    Command = param.Command,
                    Username = param.Username,
                    Success = false
                };

                if (!_context.Users.Any(u => u.Username == param.Username || u.Email == param.Username))
                {
                    response.Remarks = "Invalid username or email";
                    return response;
                }

                var salt = "cbb4a64006378ec261840d39ab6cc76048f3dad16e19b7db508fb11ba4594c51"; //fixed for now
                var validity = int.Parse(GetSettings("salt_expiry")); //in seconds

                var authFound = _context.Authentications
                    .OrderByDescending(a => a.Id)
                    .Any(a => a.Username == param.Username && a.ExpirationTime >= DateTime.Now);

                if (!authFound) //create new record only if there is no current valid salt
                {
                    var auth = new Authentication()
                    {
                        Username = param.Username,
                        Validity = validity,
                        Salt = salt,
                        Timestamp = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddSeconds(validity)
                    };

                    _context.Authentications.Add(auth);

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                
                response.Validity = validity;
                response.Salt = salt;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorCode(ex.Message);
            }
        }
    }
}
