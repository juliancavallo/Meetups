using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Santander.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IUserService userService;
        private readonly ISecurityService securityService;
        private readonly ILogger<SecurityController> logger;

        public SecurityController(IConfiguration config, IUserService userService, ISecurityService securityService)
        {
            this.config = config;
            this.userService = userService;
            this.securityService = securityService;
        }

        [HttpPost]
        [Route("Login", Name ="Login")]
        public ActionResult Login(LoginRequest loginDetails)
        {
            bool result = ValidateUser(loginDetails);
            if (result)
            {
                var tokenString = GenerateJWT();
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateJWT()
        {
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(expires: expiry, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        private bool ValidateUser(LoginRequest loginDetails)
        {
            loginDetails.Password = securityService.Encrypt(loginDetails.Password);
            return userService.ValidateLogin(loginDetails);
        }
    }
}
