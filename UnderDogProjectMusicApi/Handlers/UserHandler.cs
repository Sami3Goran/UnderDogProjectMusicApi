using System.Net;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Repositories;

namespace UnderDogProjectMusicApi.Handlers
{
    public class UserHandler
    {
        //handler för user
        public static IResult AddUser(IMusic music, UserDto user)
        {
            try
            {
                music.AddUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in AddUser():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ListAllUsers(IMusic music)
        {
            try
            {
                var users = music.ListAllUsers();
                return Results.Json(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error in ListAllUsers():{ex}");
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
