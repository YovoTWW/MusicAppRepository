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
        public async Task<IActionResult> Index(List<string> GenreFilters, List<string> songNames, string searchTextName)
        {
            ViewData["GenreFilters"] = GenreFilters;
            ViewData["SearchTextName"] = searchTextName;

            IQueryable<Song>? query = dbContext.Songs;

            if (songNames != null && songNames.Any())
            {
                query = query.Where(p => songNames.Contains(p.Title));
            }

            if (GenreFilters != null && GenreFilters.Any())
            {
                query = query.Where(p => GenreFilters.Contains(p.Genre));
            }

            if (!string.IsNullOrWhiteSpace(searchTextName))
            {
                //query = query.Where(e => e.Name.Contains(searchTextName, StringComparison.OrdinalIgnoreCase));
                var lowerSearch = searchTextName.ToLower();
                query = query.Where(e => e.Title.ToLower().Contains(lowerSearch));
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
                        Genre = p.Genre
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
                ImageURL = model.ImageURL
            };

            await dbContext.Songs.AddAsync(song);
            await dbContext.SaveChangesAsync();

            return this.RedirectToAction("Index");
        }

        [HttpGet]
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
                Creator = p.Creator
            }).FirstOrDefaultAsync();

            return this.View(model);
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
