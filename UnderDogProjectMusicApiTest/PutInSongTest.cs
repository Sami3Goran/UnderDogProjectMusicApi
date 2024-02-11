using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApiTest
{
    [TestClass]
    public class PutInSongTest
    {
        [TestMethod]
        public void PutInSong_ValidSong_SongAddedSuccessfully()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("PutInSong_ValidSong_SongAddedSuccessfully")
                .Options;

            using (var context = new ApplicationContext(options))
            {
                // put in testdata
                context.Artists.Add(new Artist { ArtistId = 1, ArtistTitle = "TestArtist" });
                context.Genres.Add(new Genre { GenreId = 1, GenreTitle = "TestGenre" });
                context.SaveChanges();

                var music = new Music(context);

                var songDto = new PutInSongsDto
                {
                    Name = "TestSong",
                    ArtistId = 1, // Assuming artist with ID 1 exists in the database
                    GenreId = 1   // Assuming genre with ID 1 exists in the database
                };

                // Act
                music.PutInSong(songDto);

                // Assert
                var songs = context.Songs.ToList();
                Assert.AreEqual(1, songs.Count);
                Assert.AreEqual(songDto.Name, songs[0].SongName);
            }
        }
    }
}