using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace Santander.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetupsController : ControllerBase
    {
        private readonly ILogger<MeetupsController> logger;
        private readonly IMeetupService meetupsService;

        public MeetupsController(ILogger<MeetupsController> logger, IMeetupService meetupsService)
        {
            this.meetupsService = meetupsService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Meetup>> Get([FromQuery]MeetupSearchFilter filter)
        {
            try
            {
                return Ok(meetupsService.Get(filter));
            }
            catch (Exception ex)
            {
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

                return Ok(meetupsService.Create(meetup));
            }
            catch(BadRequestException ex)
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
                if(id < 1)
                    throw new BadRequestException("id must be greater than 0");

                meetupsService.Delete(id);
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

        [HttpPost]
        [Route("Join", Name = "Join")]
        public ActionResult Join(int id)
        {
            try
            {
                /*Search meetup by id and add logged user to attendees*/
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
