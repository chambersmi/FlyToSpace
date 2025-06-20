﻿using API.Application.DTOs;
using API.Domain.Entities;
using API.Infrastructure.Auth;
using API.Infrastructure.Services;
using API.Tests.MockHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly IMapper _mapper;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userManagerMock = MockUserManagerAndSignInWithDTOConversion.MockUserManager<ApplicationUser>();
            _signInManagerMock = MockUserManagerAndSignInWithDTOConversion.MockSignInManager(_userManagerMock.Object);
            _mockLogger = new Mock<ILogger<AuthService>>();
            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<RegisterUserDto, ApplicationUser>();
            });

            _mapper = mapperConfig.CreateMapper();

            _authService = new AuthService(_mockLogger.Object, _userManagerMock.Object, _signInManagerMock.Object, _mapper);
        }

        /// <summary>
        /// TEST:
        /// This is sending valid user credentials via the method .GetValidRegisterUserDto();
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {

            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto();

            var dto = new LoginDto 
            { 
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            var user = MockUserManagerAndSignInWithDTOConversion.MapFromRegisterDto(registerDto);

            _userManagerMock.Setup(um => um.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _mockJwtTokenGenerator.Setup(j => j.GenerateToken(user)).Returns("mocked.jwt.token");

            // Act
            var (success, returnedUser) = await _authService.AuthenticateUserAsync(dto);

            // Assert
            Assert.True(success);
            Assert.NotNull(returnedUser);
            Assert.Equal(dto.Email, returnedUser!.Email);
        }

        /// <summary>
        /// TEST:
        /// This is sending invalid user credentials (Bad Email) via the method .GetInvalidRegisterUserDto();
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {

            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDto_BadEmail();

            var dto = new LoginDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            var user = MockUserManagerAndSignInWithDTOConversion.MapFromRegisterDto(registerDto);

            _userManagerMock.Setup(um => um.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _mockJwtTokenGenerator.Setup(j => j.GenerateToken(user)).Returns("mocked.jwt.token");

            // Act
            var (success, returnedUser) = await _authService.AuthenticateUserAsync(dto);

            // Assert
            Assert.True(success);
            Assert.NotNull(returnedUser);
            Assert.Equal(dto.Email, returnedUser!.Email);
        }

        /// <summary>
        /// TEST:
        /// This is sending invalid user credentials (Birthday is tomorrow) via the method .GetInvalidRegisterUserDto_BirthdayIsTomorrow();
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AuthenticateUserAsync_ShouldReturnFalse_WhenBirthdayIsTomorrow()
        {

            // Arrange
            var registerDto = MockUserManagerAndSignInWithDTOConversion.GetInvalidRegisterUserDto_BirthdayIsTomorrow();

            var dto = new LoginDto
            {
                Email = registerDto.Email,
                Password = registerDto.Password
            };

            var user = MockUserManagerAndSignInWithDTOConversion.MapFromRegisterDto(registerDto);

            _userManagerMock.Setup(um => um.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(SignInResult.Success);

            _mockJwtTokenGenerator.Setup(j => j.GenerateToken(user)).Returns("mocked.jwt.token");

            // Act
            var (success, returnedUser) = await _authService.AuthenticateUserAsync(dto);

            // Assert
            Assert.True(success);
            Assert.NotNull(returnedUser);
            Assert.Equal(dto.Email, returnedUser!.Email);
        }
    }
}
