using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicClient.UnderdogApiModels;

namespace UnderDogProjectMusicClient.Methods
{
    public class UserMethods
    {

        public static async Task ListAllUsers(HttpClient client)
        {
            var response = await client.GetAsync($"/user/");

            if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"Error listing user's (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu");
                return;
            }

            string responseData = await response.Content.ReadAsStringAsync();

            List<User> users = JsonSerializer.Deserialize<List<User>>(responseData);

            foreach (var user in users)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"User ID: {user.UserId}, username: {user.UserName}");
                Console.ResetColor();
            }

            Console.ReadLine();
        }

        public static async Task AddUser(HttpClient client)
        {
            await Console.Out.WriteAsync("Enter username: ");
            Console.ForegroundColor = ConsoleColor.White;
            string userName = Console.ReadLine();

            AddUser adduser = new AddUser()
            {
                UserName = userName
            };

            string json = JsonSerializer.Serialize(adduser);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/newUser", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                await Console.Out.WriteLineAsync($"Error creating new user (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                await Console.Out.WriteLineAsync($"Succesfully created a user with username: {userName} (status code: {response.StatusCode}). " +
                    $"\nPress enter to return to menu.");
                Console.ResetColor();
            }
            Console.ReadLine();
            Console.Clear();
        }
    }
}
