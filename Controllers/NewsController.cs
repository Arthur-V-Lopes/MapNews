using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsGlobe.Data;
using NewsGlobe.Models;

namespace NewsGlobe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public NewsController(AppDbContext db)
        {
            _db = db;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _db.News
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    id = n.Id,
                    titulo = n.Title,
                    descricao = n.Description,
                    lat = n.Latitude,
                    lon = n.Longitude,
                    createdAt = n.CreatedAt
                })
                .ToListAsync();

            return Ok(items);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NewsItem model)
        {
            if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
            {
                return BadRequest("Título e descrição são obrigatórios.");
            }

            model.CreatedAt = DateTime.UtcNow;
            _db.News.Add(model);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                id = model.Id,
                titulo = model.Title,
                descricao = model.Description,
                lat = model.Latitude,
                lon = model.Longitude,
                createdAt = model.CreatedAt
            });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _db.News.FindAsync(id);
            if (news == null)
                return NotFound("Notícia não encontrada.");

            _db.News.Remove(news);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Notícia removida com sucesso." });
        }
    }
}
