using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesWatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly new List<string> _allowedExtentions = new List<string>() {".jpg",".png" };
        private readonly long _allowedSizePoster = 10485760;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Movies =await _context
                .Movies
                .Include(x=>x.Genre)
                .OrderByDescending(x=>x.Rate)
                .ToListAsync();
            return Ok(Movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Movie = await _context.Movies.Include(x=>x.Genre).SingleOrDefaultAsync(x=>x.Id==id);
            if (Movie == null)
            {
                return NotFound();
            }
            return Ok(Movie);
        }

        [HttpGet("{GenreId}")]
        public async Task<IActionResult> GetALLByGenreID(int GenreId)
        {
            var Movie = await _context.Movies.SingleOrDefaultAsync(x=>x.GenreId==GenreId);
            if(Movie== null) 
            {
                return NotFound("The Type of This Films Not Found!!!!");
            }
            return Ok(Movie);
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto)
        {
            if(!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower())) 
            {
                return BadRequest("Only .Png and .Jpg Extentions");
            }
            if(dto.Poster.Length>_allowedSizePoster )
            {
                return BadRequest("The Size Not Allowed Please Enter Allowed Size");

            }

            var isValid = await _context.Gernes.AnyAsync(x => x.Id == dto.GenreId);
            if(!isValid)
            {
                return BadRequest("This is not found Genre");
            }


            using var dataStream= new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var Movie = new Movie()
            {
                Title = dto.Title,
                Rate = dto.Rate,
                Year= dto.Year,
                GenreId= dto.GenreId,
                StoreLine= dto.StoreLine,
                Poster= dataStream.ToArray(),
            };
            await _context.Movies.AddAsync(Movie);
            _context.SaveChanges();
            return Ok(Movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromForm]MovieDto dto)
        {
            if (dto.Poster == null)
            {
                return BadRequest();
            }
            var Movie = await _context.Movies.SingleOrDefaultAsync(x=>x.Id==id);
            if (Movie == null)
            {
                return NotFound();
            }

            var isValid = await _context.Gernes.AnyAsync(x => x.Id == dto.GenreId);
            if (!isValid)
            {
                return BadRequest("This is not found Genre");
            }

            if (dto.Poster!=null) 
            { 
            if (!_allowedExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .Png and .Jpg Extentions");
            }
            if (dto.Poster.Length > _allowedSizePoster)
            {
                return BadRequest("The Size Not Allowed Please Enter Allowed Size");
            }
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                Movie.Poster= dataStream.ToArray();
            }

                Movie.Title = dto.Title;
                Movie.Rate = dto.Rate;
                Movie.Year = dto.Year;
                Movie.StoreLine = dto.StoreLine;
                Movie.GenreId = dto.GenreId;

                _context.SaveChanges();

                return Ok(Movie);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var Movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
            if (Movie == null)
            {
                return NotFound($"no movie was found by id {id}");
            }
            else
            {
                _context.Movies.Remove(Movie);
                _context.SaveChanges();
                return Ok(Movie);
            }
        }

    }
}
