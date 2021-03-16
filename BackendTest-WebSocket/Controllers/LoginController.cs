using BackendTestWebSocket.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(IConfiguration config, AppDbContext context) : base(config, context) { }

        public async Task<LoginResponse> Post(LoginParam param)
        {
            var response = new LoginResponse
            {
                Command = param.Command,
                UsernameOrEmail = param.UsernameOrEmail,
                Challenge = param.Challenge,
                Success = false
            };

            try
            {
                if (param.Command != "login")
                {
                    response.Remarks = "Invalid command";
                    return response;
                }

                var isActualUsername = false;
                var actualUsername = _context.Users //find user on username and email
                    .Where(u => u.Username == param.UsernameOrEmail || u.Email == param.UsernameOrEmail)
                    .Select(u => u.Username)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(actualUsername))
                {
                    response.Remarks = "Invalid username or email";
                    return response;
                }

                if (param.UsernameOrEmail == actualUsername)
                {
                    isActualUsername = true;
                }

                var auth = _context.Authentications
                    .OrderByDescending(a => a.Id)
                    .Where(a => a.Username == actualUsername && a.ExpirationTime >= DateTime.Now)
                    .FirstOrDefault();

                if (auth != null) //check if salt is still valid
                {
                    var user = _context.Users.FirstOrDefault(u => u.Username == actualUsername);

                    var ctrlHash = new HashController();
                    var hashedPassword = isActualUsername ? user.Password : user.Password2; //key is username or email
                    var expectedChallenge = ctrlHash.Get(hashedPassword, auth.Salt);

                    if (param.Challenge == expectedChallenge) //check if password/challenge is valid
                    {
                        var sessionId = ctrlHash.Get(param.UsernameOrEmail, DateTime.Now.ToString()); //TODO - get actual session id
                        var sessionExpiry = int.Parse(GetSettings("session_expiry")); //in seconds

                        var login = new Login()
                        {
                            Username = actualUsername,
                            SessionId = sessionId,
                            Timestamp = DateTime.Now,
                            SessionExpiry = DateTime.Now.AddSeconds(sessionExpiry)
                        };

                        _context.Logins.Add(login);

                        await _context.SaveChangesAsync().ConfigureAwait(false);

                        response.Success = true;
                        response.SessionId = sessionId;
                        response.UserId = user.Id;
                        response.Validity = auth.Validity;

                        return response;
                    }
                    else
                    {
                        response.Remarks = "Login challenge invalid or wrong password";
                        return response;
                    }
                }
                else
                {
                    response.Remarks = "Login salt not found or no longer valid";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Remarks = ex.Message;
                return response;
            }
        }
    }
}
