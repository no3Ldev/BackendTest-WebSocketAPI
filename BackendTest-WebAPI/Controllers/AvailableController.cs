using BackendTestWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace BackendTestWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableController : BaseController
    {
        private readonly AppDbContext _context;

        public AvailableController(IConfiguration config, AppDbContext context) : base(config)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<AvailabilityResponse> Post(AvailabilityParam param)
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
                    if (_context.Users.Any(u => u.Username == param.Username))
                    {
                        response.Available = false;
                    }
                }

                if (param.Command == "checkEmail")
                {
                    if (_context.Users.Any(u => u.Email == param.Email))
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
