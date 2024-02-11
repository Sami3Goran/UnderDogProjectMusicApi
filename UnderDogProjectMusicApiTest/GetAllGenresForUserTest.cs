using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApiTest
{
    [TestClass]
    public class GenreHandlerTest
    {
        [TestMethod]
        public void GetAllGenresForUser_ValidUserId_ReturnsGenres()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("GetAllGenresForUser_ValidUserId_ReturnsGenres")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var music = new Music(context);

                // Lägg till en användare och associera genrer med den
                var user = new User { UserName = "TestUser" };
                var genres = new List<Genre>
                {
                    new Genre { GenreTitle = "Rock" },
                    new Genre { GenreTitle = "Pop" }
                    // Lägg till fler genrer om det behövs
                };

                context.Users.Add(user);
                context.Genres.AddRange(genres);
                context.SaveChanges();

                music.AttachGenreForUser(1, user.UserId); // Anta att "Rock" har GenreId 1
                music.AttachGenreForUser(2, user.UserId); // Anta att "Pop" har GenreId 2

                // Act
                var resultGenres = music.GetAllGenresForUser(user.UserId);

                // Assert
                Assert.IsNotNull(resultGenres);
                Assert.AreEqual(2, resultGenres.Count); // Antalet genrer du förväntar dig baserat på det du har lagt till
            }
        }
    }
}