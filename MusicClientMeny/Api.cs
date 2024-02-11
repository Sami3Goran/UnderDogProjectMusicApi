using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UnderDogProjectMusicApi.Models.Dto;


class API
{
    private static readonly HttpClient httpClient = new HttpClient();
    private const string applicationUrl = "https://localhost:7211/";
    private const string GenresEndpoint = "genres";
    private const string ArtistsEndpoint = "artists";
    private const string SongsEndpoint = "songs";
    private const string UsersEndpoint = "users";

    public static async Task<GenreDto[]> GetGenresForUser(int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync($"{applicationUrl}{GenresEndpoint}/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<GenreDto[]>();
            }
            else
            {

                return null;
            }
        }
    }

    public static async Task<ArtistDto[]> GetArtistsForUser(int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync($"{applicationUrl}{ArtistsEndpoint}/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ArtistDto[]>();
            }
            else
            {

                return null;
            }
        }
    }

    public static async Task<SongDto[]> GetSongsForUser(int userId)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync($"{applicationUrl}{SongsEndpoint}/user/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SongDto[]>();
            }
            else
            {

                return null;
            }
        }
    }

    public static async Task<bool> AddGenreForUser(int userId, GenreDto genre)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{applicationUrl}{GenresEndpoint}/user/{userId}", genre);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {

                return false;
            }
        }
    }

    public static async Task<bool> AddArtistForUser(int userId, ArtistDto artist)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{applicationUrl}{ArtistsEndpoint}/user/{userId}", artist);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                // Hantera fel här
                return false;
            }
        }
    }

    public static async Task<bool> AddSongForUser(int userId, SongDto song)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{applicationUrl}{SongsEndpoint}/user/{userId}", song);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                // Hantera fel här
                return false;
            }
        }
    }

    static void ListUserGenres(int userId)
    {
        try
        {
            GenreDto[] genres = API.GetGenresForUser(userId).Result;

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

    static async Task AddSongForUser(int userId)
    {
        try
        {
            Console.Clear();

            // Hämta användarens befintliga artister
            ArtistDto[] artists = await API.GetArtistsForUser(userId);

            if (artists == null || artists.Length == 0)
            {
                Console.WriteLine("Error: No artists found for the user. Please add an artist first.");
                Console.ReadKey();
                return;
            }

            // Visa befintliga artister
            Console.WriteLine("Existing Artists:");
            for (int i = 0; i < artists.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {artists[i].ArtistTitle}");
            }

            // Användaren väljer en artist
            Console.Write("Select an artist (enter the artist number): ");
            int artistChoice = int.Parse(Console.ReadLine());

            // Validera användarens val av artist
            if (artistChoice < 1 || artistChoice > artists.Length)
            {
                Console.WriteLine("Error: Invalid artist choice.");
                Console.ReadKey();
                return;
            }

            ArtistDto selectedArtist = artists[artistChoice - 1];

            // Användaren anger låtinformation
            Console.Write("Enter song name: ");
            string songName = Console.ReadLine();

            // Skapa en ny sång
            SongDto newSong = new SongDto
            {
                SongName = songName,
                ArtistId = selectedArtist.ArtistId
            };

            // Försök att lägga till sången för användaren
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

    static void ListUserSongs(int userId)
    {
        try
        {
            Console.Clear();

            // Hämta användarens befintliga låtar
            SongDto[] songs = API.GetSongsForUser(userId).Result;

            if (songs == null || songs.Length == 0)
            {
                Console.WriteLine("No songs found for the user.");
            }
            else
            {
                Console.WriteLine("List of User's Songs:");
                foreach (var song in songs)
                {
                    Console.WriteLine($"- {song.SongName} (Artist: {song.ArtistName})");
                }
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