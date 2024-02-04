using System.Text.Json.Serialization;

namespace UnderDogProjectMusicApi.Models.Dto
{
    public class MusicDto
    {
        [JsonPropertyName("album")]
        public List<Album> Albums { get; set; }

        public class Album
        {
            [JsonPropertyName("strAlbum")]
            public string NameAlbum { get; set; }

            [JsonPropertyName("intYearReleased")]
            public string ReleseYear { get; set; }
        }
    }
}
