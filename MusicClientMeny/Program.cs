using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicApi.Models;
using UnderDogProjectMusicApi.Models.Dto;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Välkommen till Musik API-klienten!");

        int userId;

        // Låt användaren välja eller skapa en ny användare
        Console.WriteLine("1. Välj befintlig användare");
        Console.WriteLine("2. Skapa ny användare");
        Console.Write("Ange ditt val: ");
        int choice = int.Parse(Console.ReadLine());

        Console.Clear();

        if (choice == 1)
        {
            Console.Write("Ange användar-ID: ");
            userId = int.Parse(Console.ReadLine());
        }
        else
        {
            // Låt användaren skapa en ny användare
            Console.Write("Ange användarnamn för den nya användaren: ");
            string newUser = Console.ReadLine();

            // Anropa ditt API för att skapa en ny användare och få tillbaka användar-ID
            userId = await CreateUser(newUser);

            Console.WriteLine($"Ny användare skapad med användar-ID: {userId}");
        }


        // Huvudmeny
        while (true)
        {
            Console.WriteLine("\nHuvudmeny:");
            Console.WriteLine("1. Lista användarens genrer");
            Console.WriteLine("2. Lägg till genre");
            Console.WriteLine("3. Lista användarens artister");
            Console.WriteLine("4. Lägg till artist");
            Console.WriteLine("5. Lista användarens låtar");
            Console.WriteLine("6. Lägg till låt");
            Console.WriteLine("0. Avsluta");

            Console.Write("Ange ditt val: ");

            // Använd TryParse för att försöka konvertera användarens inmatning till en siffra
            if (int.TryParse(Console.ReadLine(), out int menuChoice))
            {
                switch (menuChoice)
                {
                    case 1:
                        await ListUserGenres(userId);
                        break;
                    case 2:
                        await AddGenreForUser(userId);
                        break;
                    case 3:
                        await ListUserArtists(userId);
                        break;
                    case 4:
                        await AddArtistForUser(userId);
                        break;
                    case 5:
                        await ListUserSongs(userId);
                        break;
                    case 6:
                        await AddSongForUser(userId);
                        break;
                    case 0:
                        Console.WriteLine("Programmet avslutas. Tack för besöket!");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

            }
            else
            {
                Console.WriteLine("Ogiltig inmatning. Vänligen ange en siffra.");
            }
        }

        static async Task<int> CreateUser(string newUserName)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string applicationUrl = "https://localhost:7211/users/create";

                    var newUser = new { UserName = newUserName };

                    HttpResponseMessage response = await client.PostAsJsonAsync(applicationUrl, newUser);

                    if (response.IsSuccessStatusCode)
                    {
                        int userId = await response.Content.ReadFromJsonAsync<int>();
                        Console.WriteLine($"User created successfully with ID: {userId}");
                        return userId;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Error: The requested resource was not found. Check the API route and address.");
                    }
                    else
                    {
                        Console.WriteLine($"Error creating user. Status code: {response.StatusCode}");
                    }

                    return -1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        static async Task ListUserGenres(int userId)
        {
            try
            {
                GenreDto[] genres = await API.GetGenresForUser(userId);

                if (genres != null)
                {
                    Console.Clear();
                    Console.WriteLine("List of Genres:");
                    foreach (var genre in genres)
                    {
                        Console.WriteLine($"{genre.GenreTitle}");
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching genres.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        static async Task AddGenreForUser(int userId)
        {
            try
            {
                // Låt användaren ange genreinformation
                Console.Write("Ange genretitel: ");
                string genreTitle = Console.ReadLine();

                // Skapa ett GenreDto-objekt
                var newGenre = new GenreDto
                {
                    GenreTitle = genreTitle
                };

                // Anropa ditt API för att lägga till genre för användaren
                bool success = await API.AddGenreForUser(userId, newGenre);
                Console.Clear();

                if (success)
                {
                    Console.WriteLine("Genre added successfully.");
                }
                else
                {
                    Console.WriteLine("Error adding genre.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        static async Task ListUserArtists(int userId)
        {
            try
            {
                ArtistDto[] artists = await API.GetArtistsForUser(userId);

                if (artists != null)
                {
                    Console.Clear();
                    Console.WriteLine("List of Artists:");
                    foreach (var artist in artists)
                    {
                        Console.WriteLine($"{artist.ArtistTitle}");
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching artists.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        static async Task AddArtistForUser(int userId)
        {
            try
            {
                // Låt användaren ange artistinformation
                Console.Write("Ange artisttitel: ");
                string artistTitle = Console.ReadLine();

                // Låt användaren ange artistbeskrivning
                Console.Write("Ange artistbeskrivning: ");
                string artistDescription = Console.ReadLine();

                // Skapa ett ArtistDto-objekt
                var newArtist = new ArtistDto
                {
                    ArtistTitle = artistTitle,
                    Description = artistDescription
                };

                // Anropa ditt API för att lägga till artist för användaren
                bool success = await API.AddArtistForUser(userId, newArtist);
                Console.Clear();

                if (success)
                {
                    Console.WriteLine("Artist added successfully.");
                }
                else
                {
                    Console.WriteLine("Error adding artist.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        static async Task ListUserSongs(int userId)
        {
            try
            {
                SongDto[] songs = await API.GetSongsForUser(userId);

                if (songs != null)
                {
                    Console.Clear();
                    Console.WriteLine("List of Songs:");
                    foreach (var song in songs)
                    {
                        Console.WriteLine($"{song.SongId} by {ArtistDto.Equals}");
                    }
                }
                else
                {
                    Console.WriteLine("Error fetching songs.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }

        static async Task AddSongForUser(int userId)
        {
            try
            {
                Console.Clear();

                ArtistDto[] artists = await API.GetArtistsForUser(userId);

                if (artists == null || artists.Length == 0)
                {
                    Console.WriteLine("Error: No artists found for the user. Please add an artist first.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Existing Artists:");
                for (int i = 0; i < artists.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {artists[i].ArtistTitle}");
                }

                Console.Write("Select an artist (enter the artist number): ");
                int artistChoice = int.Parse(Console.ReadLine());

                if (artistChoice < 1 || artistChoice > artists.Length)
                {
                    Console.WriteLine("Error: Invalid artist choice.");
                    Console.ReadKey();
                    return;
                }

                ArtistDto selectedArtist = artists[artistChoice - 1];

                Console.Write("Enter song name: ");
                string songName = Console.ReadLine();

                var newSong = new SongDto
                {
                    SongName = songName,
                    ArtistId = selectedArtist.ArtistId
                };

                bool success = await API.AddSongForUser(userId, newSong);

                if (success)
                {
                    Console.WriteLine("Song added successfully.");
                }
                else
                {
                    Console.WriteLine("Error adding song.");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
