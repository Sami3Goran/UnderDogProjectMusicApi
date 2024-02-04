namespace UnderDogProjectMusicApi.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string ArtistTitle { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
