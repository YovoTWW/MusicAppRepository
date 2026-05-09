using System.ComponentModel.DataAnnotations;

namespace MusicApp.Data.Models
{
    public class Song
    {
        public Song()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Creator { get; set; }

        [Required]
        public int YearReleased { get; set; }

        [Required]
        public string Genre { get; set; }
        public string? ImageURL { get; set; }
        public string? WikiURL { get; set; }
        public string? PlayURL { get; set; }
    }
}
