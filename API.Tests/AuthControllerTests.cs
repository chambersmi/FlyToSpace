using API.Application.DTOs;
using API.Controllers;
using API.Domain.Entities;
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
            var mockUserManager = MockUserManager<ApplicationUser>();
            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            var mockLogger = new Mock<ILogger<AuthController>>();

            var mockMapper = new Mock<IMapper>();

            var registerDto = new RegisterUserDto
            {
                Email = "testuser@example.com",
                Password = "Test123!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };

            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            mockMapper.Setup(m => m.Map<ApplicationUser>(registerDto)).Returns(user);
            mockUserManager.Setup(mum => mum.CreateAsync(user, registerDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new AuthController(mockLogger.Object, mockUserManager.Object, mockSignInManager.Object, mockMapper.Object);

            // Act
            var result = await controller.RegisterUser(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            mockUserManager.Verify(mum => mum.CreateAsync(user, registerDto.Password), Times.Once);
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
