namespace UnderDogProjectMusicApi.Models.Dto
{
    public class PutInSongsDto
    {
        public string Name { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
    }
}
