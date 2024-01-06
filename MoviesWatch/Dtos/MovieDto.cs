using System.ComponentModel.DataAnnotations;

namespace MoviesWatch.Dtos
{
    public class MovieDto
    {
        [MaxLength(250)]
        public string Title { get; set; }
        public double Rate { get; set; }
        public int Year { get; set; }
        [MaxLength(2500)]
        public string StoreLine { get; set; }
        public IFormFile Poster { get; set; }
        public byte GenreId { get; set; }
    }
}
