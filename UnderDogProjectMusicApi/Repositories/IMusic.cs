using UnderDogProjectMusicApi.Data;
using UnderDogProjectMusicApi.Models.Dto;
using UnderDogProjectMusicApi.Models.ViewModel;
using UnderDogProjectMusicApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace UnderDogProjectMusicApi.Repositories
{
    //här använder vi oss av dependency injection.
    public interface IMusic
    {
        void AddUser(UserDto user);
        List<ListUserViewModel> ListAllUsers();

        List<ArtistViewModel> ListUsersArtists(int userId);
        void AttachUserToArtist(int userId, int songId);

        List<ListGenreViewModel> GetAllGenresForUser(int userId);

        void AttachGenreForUser(int genreId, int userId);

        List<SongArtistViewModel> ListUserSongs(int userId);
        void AttachSongToUser(int userId, int songId);

        void PutInSong(PutInSongsDto song);

        void HandleError(string errorMessage);

        bool ExistingUser(string username);

        void AddNewArtist(ArtistDto artistDto);

        void AddNewGenre(GenreDto newGenre);
    }

    public class Music : IMusic
    {
        private readonly ApplicationContext _context;
        public Music(ApplicationContext context)
        {
            _context = context;
        }

        //Metoder för att förenkla de andra metoderna
        public bool ExistingUser(string username)
        {
            return _context.Users.Any(u => u.UserName == username);
        }

        public void HandleError(string errorMessage)
        {
            throw new InvalidOperationException(errorMessage);
        }

        // lägger till användare
        public void AddUser(UserDto user)
        {
            //fel meddelande tillkommer om användaren existerar, eller det inte angivits namn.
            if (string.IsNullOrEmpty(user.UserName))
            {
                HandleError("The User must have a name");
                return;
            }
            if (ExistingUser(user.UserName))
            {
                HandleError("This name already exist, please select another one");
                return;
            }

            //användaren läggs till i databasen.
            _context.Users.Add(new User()
            {
                UserName = user.UserName
            });
            _context.SaveChanges();
        }

        // tar fram användare
        public List<ListUserViewModel> ListAllUsers()
        {
            //hämtar alla användare
            return _context.Users
                .Select(u => new ListUserViewModel 
                { UserId = u.UserId, UserName = u.UserName })
                .ToList();
        }

        //tar fram artister som är kopplade till ancändare
        public List<ArtistViewModel> ListUsersArtists(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Artists)
                .SingleOrDefault(u => u.UserId == userId);

            //Om användaren inte finns.
            if (user == null)
            {
                HandleError("The User does not exist");
                return new List<ArtistViewModel>();
            }

            // om användaren hittas kommer en lista på deras artister.
            List<ArtistViewModel> result = user.Artists
               .Select(a => new ArtistViewModel()
               {
                   ArtistTitle = a.ArtistTitle
               }).ToList();

            return result;
        }

        //ifall du vill koppla en artist till en användare, skapar även en artist om den inte exist.
        public void AttachUserToArtist(int userId, int artistId)
        {
            User? user = _context.Users
            .Include(u => u.Artists)
            .SingleOrDefault(u => u.UserId == userId);
            Artist? artist = _context.Artists
            .SingleOrDefault(a => a.ArtistId == artistId);

            if (user == null)
            {
                HandleError("The user doesn't exist");
                return;
            }
            if (artist == null)
            {
                HandleError("The Artist doesn't exist");
                return;
            }

            //se ifall kopplingen mellan artist och användare existerar, om inte lägg till den.
            if (!user.Artists.Any(a => a.ArtistId == artistId))
            {
                user.Artists.Add(artist);
                _context.SaveChanges();
            }
            else
            {
                HandleError("This user already likes this artist");
                return;
            }
        }

        // tar fram genre som är kopplade till användare
        public List<ListGenreViewModel> GetAllGenresForUser(int userId)
        {
            // hämta alla genre som är kopplade till en användare
            User? user = _context.Users
                 .Include(u => u.Genres)
                 .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                HandleError("The User doesnt exist");
                return new List<ListGenreViewModel>();
            }

            List<ListGenreViewModel> result = user.Genres
                   .Select(a => new ListGenreViewModel()
                   {
                       GenreTitle = a.GenreTitle,
                   }).ToList();
            return result;

        }

        // kopplar genre till användare
        public void AttachGenreForUser(int genreId, int userId)
        {
            // lägg till en specifik genre för en specifik användare.
            User? user = _context.Users
                .Include(u => u.Genres)
            .SingleOrDefault(u => u.UserId == userId);

            Genre? genre = _context.Genres
              .SingleOrDefault(g => g.GenreId == genreId);

            if (user == null)
            {
                HandleError("The user doesnt exist");
                return;
            }
            if (genre == null)
            {
                HandleError("The Genre doesnt exist.");
                return;
            }

            user.Genres.Add(genre);

            _context.SaveChanges();

        }

        // List all Songs for a Specific User
        public List<SongArtistViewModel> ListUserSongs(int userId)
        {
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                HandleError("User doesnt exist");
                return new List<SongArtistViewModel>();
            }
            // alla låtar som är kopplade till en användare blir listade.
            List<SongArtistViewModel> userSongs = user.Songs
                .Join(
                    _context.Artists,
                    song => song.ArtistId,
                    artist => artist.ArtistId,
                    (song, artist) => new SongArtistViewModel
                    {
                        Title = song.SongName,
                        ArtistName = artist.ArtistTitle
                    })
                .ToList();

            return userSongs;
        }

        //kopplar låt till användare
        public void AttachSongToUser(int userId, int songId)
        {
            // lägg till en låt till en användare
            User? user = _context.Users
                .Include(u => u.Songs)
                .SingleOrDefault(u => u.UserId == userId);

            Song? song = _context.Songs
                .SingleOrDefault(s => s.SongId == songId);

            if (user == null)
            {
                HandleError("The User doesnt exist");
                return;
            }
            if (song == null)
            {
                HandleError("The song doesnt exist");
                return;
            }

            user.Songs.Add(song);
            _context.SaveChanges();
        }

        //lägger in låt i databas
        public void PutInSong(PutInSongsDto song)
        {
            //lägg till en låt i databasen.
            if (string.IsNullOrEmpty(song.Name))
            {
                HandleError("The song must have a Name");
                return;
            }

            var artist = _context.Artists.Find(song.ArtistId);
            var genre = _context.Genres.Find(song.GenreId);

            if (artist == null)
            {
                throw new Exception("The artist doesnt exist");
            }
            if (genre == null)
            {
                throw new Exception("The genre doesnt exist");
            }
            
            var newSong = new Song
            {
                SongName = song.Name,
                ArtistId = song.ArtistId,
                GenreId = song.GenreId,

            };
            _context.Songs.Add(newSong);
            _context.SaveChanges();
        }

        //ifall du vill endast lägga till en Artist i databasen
        public void AddNewArtist(ArtistDto artistDto)
        {
            var allArtists =
                _context.Artists
                .ToArray();
            if (allArtists.Any(a => a.ArtistTitle == artistDto.ArtistTitle))
            {
                HandleError($"{artistDto.ArtistTitle} already exists.");
                return;
            }
            else if (string.IsNullOrEmpty(artistDto.ArtistTitle))
            {
                HandleError("You must write an artist name.");
                return;
            }
            else
            {
                var newArtist = new Artist
                {
                    ArtistTitle = artistDto.ArtistTitle,
                    Description = artistDto.Description
                };
                _context.Artists.Add(newArtist);
                _context.SaveChanges();
            }
        }

        //ifall du vill lägga till en genre i databasen
        public void AddNewGenre(GenreDto addGenre)

        {
            var allGenres = _context.Genres
            .ToArray();

            if (allGenres.Any(x => x.GenreTitle == addGenre.GenreTitle))
            {
                HandleError("Genre already exists");
                return;
            }

            if (string.IsNullOrEmpty(addGenre.GenreTitle))
            {
                HandleError("User entered empty field");
                return;
            }

            var genre = new Genre
            {
                GenreTitle = addGenre.GenreTitle
            };

            _context.Genres.Add(genre);
            _context.SaveChanges();
        }

    }
}
