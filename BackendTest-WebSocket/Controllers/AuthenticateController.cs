using BackendTestWebSocket.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class AuthenticateController : BaseController
    {
        public AuthenticateController(IConfiguration config, AppDbContext context) : base(config, context) { }

        public async Task<AuthenticationResponse> Post(AuthenticationParam param)
        {
            var response = new AuthenticationResponse
            {
                Command = param.Command,
                Username = param.Username,
                Success = false
            };

            try
            {
                if (param.Command != "loginSalt")
                {
                    response.Remarks = "Invalid command";
                    return response;
                }

                if (!_context.Users.Any(u => u.Username == param.Username || u.Email == param.Username))
                {
                    response.Remarks = "Invalid username or email";
                    return response;
                }

                var ctrlHash = new HashController();
                var salt = ctrlHash.Get(Guid.NewGuid().ToString(), DateTime.Now.ToString());
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
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Remarks = ex.Message;
                return response;
            }
        }
    }
}
