using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderDogProjectMusicClient.UnderdogApiModels
{
    internal class PutInSong
    {
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
    }
}
