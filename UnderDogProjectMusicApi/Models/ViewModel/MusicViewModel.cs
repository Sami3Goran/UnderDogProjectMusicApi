using System.Text.Json.Serialization;

namespace UnderDogProjectMusicApi.Models.ViewModel
{
    public class MusicViewModel
    {
        [JsonPropertyName("album")]
        public List<ViewAlbum> ViewAlbums { get; set; }

        public class ViewAlbum
        {
            [JsonPropertyName("strAlbum")]
            public string Name { get; set; }

            [JsonPropertyName("intYearReleased")]
            public string Year { get; set; }
        }
    }
}
