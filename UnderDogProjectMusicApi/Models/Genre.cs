namespace UnderDogProjectMusicApi.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreTitle { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
