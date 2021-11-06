using Domain.Exceptions;
using Domain.Models;
using Domain.Requests;
using Meetups.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Services;
using Services.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Tests.API
{
    public class SecurityControllerTest
    {
        private Mock<IConfiguration> config;
        private Mock<IUserService> userService;
        private Mock<ISecurityService> securityService;
        private Mock<ILoggerService> loggerService;
        private SecurityController controller;

        [SetUp]
        public void Setup()
        {
            config = new Mock<IConfiguration>();
            userService = new Mock<IUserService>();
            securityService = new Mock<ISecurityService>();
            loggerService = new Mock<ILoggerService>();

            config.Setup(x => x["Jwt:Key"]).Returns("JWTSecretKey123456789");

            controller = new SecurityController(config.Object, userService.Object, securityService.Object, loggerService.Object);
        }

        [Test]
        public void Login_ReturnsOK()
        {
            var request = new LoginRequest()
            {
                Password = "pass",
                User = "user"
            };

            var user = new User()
            {
                UserName = "user",
                Id = 1
            };

            userService.Setup(x => x.ValidateLogin(It.IsAny<LoginRequest>())).Returns(user);

            var result = controller.Login(request);

            var token = ((ObjectResult)result).Value.GetType().GetProperty("token").GetValue(((ObjectResult)result).Value, null);

            loggerService.Verify(x => x.LogInformation("SecurityController > Login. OK. Token = " + token), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Login_ReturnsBadRequest()
        {
            var request = new LoginRequest()
            {
                Password = "pass",
                User = "user"
            };
            var exception = new BadRequestException("Test Exception");

            userService.Setup(x => x.ValidateLogin(It.IsAny<LoginRequest>())).Throws(exception);

            var result = controller.Login(request);

            loggerService.Verify(x => x.LogError("SecurityController > Login. ERROR 400", It.IsAny<BadRequestException>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void Login_ReturnsInternalServerError()
        {
            var request = new LoginRequest()
            {
                Password = "pass",
                User = "user"
            };
            var exception = new Exception("Test Exception");

            userService.Setup(x => x.ValidateLogin(It.IsAny<LoginRequest>())).Throws(exception);

            var result = controller.Login(request);

            loggerService.Verify(x => x.LogError("SecurityController > Login. ERROR 500", It.IsAny<Exception>()), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((StatusCodeResult)result).StatusCode);
        }

        [Test]
        public void Login_ReturnsUnauthorized()
        {
            var request = new LoginRequest()
            {
                Password = null,
                User = "user"
            };

            userService.Setup(x => x.ValidateLogin(It.IsAny<LoginRequest>())).Returns((User)null);

            var result = controller.Login(request);

            loggerService.Verify(x => x.LogError("SecurityController > Login. ERROR 401"), Times.Once);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status401Unauthorized, ((StatusCodeResult)result).StatusCode);
        }
    }
}
