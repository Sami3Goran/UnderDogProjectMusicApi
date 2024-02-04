namespace UnderDogProjectMusicApi.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string SongName { get; set; }
        public virtual int GenreId { get; set; }
        public virtual int ArtistId { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
