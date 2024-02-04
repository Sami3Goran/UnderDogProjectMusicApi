using Microsoft.EntityFrameworkCore;
using System.Net;
using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Handlers;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Models.ViewModel;
using UnderDogProjectMusicApi.Repositories;
using UnderDogProjectMusicApi.Service;

namespace UnderDogProjectMusicApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.AddScoped<IMusicService, MusicService>();
            builder.Services.AddScoped<IMusic, Music>();
            var app = builder.Build();

            app.MapGet("/", () => "ANEETA MAXIMA VICTORIA");

            //GET METODER

            //hämta alla användare
            app.MapGet("/user", UserHandler.ListAllUsers);
            //hämta alla artistier till en specifik användare
            app.MapGet("/artist/{userId}", ArtistHandler.ListUsersArtists);
            //hämta alla genre till en specifik användare
            app.MapGet("/genre/{userId}", GenreHandler.ListUsersGenres);
            //hämta alla låtar till en specifik användare
            app.MapGet("/song/{userId}", SongHandler.ListUserSongs);

            //POST METODER

            //lägg till ny användare
            app.MapPost("/newUser", UserHandler.AddUser);
            // koppla ihop artist till användare
            app.MapPost("/user/{userId}/artist/{artistId}", ArtistHandler.AttachUserToArtist);
            //koppla ihop användare till genre
            app.MapPost("/user/{userId}/genre/{genreId}", GenreHandler.AttachUserToGenre);
            //koppla ihop låt till användare
            app.MapPost("/user/{userId}/song/{songId}", SongHandler.AttachSongToUser);
            //lägg till ny låt i databas
            app.MapPost("/song", SongHandler.PutInSong);
            //lägg till ny artist i databas
            app.MapPost("/artist", ArtistHandler.AddNewArtist);
            //lägg till ny genre i databas
            app.MapPost("/genre", GenreHandler.AddNewGenre);

            //GET METOD GENOM EXTERN API
            //Return Discography for an Artist with latest Album names and year only
            app.MapGet("/audiodb/{artist}", async (string artist, IMusic music) =>
            {
                try
                {
                    // Skapa en instans av MusicService
                    var musicService = new MusicService();

                    // Använd instansen för att anropa metoden
                    MusicDto All = await musicService.GetLatestAlbumAsync(artist);

                    List<MusicDto.Album> albums = All?.Albums;

                    MusicViewModel result = new MusicViewModel
                    {
                        ViewAlbums = albums?.Select(a => new MusicViewModel.ViewAlbum
                        {
                            Name = a.NameAlbum,
                            Year = a.ReleseYear
                        }).ToList()
                    };

                    return Results.Json(result);
                }
                catch (Exception ex)
                {
                    // Hantera undantag på lämpligt sätt
                    Console.WriteLine($"There was an error in GetLatestAlbumAsync():{ex}");
                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }
            });

            app.Run();
        }
    }
}
