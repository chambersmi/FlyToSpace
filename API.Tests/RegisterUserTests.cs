using API.Application.Interfaces.IServices;
using API.Controllers;
using API.Infrastructure.Auth;
using API.Infrastructure.Services;
using API.Tests.MockHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests
{
    public class RegisterUserTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _authController;

        public RegisterUserTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _authController = new AuthController(_mockLogger.Object, _mockAuthService.Object, _mockJwtTokenGenerator.Object);
        }

        /// <summary>
        /// This test will check if registering a user with valid data works and returns a success response.
        /// </summary>
        /// <returns></returns>
        /// 
        [Fact]
        public async Task RegisterUser_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto();
            var identityResult = IdentityResult.Success;

            // Set up mock AuthService to return successful registration result
            _mockAuthService.Setup(x => x.RegisterAsync(registerDto)).ReturnsAsync(identityResult);

            // Act
            var actionResult = await _authController.RegisterUser(registerDto);

            //Assert
            var result = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal("User successfully created.", result.Value);
        }

        /// <summary>
        /// This test will try registering a user using mismatched password/confirm password
        /// Should return 400.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterUser_ShouldReturnFailed_WhenPasswordsDoNotMatch()
        {
            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDto_PasswordsDoNotMatch();
            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordMismatch",
                Description = "Passwords must match."
            });

            // Set up mock AuthService to return successful registration result
            _mockAuthService.Setup(x => x.RegisterAsync(registerDto)).ReturnsAsync(identityResult);

            // Act
            var actionResult = await _authController.RegisterUser(registerDto);

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal(400, result.StatusCode);

            var errResponse = Assert.IsType<List<IdentityError>>(result.Value);
            Assert.Contains(errResponse, e => e.Code == "PasswordMismatch");
        }

        /// <summary>
        /// This test will try registering a user that already exists.
        /// Should return 400
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterUser_ShouldReturnFailed_WhenUserAlreadyExists()
        {
            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDto_PasswordsDoNotMatch();
            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "null" // not used
            });

            // Set up mock AuthService to return successful registration result
            _mockAuthService.Setup(x => x.RegisterAsync(registerDto)).ReturnsAsync(identityResult);

            // Act
            var actionResult = await _authController.RegisterUser(registerDto);

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal(400, result.StatusCode);

            var errResponse = Assert.IsType<List<IdentityError>>(result.Value);
            Assert.Contains(errResponse, e => e.Code == "DuplicateEmail");
        }
    }
}
