using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Data.Models;
using System.Diagnostics;
using MusicApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(List<string> GenreFilters,string searchTextName, string searchTextCreator, string YearFilters)
        {
            ViewData["GenreFilters"] = GenreFilters;
            ViewData["SearchTextName"] = searchTextName;
            ViewData["SearchTextCreator"] = searchTextCreator;
            ViewData["YearFilters"] = YearFilters;

            IQueryable<Song>? query = dbContext.Songs;       

            if (GenreFilters != null && GenreFilters.Any())
            {
                query = query.Where(p => GenreFilters.Contains(p.Genre));
            }

            if (!string.IsNullOrWhiteSpace(searchTextName))
            {
                var lowerSearch = searchTextName.ToLower();
                query = query.Where(e => e.Title.ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrWhiteSpace(searchTextCreator))
            {
                var lowerSearch = searchTextCreator.ToLower();
                query = query.Where(e => e.Creator.ToLower().Contains(lowerSearch));
            }

            if (!string.IsNullOrWhiteSpace(YearFilters))
            {
                query = query.Where(e => e.YearReleased == int.Parse(YearFilters));
            }

            var model = await query
                    .Select(p => new BasicSongViewModel()
                    {
                        Id = p.Id,
                        ImageURL = p.ImageURL,
                        Title = p.Title,
                        YearReleased = p.YearReleased,
                        Duration = p.Duration,
                        Creator = p.Creator,
                        Genre = p.Genre,
                        WikiURL = p.WikiURL ?? null,
                        PlayURL = p.PlayURL ?? null
                    }).AsNoTracking().ToListAsync();

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddSongViewModel();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSongViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }


            Song song = new Song
            {
                Title = model.Title,
                Creator = model.Creator,
                Duration = model.Duration,
                YearReleased = model.YearReleased,
                Genre = model.Genre,
                ImageURL = model.ImageURL,
                WikiURL = model.WikiURL,
                PlayURL = model.PlayURL
            };

            await dbContext.Songs.AddAsync(song);
            await dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index");
        }

        /*[HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = await dbContext.Songs.Where(p => p.Id == id).AsNoTracking().Select(p => new BasicSongViewModel
            {
                Id = p.Id,               
                ImageURL = p.ImageURL,
                Title = p.Title,
                Duration = p.Duration,
                YearReleased = p.YearReleased,
                Genre = p.Genre,
                Creator = p.Creator,
                WikiURL = p.WikiURL,
                PlayURL = p.PlayURL
            }).FirstOrDefaultAsync();

            return this.View(model);
        }*/

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await dbContext.Songs.Where(p => p.Id == id).AsNoTracking().Select(p => new AddSongViewModel
            {
                ImageURL = p.ImageURL,
                Title = p.Title,
                Duration = p.Duration,
                YearReleased = p.YearReleased,
                Genre = p.Genre,
                Creator = p.Creator,
                WikiURL = p.WikiURL ?? null,
                PlayURL = p.PlayURL ?? null
            }).FirstOrDefaultAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddSongViewModel model , Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }


            Song? song = await dbContext.Songs.FindAsync(id);

            if (song == null)
            {
                throw new ArgumentException("Invalid id");
            }


            song.Title = model.Title;
            song.Creator = model.Creator;
            song.Duration = model.Duration;
            song.YearReleased = model.YearReleased;
            song.Genre = model.Genre;
            song.ImageURL = model.ImageURL;
            song.WikiURL = model.WikiURL;
            song.PlayURL = model.PlayURL;

            await dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
