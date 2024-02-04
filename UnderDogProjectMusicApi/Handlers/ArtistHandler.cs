using System.Net;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Models.ViewModel;
using UnderDogProjectMusicApi.Repositories;
using UnderDogProjectMusicApi.Service;

namespace UnderDogProjectMusicApi.Handlers
{
    public class ArtistHandler
    {
        // förenklar testing
        public static IResult ListUsersArtists(IMusic music, int userId)
        {
            try
            {
                var artists = music.ListUsersArtists(userId);
                return Results.Json(artists);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ListUsersArtists():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }


        }

        public static IResult AttachUserToArtist(IMusic music, int userId, int artistId)
        {
            try
            {
                music.AttachUserToArtist(userId, artistId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ConnectUserToArtist():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult AddNewArtist(IMusic music, ArtistDto artistDto)
        {
            try
            {
                music.AddNewArtist(artistDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in AddNewArtist():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
