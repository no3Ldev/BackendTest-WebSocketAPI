using BackendTestWebSocket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class AvailableController : BaseController
    {
        public AvailableController(IConfiguration config, AppDbContext context) : base(config, context) { }

        public async Task<AvailabilityResponse> Post(AvailabilityParam param)
        {
            var response = new AvailabilityResponse
            {
                Command = param.Command,
                Username = param.Username,
                Email = param.Email,
                Available = true
            };

            try
            {
                if (param.Command != "checkUsername" && param.Command != "checkEmail")
                {
                    response.Remarks = "Invalid command";
                    return response;
                }

                if (param.Command == "checkUsername")
                {
                    if (await _context.Users.AnyAsync(u => u.Username == param.Username))
                    {
                        response.Available = false;
                    }
                }

                if (param.Command == "checkEmail")
                {
                    if (await _context.Users.AnyAsync(u => u.Email == param.Email))
                    {
                        response.Available = false;
                    }
                }
                
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
