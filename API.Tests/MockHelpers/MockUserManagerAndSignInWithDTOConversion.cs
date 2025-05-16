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

        public static RegisterUserDto GetInvalidRegisterUserDtoBadEmail()
        {
            return new RegisterUserDto
            {
                Email = "testemail.com",
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