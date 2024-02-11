using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Models.ViewModel;
using UnderDogProjectMusicApi.Repositories;
using System.Collections.Generic;

namespace UnderDogProjectMusicApiTest
{
    [TestClass]
    public class AddUserTest
    {
        [TestMethod]
        public void AddUser_ValidUser_UserAddedSuccessfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("AddUser_ValidUser_UserAddedSuccessfully")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var userRepository = new Music(context);

                var userDto = new UserDto
                {
                    UserName = "TestUser"
                };

                // Act
                userRepository.AddUser(userDto);

                // Assert
                var users = context.Users.ToList();
                Assert.AreEqual(1, users.Count);
                Assert.AreEqual(userDto.UserName, users[0].UserName);
            }
        }
    }
}