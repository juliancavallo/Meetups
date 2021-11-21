using System;
using System.Linq;
using System.Security.Claims;
using Meetups.Domain.Exceptions;
using Meetups.Domain.Filters;
using Meetups.Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Meetups.Domain.Interfaces.Services;

namespace Meetups.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MeetupsController : ControllerBase
    {
        private readonly ILoggerService logger;
        private readonly IMeetupService meetupsService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MeetupsController(IMeetupService meetupsService, ILoggerService logger, IHttpContextAccessor httpContextAccessor)
        {
            this.meetupsService = meetupsService;
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]MeetupSearchFilter filter)
        {
            try
            {
                var result = meetupsService.Get(filter);
                logger.LogInformation("MeetupsController > Get. OK");
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                logger.LogError("MeetupsController > Get. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("MeetupsController > Get. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("Create", Name = "Create")]
        public IActionResult Create(MeetupRequest meetup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(meetup.Description))
                    throw new BadRequestException("The field Description must be completed");

                if (meetup.MeetupDate == DateTime.MinValue)
                    throw new BadRequestException("The field MeetupDate must be completed");

                int userId = meetupsService.Create(meetup);

                logger.LogInformation("MeetupsController > Create. OK. MeetupId: " + userId);

                return Ok(userId);
            }
            catch(BadRequestException ex)
            {
                logger.LogError("MeetupsController > Create. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("MeetupsController > Create. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if(id < 1)
                    throw new BadRequestException("id must be greater than 0");

                meetupsService.Delete(id);

                logger.LogInformation("MeetupsController > Delete. OK. MeetupId: " + id);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("MeetupsController > Delete. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("MeetupsController > Delete. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        public IActionResult Update(MeetupRequest meetup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(meetup.Description))
                    throw new BadRequestException("The field Description must be completed");

                if (meetup.MeetupDate == DateTime.MinValue)
                    throw new BadRequestException("The field MeetupDate must be completed");

                if (meetup.Id < 1)
                    throw new BadRequestException("id must be greater than 0");

                meetupsService.Update(meetup);

                logger.LogInformation("MeetupsController > Update. OK. MeetupId: " + meetup.Id);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("MeetupsController > Update. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("MeetupsController > Update. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Join", Name = "Join")]
        public IActionResult Join(int id)
        {
            try
            {
                var loggedUserId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                meetupsService.Join(int.Parse(loggedUserId), id);

                logger.LogInformation("MeetupsController > Update. OK. Join: " + id + ", UserId: " + loggedUserId);

                return Ok();
            }
            catch (BadRequestException ex)
            {
                logger.LogError("MeetupsController > Join. ERROR 400", ex);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError("MeetupsController > Join. ERROR 500", ex);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }
}
