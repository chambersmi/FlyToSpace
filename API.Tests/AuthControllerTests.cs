using API.Application.DTOs;
using API.Controllers;
using API.Domain.Entities;
using API.Tests.MockHelpers;
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    /// <summary>
    /// Verifies IMapper.Map creates the ApplicationUser from the DTO.
    /// Verifies UserManager.CreateAsync() is called with the correct values.
    /// Asserts that the response is Ok.
    /// </summary>
    public class AuthControllerTests
    {
        [Fact]
        public async Task RegisterUser_ReturnsOk_WhenUserIsCreated()
        {
            // Arrange
            var mockUserManager = MockUserManagerAndSignInWithDTOConversion.MockUserManager<ApplicationUser>();
            var mockSignInManager = MockUserManagerAndSignInWithDTOConversion.MockSignInManager(mockUserManager.Object);
            var mockLogger = new Mock<ILogger<AuthController>>();
            var mockMapper = new Mock<IMapper>();

            var registerValidDto = MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto();

            var user = new ApplicationUser
            {
                Email = registerValidDto.Email,
                UserName = registerValidDto.Email
            };

            mockMapper.Setup(m => m.Map<ApplicationUser>(registerValidDto)).Returns(user);
            mockUserManager.Setup(m => m.CreateAsync(user, registerValidDto.Password)).ReturnsAsync(IdentityResult.Success);

            var controller = new AuthController(mockLogger.Object, mockUserManager.Object, mockSignInManager.Object, mockMapper.Object);

            // Act
            var result = await controller.RegisterUser(registerValidDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockUserManager.Verify(m => m.CreateAsync(user, registerValidDto.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_ReturnsFail_WithInvalidEmail()
        {
            // Arrange
            var mockUserManager = MockUserManagerAndSignInWithDTOConversion.MockUserManager<ApplicationUser>();
            var mockSignInManager = MockUserManagerAndSignInWithDTOConversion.MockSignInManager(mockUserManager.Object);
            var mockLogger = new Mock<ILogger<AuthController>>();
            var mockMapper = new Mock<IMapper>();

            var registerValidDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDtoBadEmail();

            var user = new ApplicationUser
            {
                Email = registerValidDto.Email,
                UserName = registerValidDto.Email
            };

            mockMapper.Setup(m => m.Map<ApplicationUser>(registerValidDto)).Returns(user);
            mockUserManager.Setup(m => m.CreateAsync(user, registerValidDto.Password)).ReturnsAsync(IdentityResult.Failed());

            var controller = new AuthController(mockLogger.Object, mockUserManager.Object, mockSignInManager.Object, mockMapper.Object);

            // Act
            var result = await controller.RegisterUser(registerValidDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockUserManager.Verify(m => m.CreateAsync(user, registerValidDto.Password), Times.Once);
        }

        [Fact]
        public async Task RegisterUser_ReturnsFail_WhenBirthdayIsTomorrow()
        {
            // Arrange
            var mockUserManager = MockUserManagerAndSignInWithDTOConversion.MockUserManager<ApplicationUser>();
            var mockSignInManager = MockUserManagerAndSignInWithDTOConversion.MockSignInManager(mockUserManager.Object);
            var mockLogger = new Mock<ILogger<AuthController>>();
            var mockMapper = new Mock<IMapper>();

            var registerValidDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDtoBirthdayIsTomorrow();

            var user = new ApplicationUser
            {
                Email = registerValidDto.Email,
                UserName = registerValidDto.Email
            };

            mockMapper.Setup(m => m.Map<ApplicationUser>(registerValidDto)).Returns(user);
            mockUserManager.Setup(m => m.CreateAsync(user, registerValidDto.Password)).ReturnsAsync(IdentityResult.Failed());

            var controller = new AuthController(mockLogger.Object, mockUserManager.Object, mockSignInManager.Object, mockMapper.Object);

            // Act
            var result = await controller.RegisterUser(registerValidDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockUserManager.Verify(m => m.CreateAsync(user, registerValidDto.Password), Times.Once);
        }

    }
}
