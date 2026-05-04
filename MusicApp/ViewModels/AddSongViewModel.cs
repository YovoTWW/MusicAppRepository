using System.ComponentModel.DataAnnotations;
using static MusicApp.Constants.Genres;

namespace MusicApp.ViewModels
{

    public class AddSongViewModel
    {
        [MinLength(1, ErrorMessage = "Title cant be less than 1 characters long")]
        [MaxLength(50, ErrorMessage = "Title cant be more than 50 characters long")]
        public string Title { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public int YearReleased { get; set; }
        public string Genre { get; set; } = null!;
        public List<string> GenreList { get; set; } = MusicGenres;
        public string Creator { get; set; } = null!;
        public int Duration { get; set; }
        public string? WikiURL { get; set; }
        public string? PlayURL { get; set; } 
    }
}
