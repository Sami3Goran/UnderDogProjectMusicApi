using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicClient.UnderdogApiModels;

namespace UnderDogProjectMusicClient.Methods
{
    public class MenuMethods
    {

        // Asynchronously displays the user menu based on the selected user
        public static async Task UserMenu(HttpClient client, int userId)
        {
            Console.Clear();
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Current user ID: {userId}");
            Console.ForegroundColor = originalColor;
            Console.WriteLine($"Now choose an option for this user:");
            Console.WriteLine($"--------------------------");


            Console.ForegroundColor = ConsoleColor.Yellow;
            // Display user menu options

            Console.WriteLine("[1] List user's artists");
            Console.WriteLine("[2] Connect user to artist");
            Console.WriteLine("[3] List user's genres");
            Console.WriteLine("[4] Connect user to genre");
            Console.WriteLine("[5] List user's songs");
            Console.WriteLine("[6] Connect song to user");
            Console.WriteLine("[Q] Return to main menu");
            Console.ResetColor();
            string input = Console.ReadLine().ToLower();

            Console.ForegroundColor = originalColor;
            switch (input)
            {
                case "1":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Listing user's artists. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();

                    await ArtistMethods.ListUsersArtists(client, userId);
                    break;
                case "2":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();

                    await ArtistMethods.ConnectUserToArtist(client, userId);
                    break;
                case "3":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Listing user's genres. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();

                    await GenreMethods.ListUsersGenres(client, userId);
                    break;
                case "4":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();
                    await GenreMethods.ConnectUserToGenre(client, userId);
                    break;
                case "5":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"Listing user's songs. \nPress enter to return to menu.");
                    Console.WriteLine($"--------------------------");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();


                    await SongMethods.ListUserSongs(client, userId);
                    break;
                case "6":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();

                    await SongMethods.ConnectSongToUser(client, userId);
                    break;
                case "q":
                    Console.Clear();
                    ConsoleColor originalColor2 = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.ResetColor();
                    Console.WriteLine("Press enter to return to main menu.");
                    Console.ForegroundColor = originalColor2;

                    return;
                default:


                    //Save the orginal color
                    ConsoleColor originalColor1 = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Eek, wrong input!");
                    Console.WriteLine("Press enter to return to menu.");
                    Console.ReadLine();
                    Console.ForegroundColor = originalColor1;
                    Console.Clear();
                    break;
            }
        }
        // Asynchronously checks if a user with a specific ID exists
        public static async Task<bool> CheckIfUserExists(HttpClient client, int userId)
        {
            var response = await client.GetAsync("/user/");

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkRed;

                await Console.Out.WriteLineAsync($"Error finding user (status code: {response.StatusCode})");
                return false;
            }

            string responseData = await response.Content.ReadAsStringAsync();

            List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

            return users.Any(user => user.UserId == userId);
        }

        //Will print out a letter at a time with 100 milliseconds of pause in between
        public static void PrintOneByOne(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(100);
            }
        }
    }
}
