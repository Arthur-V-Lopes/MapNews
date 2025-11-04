using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsGlobe.Data;
using NewsGlobe.Models;

namespace NewsGlobe.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) { _db = db; }

        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsItem model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.News.Add(model);
            await _db.SaveChangesAsync();
            TempData["ok"] = "Notícia cadastrada!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> List()
        {
            var items = await _db.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(items);
        }
    }
}
