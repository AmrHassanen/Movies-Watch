namespace MoviesWatch.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Genre> Gernes { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
    }
}
