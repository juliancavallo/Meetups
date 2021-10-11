using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;

namespace Santander.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetupsController : ControllerBase
    {
        private readonly ILogger<MeetupsController> logger;
        private readonly IMeetupsService meetupsService;

        public MeetupsController(ILogger<MeetupsController> logger, IMeetupsService meetupsService)
        {
            this.meetupsService = meetupsService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Meetup>> Get()
        {
            try
            {
                return Ok(meetupsService.Get());
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public ActionResult Create(MeetupRequest meetup)
        {
            try
            {
                return Ok(meetupsService.Create(meetup));
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
                meetupsService.Delete(id);
                return Ok();
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
                meetupsService.Update(meetup);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

    }
}
