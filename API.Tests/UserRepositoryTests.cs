using API.Domain.Entities;
using API.Infrastructure;
using API.Infrastructure.Repositories;
using API.Tests.MockHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Runtime.Intrinsics.X86;

namespace API.Tests
{
    public class UserRepositoryTests
    {
        /// <summary>
        /// Checks .GetAllUsersAsync() method.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            // Arrange

            // Set up valid users
            var user1 = MockUserManagerAndSignInWithDTOConversion.MapFromRegisterDto(MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto());
            var user2 = MockUserManagerAndSignInWithDTOConversion.MapFromRegisterDto(MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto_2());

            var context = AppDbContextHelper.CreateInMemoryContext(user1, user2);

            var mockLogger = new Mock<ILogger<UserRepository>>();
            var mockRepository = new UserRepository(mockLogger.Object, context);

            // Act
            var result = await mockRepository.GetAllUsersAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());            
        }

        /// <summary>
        /// Checks .GetUsersByIdAsync() method.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto_WithId();

            var context = AppDbContextHelper.CreateInMemoryContext(user);

            var mockLogger = new Mock<ILogger<UserRepository>>();
            var mockRepository = new UserRepository(mockLogger.Object, context);

            // Act
            var result = await mockRepository.GetUserByIdAsync(user.Id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        /// <summary>
        /// Checks .DeleteUserAsync() method.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnTrue()
        {
            // Arrange
            var user = MockUserManagerAndSignInWithDTOConversion.GetValidRegisterUserDto_WithId();

            var context = AppDbContextHelper.CreateInMemoryContext(user);

            var mockLogger = new Mock<ILogger<UserRepository>>();
            var mockRepository = new UserRepository(mockLogger.Object, context);

            // Act
            var result = await mockRepository.DeleteUserAsync(user.Id);

            //Assert
            Assert.True(result);
        }
    }
}
