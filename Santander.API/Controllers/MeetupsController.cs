using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.Logger;

namespace Santander.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MeetupsController : ControllerBase
    {
        private readonly ILoggerService logger;
        private readonly IMeetupService meetupsService;

        public MeetupsController(IMeetupService meetupsService, ILoggerService logger)
        {
            this.meetupsService = meetupsService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Meetup>> Get([FromQuery]MeetupSearchFilter filter)
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
        public ActionResult Create(MeetupRequest meetup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(meetup.Description))
                    throw new BadRequestException("The field Description must be completed");

                if (meetup.MeetupDate == null)
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
        public ActionResult Delete(int id)
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
        public ActionResult Update(MeetupRequest meetup)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(meetup.Description))
                    throw new BadRequestException("The field Description must be completed");

                if (meetup.MeetupDate == null)
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
        public ActionResult Join(int id)
        {
            try
            {
                var user = HttpContext.User.Identity.Name;
                /*Search meetup by id and add logged user to attendees*/
                logger.LogInformation("MeetupsController > Update. OK. MeetupId: " + id + ", UserId: " + 0);

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
