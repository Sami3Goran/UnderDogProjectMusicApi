using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UnderDogProjectMusicClient.UnderdogApiModels
{
    internal class AlbumFinder
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
