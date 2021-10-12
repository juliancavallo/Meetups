using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace Santander.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IUserService userService;
        private readonly ISecurityService securityService;

        public UsersController(ILogger<UsersController> logger, IUserService userService, ISecurityService securityService)
        {
            this.logger = logger;
            this.userService = userService;
            this.securityService = securityService;
        }

        [HttpGet]
        public ActionResult Get([FromQuery]UserSearchFilter filter)
        {
            try
            {
                return Ok(userService.Get(filter));
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult Create(UserRequest user)
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

                return Ok(userService.Create(user));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public ActionResult Update(UserRequest user)
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
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id < 1)
                    throw new BadRequestException("id must be greater than 0");

                userService.Delete(id);
                return Ok();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }

}