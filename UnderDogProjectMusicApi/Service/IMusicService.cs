using System.Text.Json;

namespace UnderDogProjectMusicApi.Service
{
    public interface IMusicService
    {
        Task<Models.Dto.MusicDto> GetLatestAlbumAsync(string artist);
    }

    public class MusicService : IMusicService
    {
        private HttpClient _client;

        public MusicService() : this(new HttpClient()) { }

        public MusicService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Models.Dto.MusicDto> GetLatestAlbumAsync(string artist)
        {
            var result = await _client.GetAsync($"https://www.theaudiodb.com/api/v1/json/2/discography.php?s={artist}");

            result.EnsureSuccessStatusCode();

            Models.Dto.MusicDto content = JsonSerializer.Deserialize<Models.Dto.MusicDto>(await result.Content.ReadAsStringAsync());

            return content;
        }

    }
}
