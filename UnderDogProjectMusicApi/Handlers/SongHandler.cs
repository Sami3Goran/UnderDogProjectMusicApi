using System.Net;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApi.Handlers
{
    public class SongHandler
    {
        //handler för Song
        public static IResult AttachSongToUser(IMusic music, int userId, int songId)
        {
            try
            {
                music.AttachSongToUser(userId, songId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ConnectSongToUser():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static IResult ListUserSongs(IMusic music, int userId)
        {
            try
            {
                var userSongs = music.ListUserSongs(userId);
                return Results.Json(userSongs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ListUserSongs():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        public static IResult PutInSong(IMusic music, PutInSongsDto song)
        {
            try
            {
                music.PutInSong(song);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in AddSong():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
