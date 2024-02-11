using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicClient.UnderdogApiModels;

namespace UnderDogProjectMusicClient.Methods
{
    internal class ArtistMethods
    {

        public static async Task ListUsersArtists(HttpClient client, int userId)
        {
            //Makes a GET call to API's endpoint, which is through user's ID
            var response = await client.GetAsync($"/artist/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                await Console.Out.WriteLineAsync($"Error listing user's artists (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                return;
            }

            //The string answer we get from the call
            string responseData = await response.Content.ReadAsStringAsync();

            //Deserializing the Json string  into a list of ListUsersArtists objects
            List<ListArtist> artists = JsonSerializer.Deserialize<List<ListArtist>>(responseData);

            foreach (var artist in artists)
            {
                Console.WriteLine($"Artist name: {artist.ArtistTitle}");
            }

            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }
        public static async Task ConnectUserToArtist(HttpClient client, int userId)
        {
            //This chosen artist ID is to be connected with chosen user ID in main menu
            int artistId;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                await Console.Out.WriteAsync("Enter artist ID to connect with: ");
                Console.ForegroundColor = ConsoleColor.White;
                if (int.TryParse(Console.ReadLine(), out artistId))
                {

                    break;
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid artist ID. Please enter a valid number.\nPress enter to return to menu.");
                    return;
                }
            }

            var response = await client.PostAsync($"/user/{userId}/artist/{artistId}", null);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkRed;

                await Console.Out.WriteLineAsync($"Error connecting user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Succesfully connected user with artist (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            Console.ReadLine();
            Console.Clear();
            await MenuMethods.UserMenu(client, userId);
        }
        public static async Task AddNewArtistAysnc(HttpClient client, int userId, User user)
        {
            Console.Clear();
            Console.CursorVisible = true;
            await Console.Out.WriteLineAsync("Adding new artist:");
            await Console.Out.WriteLineAsync("");
            await Console.Out.WriteLineAsync("Enter artist name: ");
            string artistName = Console.ReadLine();
            await Console.Out.WriteLineAsync("Enter description: ");
            string description = Console.ReadLine();

            ListArtist artist = new ListArtist()
            {
                ArtistTitle = artistName
            };

            string json = JsonSerializer.Serialize(artist);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/artist", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"Failed to add new artist(status code: {response.StatusCode} )");
            }
            await ConnectUserToArtist(client, userId);
            await Console.Out.WriteLineAsync("Press enter to go back to main menu");
            Console.ReadLine();
            await MenuMethods.UserMenu(client, userId);
        }
    }
}
