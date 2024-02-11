using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApiTest
{
    [TestClass]
    public class AddNewArtistTest
    {
        [TestMethod]
        public void AddNewArtist_ValidArtist_ArtistAddedSuccessfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("AddNewArtist_ValidArtist_ArtistAddedSuccessfully")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var music = new Music(context);

                var artistDto = new ArtistDto
                {
                    ArtistTitle = "TestArtist",
                    Description = "TestDescription"
                };

                // Act
                music.AddNewArtist(artistDto);

                // Assert
                var artists = context.Artists.ToList();
                Assert.AreEqual(1, artists.Count);
                Assert.AreEqual(artistDto.ArtistTitle, artists[0].ArtistTitle);
                Assert.AreEqual(artistDto.Description, artists[0].Description);
            }
        }

        [TestMethod]
        public void AddNewArtist_ArtistAlreadyExists_HandleError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("AddNewArtist_ArtistAlreadyExists_HandleError")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var music = new Music(context);

                // Assuming an artist with the same title already exists in the database
                var existingArtistDto = new ArtistDto
                {
                    ArtistTitle = "ExistingArtist",
                    Description = "ExistingDescription"
                };

                context.Artists.Add(new Artist
                {
                    ArtistTitle = existingArtistDto.ArtistTitle,
                    Description = existingArtistDto.Description
                });
                context.SaveChanges();

                // Act & Assert
                try
                {
                    music.AddNewArtist(existingArtistDto);
                    Assert.Fail("Exception should have been thrown.");
                }
                catch (Exception ex)
                {
                    // Assert that the correct exception is thrown
                    Assert.IsTrue(ex is InvalidOperationException);
                    Assert.AreEqual($"{existingArtistDto.ArtistTitle} already exists.", ex.Message);
                }
            }
        }

        [TestMethod]
        public void AddNewArtist_EmptyArtistTitle_HandleError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("AddNewArtist_EmptyArtistTitle_HandleError")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                var music = new Music(context);

                var artistDto = new ArtistDto
                {
                    ArtistTitle = "", // Empty artist title
                    Description = "TestDescription"
                };

                // Act & Assert
                try
                {
                    music.AddNewArtist(artistDto);
                    Assert.Fail("Exception should have been thrown.");
                }
                catch (Exception ex)
                {
                    // Assert that the correct exception is thrown
                    Assert.IsTrue(ex is InvalidOperationException);
                    Assert.AreEqual("You must write an artist name.", ex.Message);
                }
            }
        }
    }
}