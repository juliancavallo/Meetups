using Domain.Exceptions;
using Domain.Filters;
using Domain.Requests;
using Domain.Responses;
using Meetups.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Services;
using Services.Logger;
using System;
using System.Collections.Generic;

namespace Meetups.Tests.API
{
    public class UsersControllerTest
    {
        private Mock<IUserService> userService;
        private Mock<ISecurityService> securityService;
        private Mock<ILoggerService> loggerService;
        private UsersController controller;
        private readonly int userId = 1;

        [SetUp]
        public void Setup()
        {
            userService = new Mock<IUserService>();
            loggerService = new Mock<ILoggerService>();
            securityService = new Mock<ISecurityService>();

            controller = new UsersController(loggerService.Object, userService.Object, securityService.Object);
        }

        #region Get

        [Test]
        public void Get_ReturnsOK()
        {
            var searchResultMock = new UsersSearchResponse();
            userService.Setup(x => x.Get(It.IsAny<UserSearchFilter>())).Returns(searchResultMock);

            var result = controller.Get(new UserSearchFilter());

            loggerService.Verify(x => x.LogInformation("UsersController > Get. OK"), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Get_ReturnsBadRequest()
        {
            var exception = new BadRequestException("Test Exception");
            userService.Setup(x => x.Get(It.IsAny<UserSearchFilter>())).Throws(exception);

            var result = controller.Get(new UserSearchFilter());

            loggerService.Verify(x => x.LogError("UsersController > Get. ERROR 400", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Get_Returns_InternalServerError()
        {
            var exception = new Exception("Test Exception");
            userService.Setup(x => x.Get(It.IsAny<UserSearchFilter>())).Throws(exception);

            var result = controller.Get(new UserSearchFilter());

            loggerService.Verify(x => x.LogError("UsersController > Get. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

        #region Create

        [Test]
        public void Create_ReturnsOK()
        {
            var request = new UserRequest()
            {
                Name = "name",
                LastName = "lastname",
                Password = "pwd",
                UserName = "username"
            };

            userService.Setup(x => x.Create(It.IsAny<UserRequest>())).Returns(userId);

            var result = controller.Create(request);

            loggerService.Verify(x => x.LogInformation("UsersController > Create. OK. UserId: " + userId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Create_ReturnsBadRequest()
        {
            var requestList = new List<UserRequest>()
            {
                new UserRequest(null, "test", "test", "test"),
                new UserRequest("test", null, "test", "test"),
                new UserRequest("test", "test", null, "test"),
                new UserRequest("test", "test", "test", null)
            };

            foreach (var request in requestList)
            {
                var result = controller.Create(request);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult>(result);
                Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            }

            loggerService.Verify(x => x.LogError("UsersController > Create. ERROR 400", It.IsAny<BadRequestException>()), Times.Exactly(requestList.Count));

        }

        [Test]
        public void Create_ReturnsInternalServerError()
        {
            var request = new UserRequest()
            {
                Name = "name",
                LastName = "lastname",
                Password = "pwd",
                UserName = "username"
            };
            var exception = new Exception("Test Exception");
            userService.Setup(x => x.Create(It.IsAny<UserRequest>())).Throws(exception);

            var result = controller.Create(request);

            loggerService.Verify(x => x.LogError("UsersController > Create. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

        #region Delete
        [Test]
        public void Delete_ReturnsOK()
        {
            var result = controller.Delete(userId);

            loggerService.Verify(x => x.LogInformation("UsersController > Delete. OK. UserId: " + userId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void Delete_ReturnsBadRequest()
        {
            var result = controller.Delete(0);

            loggerService.Verify(x => x.LogError("UsersController > Delete. ERROR 400", It.IsAny<BadRequestException>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Delete_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");
            userService.Setup(x => x.Delete(It.IsAny<int>())).Throws(exception);

            var result = controller.Delete(userId);

            loggerService.Verify(x => x.LogError("UsersController > Delete. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }
        #endregion

        #region Update
        [Test]
        public void Update_ReturnsOK()
        {
            var request = new UserRequest()
            {
                Name = "name",
                LastName = "lastname",
                Password = "pwd",
                UserName = "username",
                Id = userId
            };

            var result = controller.Update(request);

            loggerService.Verify(x => x.LogInformation("UsersController > Update. OK. UserId: " + userId), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void Update_ReturnsBadRequest()
        {
            var requestList = new List<UserRequest>()
            {
                new UserRequest(null, "test", "test", "test", userId),
                new UserRequest("test", null, "test", "test", userId),
                new UserRequest("test", "test", null, "test", userId),
                new UserRequest("test", "test", "test", null, userId),
                new UserRequest("test", "test", "test", null, userId),
                new UserRequest("test", "test", "test", "test")
            };

            foreach (var request in requestList)
            {
                var result = controller.Update(request);

                Assert.IsNotNull(result);
                Assert.IsInstanceOf<ActionResult>(result);
                Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
            }

            loggerService.Verify(x => x.LogError("UsersController > Update. ERROR 400", It.IsAny<BadRequestException>()), Times.Exactly(requestList.Count));
        }

        [Test]
        public void Update_ReturnsInternalServerError()
        {
            var exception = new Exception("Test Exception");
            var request = new UserRequest()
            {
                Name = "name",
                LastName = "lastname",
                Password = "pwd",
                UserName = "username",
                Id = userId
            };

            userService.Setup(x => x.Update(It.IsAny<UserRequest>())).Throws(exception);

            var result = controller.Update(request);

            loggerService.Verify(x => x.LogError("UsersController > Update. ERROR 500", exception), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }
        #endregion
    }
}
