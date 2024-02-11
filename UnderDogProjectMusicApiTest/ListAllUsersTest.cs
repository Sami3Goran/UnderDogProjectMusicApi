using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApiTest
{
    [TestClass]
    public class ListAllUserTest
    {
        [TestMethod]
        public void ListAllUsers_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("ListAllUsers_NoUsers_ReturnsEmptyList")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var music = new Music(context);

                // Act
                var users = music.ListAllUsers();

                // Assert
                Assert.IsNotNull(users);
                Assert.AreEqual(0, users.Count);
            }
        }
    }
}