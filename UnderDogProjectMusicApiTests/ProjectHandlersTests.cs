using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using UnderDogProjectMusicApi.Handlers;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApi;

[TestClass]
public class GenreHandlerTests
{
    [TestMethod]
    public void ListUsersGenres_ReturnsGenres()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var userId = 1;
        var expectedGenres = new[] { "Rock", "Pop" };
        musicMock.Setup(x => x.GetAllGenresForUser(userId)).Returns(expectedGenres);

        // Act
        var result = GenreHandler.ListUsersGenres(musicMock.Object, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedGenres, result.Value);
        Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
    }

    [TestMethod]
    public void ListUsersGenres_HandlesException()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var userId = 1;
        musicMock.Setup(x => x.GetAllGenresForUser(userId)).Throws(new Exception("Test Exception"));

        // Act
        var result = GenreHandler.ListUsersGenres(musicMock.Object, userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Value);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
    }

    [TestMethod]
    public void AttachUserToGenre_ReturnsCreatedStatus()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var userId = 1;
        var genreId = 101;

        // Act
        var result = GenreHandler.AttachUserToGenre(musicMock.Object, userId, genreId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
    }

    [TestMethod]
    public void AttachUserToGenre_HandlesException()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var userId = 1;
        var genreId = 101;
        musicMock.Setup(x => x.AttachGenreForUser(genreId, userId)).Throws(new Exception("Test Exception"));

        // Act
        var result = GenreHandler.AttachUserToGenre(musicMock.Object, userId, genreId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
    }

    [TestMethod]
    public void AddNewGenre_ReturnsCreatedStatus()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var genreDto = new GenreDto(); // Du behöver skapa en giltig GenreDto för testning

        // Act
        var result = GenreHandler.AddNewGenre(musicMock.Object, genreDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
    }

    [TestMethod]
    public void AddNewGenre_HandlesException()
    {
        // Arrange
        var musicMock = new Mock<IMusic>();
        var genreDto = new GenreDto(); // Du behöver skapa en giltig GenreDto för testning
        musicMock.Setup(x => x.AddNewGenre(genreDto)).Throws(new Exception("Test Exception"));

        // Act
        var result = GenreHandler.AddNewGenre(musicMock.Object, genreDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
    }
}