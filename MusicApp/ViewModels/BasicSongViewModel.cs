namespace MusicApp.ViewModels
{
    public class BasicSongViewModel
    {       
            public Guid Id { get; set; }
            public string Title { get; set; } = null!;
            public string? ImageURL { get; set; }
            public int YearReleased { get; set; }
            public string Genre{ get; set; } = null!;
            public string Creator { get; set; } = null!;
            public int Duration { get; set; }
            public string? WikiURL { get; set; } 
            public string? PlayURL { get; set; } 
    }
}
