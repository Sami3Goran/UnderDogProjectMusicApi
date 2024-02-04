using System.Net;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApi.Handlers
{
    
    public class GenreHandler
    {
        //handler för genre
        public static IResult ListUsersGenres(IMusic music, int userId)
        {
            try
            {
                var genres = music.GetAllGenresForUser(userId);
                return Results.Json(genres);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ListUsersGenres():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }


        }
   
        public static IResult AttachUserToGenre(IMusic music, int userId, int genreId)
        {
            try
            {
                music.AttachGenreForUser(genreId, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ConnectUserToGenre():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult AddNewGenre(IMusic music, GenreDto genre)
        {
            try
            {
                music.AddNewGenre(genre);
            }
            catch
            {
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }

    }
}
