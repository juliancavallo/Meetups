using Meetups.Domain.Exceptions;
using Meetups.Domain.Filters;
using Meetups.Domain.Models.Requests;
using Meetups.Domain.Models.Responses;
using Meetups.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Meetups.Domain.Interfaces.Services;

namespace Meetups.Tests.API
{
    public class MeetupsControllerTest
    {
        private Mock<IMeetupService> meetupsService;
        private Mock<ILoggerService> loggerService;
        private Mock<IHttpContextAccessor> httpContextAccessor;
        private MeetupsController controller;
        private readonly int meetupId = 1;

        [SetUp]
        public void Setup()
        {
            meetupsService = new Mock<IMeetupService>();
            loggerService = new Mock<ILoggerService>();
            httpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, "UserName"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                }))
            };

            httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            controller = new MeetupsController(meetupsService.Object, loggerService.Object, httpContextAccessor.Object);
        }

        #region Get

        [Test]
        public void Get_ReturnsOK()
        {
            var searchResultMock = new MeetupsSearchResponse();
            meetupsService.Setup(x => x.Get(It.IsAny<MeetupSearchFilter>())).Returns(searchResultMock);

            var result = controller.Get(new MeetupSearchFilter());

            loggerService.Verify(x => x.LogInformation("MeetupsController > Get. OK"), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Get_ReturnsBadRequest()
        {
            var exception = new BadRequestException("Test Exception");
            meetupsService.Setup(x => x.Get(It.IsAny<MeetupSearchFilter>())).Throws(exception);

            var result = controller.Get(new MeetupSearchFilter());

            loggerService.Verify(x => x.LogError("MeetupsController > Get. ERROR 400", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Get_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");
            meetupsService.Setup(x => x.Get(It.IsAny<MeetupSearchFilter>())).Throws(exception);

            var result = controller.Get(new MeetupSearchFilter());

            loggerService.Verify(x => x.LogError("MeetupsController > Get. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

        #region Create
        
        [Test]
        public void Create_ReturnsOK()
        {
            var request = new MeetupRequest()
            {
                Description = "description",
                MeetupDate = DateTime.Now
            };

            meetupsService.Setup(x => x.Create(It.IsAny<MeetupRequest>())).Returns(meetupId);

            var result = controller.Create(request);

            loggerService.Verify(x => x.LogInformation("MeetupsController > Create. OK. MeetupId: " + meetupId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Create_ReturnsBadRequest()
        {
            var requestList = new List<MeetupRequest>()
            {
                new MeetupRequest()
                { 
                    MeetupDate = DateTime.Now
                },
                new MeetupRequest()
                {
                    Description = "test"
                }
            };

            meetupsService.Setup(x => x.Create(It.IsAny<MeetupRequest>())).Returns(meetupId);

            foreach(var request in requestList)
            {
                var result = controller.Create(request);
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult>(result);
                Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            }
            
            loggerService.Verify(x => x.LogError("MeetupsController > Create. ERROR 400", It.IsAny<BadRequestException>()), Times.Exactly(requestList.Count));

            
        }       

        [Test]
        public void Create_ReturnsInternalServerError()
        {
            var request = new MeetupRequest()
            {
                Description = "description",
                MeetupDate = DateTime.Now
            };
            var exception = new Exception("Test Exception");
            meetupsService.Setup(x => x.Create(It.IsAny<MeetupRequest>())).Throws(exception);

            var result = controller.Create(request);

            loggerService.Verify(x => x.LogError("MeetupsController > Create. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

        #region Delete
        [Test]
        public void Delete_ReturnsOK()
        {
            var result = controller.Delete(meetupId);

            loggerService.Verify(x => x.LogInformation("MeetupsController > Delete. OK. MeetupId: " + meetupId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void Delete_ReturnsBadRequest()
        {
            var result = controller.Delete(0);

            loggerService.Verify(x => x.LogError("MeetupsController > Delete. ERROR 400", It.IsAny<BadRequestException>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Delete_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");
            meetupsService.Setup(x => x.Delete(It.IsAny<int>())).Throws(exception);

            var result = controller.Delete(meetupId);

            loggerService.Verify(x => x.LogError("MeetupsController > Delete. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }
        #endregion

        #region Update
        [Test]
        public void Update_ReturnsOK()
        {
            var request = new MeetupRequest()
            {
                Description = "description",
                MeetupDate = DateTime.Now,
                Id = meetupId
            };

            var result = controller.Update(request);

            loggerService.Verify(x => x.LogInformation("MeetupsController > Update. OK. MeetupId: " + meetupId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void Update_ReturnsBadRequest()
        {
            var requestList = new List<MeetupRequest>()
            {
                new MeetupRequest()
                {
                    MeetupDate = DateTime.Now,
                    Id = meetupId
                },
                new MeetupRequest()
                {
                    Description = "test",
                    Id = meetupId
                }
                ,
                new MeetupRequest()
                {
                    Description = "test",
                    MeetupDate = DateTime.Now
                }
            };

            foreach (var request in requestList)
            {
                var result = controller.Update(request);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult>(result);
                Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            }

            loggerService.Verify(x => x.LogError("MeetupsController > Update. ERROR 400", It.IsAny<BadRequestException>()), Times.Exactly(requestList.Count));
        }

        [Test]
        public void Update_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");
            var request = new MeetupRequest()
            {
                Description = "description",
                MeetupDate = DateTime.Now,
                Id = meetupId
            };

            meetupsService.Setup(x => x.Update(It.IsAny<MeetupRequest>())).Throws(exception);

            var result = controller.Update(request);

            loggerService.Verify(x => x.LogError("MeetupsController > Update. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }
        #endregion

        #region Join
        [Test]
        public void Join_ReturnsOK()
        {
            var result = controller.Join(meetupId);
            var loggedUserId = httpContextAccessor.Object.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            loggerService.Verify(x => x.LogInformation("MeetupsController > Update. OK. Join: " + meetupId + ", UserId: " + loggedUserId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void Join_ReturnsBadRequest()
        {
            var exception = new BadRequestException();
            meetupsService.Setup(x => x.Join(It.IsAny<int>(), It.IsAny<int>())).Throws(exception);

            var result = controller.Join(meetupId);

            loggerService.Verify(x => x.LogError("MeetupsController > Join. ERROR 400", It.IsAny<BadRequestException>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Join_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");

            meetupsService.Setup(x => x.Join(It.IsAny<int>(), It.IsAny<int>())).Throws(exception);

            var result = controller.Join(meetupId);

            loggerService.Verify(x => x.LogError("MeetupsController > Join. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }
        #endregion
    }
}
