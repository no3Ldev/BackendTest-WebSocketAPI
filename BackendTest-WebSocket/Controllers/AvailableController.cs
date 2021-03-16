using BackendTestWebSocket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BackendTestWebSocket.Controllers
{
    public class AvailableController : BaseController
    {
        private readonly AppDbContext _context;

        public AvailableController(IConfiguration config, AppDbContext context) : base(config)
        {
            _context = context;
        }

        public async Task<ActionResult<AvailabilityResponse>> Post(AvailabilityParam param)
        {
            try
            {
                if (param.Command != "checkUsername" && param.Command != "checkEmail")
                    return BadRequest();

                var response = new AvailabilityResponse
                {
                    Command = param.Command,
                    Username = param.Username,
                    Email = param.Email,
                    Available = true
                };

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
                return ErrorCode(ex.Message);
            }
        }
    }
}
