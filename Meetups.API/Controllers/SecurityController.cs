using Meetups.Domain.Exceptions;
using Meetups.Domain.Models.Entities;
using Meetups.Domain.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Meetups.Domain.Interfaces.Services;

namespace Meetups.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IUserService userService;
        private readonly ISecurityService securityService;
        private readonly ILoggerService logger;

        public SecurityController(IConfiguration config, IUserService userService, ISecurityService securityService, ILoggerService logger)
        {
            this.config = config;
            this.userService = userService;
            this.securityService = securityService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Login", Name ="Login")]
        public IActionResult Login(LoginRequest loginDetails)
        {
            try
            {
                var user = ValidateUser(loginDetails);
                if (user != null)
                {
                    var tokenString = GenerateJWT(user);
                    logger.LogInformation("SecurityController > Login. OK. Token = " + tokenString);
                    return Ok(new { token = tokenString });
                }
                else
                {
                    logger.LogError("SecurityController > Login. ERROR 401");
                    return Unauthorized();
                }
            }
            catch (BadRequestException ex)
            {
                logger.LogError("SecurityController > Login. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("SecurityController > Login. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private string GenerateJWT(User user)
        {
            var expiry = DateTime.Now.AddHours(4);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private User ValidateUser(LoginRequest loginDetails)
        {
            loginDetails.Password = securityService.Encrypt(loginDetails.Password);
            return userService.ValidateLogin(loginDetails);
        }
    }
}
