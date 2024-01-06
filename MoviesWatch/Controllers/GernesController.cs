using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GernesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GernesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Gerne = await _context.Gernes.ToListAsync();
            return Ok(Gerne);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto genre)
        {
            var Gerne = new Genre()
            {
                Name = genre.Name
            };
            await _context.Gernes.AddAsync(Gerne);
            _context.SaveChanges();
            return Ok(Gerne);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, GenreDto dto)
        {
            var Gerne = await _context.Gernes.FindAsync(id);
            if (Gerne == null)
            {
                return NotFound();
            }

            Gerne.Name = dto.Name;

            return Ok(Gerne);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Gerne = await _context.Gernes.SingleOrDefaultAsync(x=>x.Id == id);
            if(Gerne == null)
            {
                return NotFound($"This Gerne with Id = {id} Is Not Found !!");
            }

            _context.Gernes.Remove(Gerne);
            _context.SaveChanges();

            return Ok(Gerne);
        }
    }

}
