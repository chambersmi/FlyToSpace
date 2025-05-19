using API.Application.DTOs;
using API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests.MockHelpers
{
    public class MockUserManagerAndSignInWithDTOConversion
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(UserManager<TUser> userManager) where TUser : class
        {
            return new Mock<SignInManager<TUser>>(
                userManager,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<TUser>>(),
                null, null, null, null
            );
        }

        public static RegisterUserDto GetValidRegisterUserDto()
        {
            return new RegisterUserDto
            {                
                Email = "testuser@example.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };
        }

        public static RegisterUserDto GetValidRegisterUserDto_2()
        {
            return new RegisterUserDto
            {
                Email = "testuser@example.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };
        }

        public static ApplicationUser GetValidRegisterUserDto_WithId()
        {
            var registerDto = GetValidRegisterUserDto();

            // Create application user with an Id to simulate that it's saved in database
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                StreetAddress1 = registerDto.StreetAddress1,
                City = registerDto.City,
                State = registerDto.State,
                ZipCode = registerDto.ZipCode
            };

            return user;
        }

        public static RegisterUserDto GetInvalidRegisterUserDto_BadEmail()
        {
            return new RegisterUserDto
            {
                Email = "testemail.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };
        }

        public static RegisterUserDto GetInvalidRegisterUserDto_PasswordsDoNotMatch()
        {
            return new RegisterUserDto
            {
                Email = "testemail.com",
                Password = "Test123!",
                ConfirmPassword = "Iamabadpassword!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };
        }

        public static RegisterUserDto GetInvalidRegisterUserDto_BirthdayIsTomorrow()
        {
            return new RegisterUserDto
            {
                Email = "testemail.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!",
                FirstName = "Test",
                MiddleName = "",
                LastName = "McTesterton",
                DateOfBirth = DateTime.UtcNow.AddDays(1),
                StreetAddress1 = "123 Main St.",
                City = "McTesterTown",
                State = Domain.Enums.StateEnum.MI,
                ZipCode = "55527"
            };
        }

        public static ApplicationUser MapFromRegisterDto(RegisterUserDto dto)
        {
            return new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                StreetAddress1 = dto.StreetAddress1,
                StreetAddress2 = dto.StreetAddress2,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode
            };
        }

    }
}