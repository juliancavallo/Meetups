using System;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Requests;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Logger;

namespace Meetups.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ILoggerService logger;
        private readonly IUserService userService;
        private readonly ISecurityService securityService;

        public UsersController(ILoggerService logger, IUserService userService, ISecurityService securityService)
        {
            this.logger = logger;
            this.userService = userService;
            this.securityService = securityService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] UserSearchFilter filter)
        {
            try
            {
                var result = userService.Get(filter);
                logger.LogInformation("UsersController > Get. OK");
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                logger.LogError("UsersController > Get. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("UsersController > Get. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Create(UserRequest user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Name))
                    throw new BadRequestException("The field Name must be completed");

                if (string.IsNullOrWhiteSpace(user.LastName))
                    throw new BadRequestException("The field LastName must be completed");

                if (string.IsNullOrWhiteSpace(user.UserName))
                    throw new BadRequestException("The field UserName must be completed");

                if (string.IsNullOrWhiteSpace(user.Password))
                    throw new BadRequestException("The field Password must be completed");

                user.Password = securityService.Encrypt(user.Password);

                int userId = userService.Create(user);

                logger.LogInformation("UsersController > Create. OK. UserId: " + userId);
                return Ok(userId);
            }
            catch (BadRequestException ex)
            {
                logger.LogError("UsersController > Create. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("UsersController > Create. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Update(UserRequest user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Name))
                    throw new BadRequestException("The field Name must be completed");

                if (string.IsNullOrWhiteSpace(user.LastName))
                    throw new BadRequestException("The field LastName must be completed");

                if (string.IsNullOrWhiteSpace(user.UserName))
                    throw new BadRequestException("The field UserName must be completed");

                if (string.IsNullOrWhiteSpace(user.Password))
                    throw new BadRequestException("The field Password must be completed");

                if (user.Id < 1)
                    throw new BadRequestException("id must be greater than 0");

                user.Password = securityService.Encrypt(user.Password);
                userService.Update(user);

                logger.LogInformation("UsersController > Update. OK. UserId: " + user.Id);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("UsersController > Update. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("UsersController > Update. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id < 1)
                    throw new BadRequestException("id must be greater than 0");

                userService.Delete(id);

                logger.LogInformation("UsersController > Delete. OK. UserId: " + id);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("UsersController > Delete. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("UsersController > Delete. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }

}