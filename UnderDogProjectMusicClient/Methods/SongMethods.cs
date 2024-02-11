using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicClient.UnderdogApiModels;

namespace UnderDogProjectMusicClient.Methods
{
    internal class SongMethods
    {

        public static async Task AddSong(HttpClient client)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            await Console.Out.WriteAsync("Enter song title: ");
            Console.ForegroundColor = ConsoleColor.White;
            string title = Console.ReadLine();
            Console.ResetColor();

            int artistId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                await Console.Out.WriteAsync("Enter artist ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out artistId))
                {

                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid artist ID. Please enter a valid number.\nPress enter to return to menu.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }
            }
            int genreId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                await Console.Out.WriteAsync("Enter genre ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out genreId))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid genre ID. Please enter a valid number.\n Press enter to return to menu.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return;
                }
            }

            PutInSong newSong = new PutInSong()
            {
                Name = title,
                ArtistId = artistId,
                GenreId = genreId
            };

            string json = JsonSerializer.Serialize(newSong);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/song", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error adding song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu");
                //Because of the async method, we have to add this In.ReadLine
                //so that the text will be printed out!!
                Console.In.ReadLine();
                return;
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Added song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
        }
        public static async Task ListUserSongs(HttpClient client, int userId)
        {
            var response = await client.GetAsync($"/song/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error listing user's songs (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            string responseData = await response.Content.ReadAsStringAsync();

            List<SongArtist> songs = JsonSerializer.Deserialize<List<SongArtist>>(responseData);

            foreach (var track in songs)
            {
                Console.WriteLine($"Title: {track.Title}, by: {track.ArtistName}");
            }

            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }
        public static async Task ConnectSongToUser(HttpClient client, int userId)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            await Console.Out.WriteAsync("Enter song ID to connect with: ");
            Console.ForegroundColor = ConsoleColor.White;

            int songId;
            while (true)
            {

                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out songId))
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid song ID. Please enter a valid number.");
                    Console.WriteLine("Press enter to return to menu");
                    return;

                }

            }

            var response = await client.PostAsync($"/user/{userId}/song/{songId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error connecting user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Succesfully connected user with song (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);

        }
    }
}
