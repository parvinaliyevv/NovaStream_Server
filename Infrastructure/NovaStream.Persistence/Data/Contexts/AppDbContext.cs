namespace NovaStream.Persistence.Data.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Director> Directors { get; set; }

    public DbSet<Soon> Soons { get; set; }
    public DbSet<SoonGenre> SoonGenres { get; set; }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieMark> MovieMarks { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; }

    public DbSet<Serial> Serials { get; set; }
    public DbSet<SerialMark> SerialMarks { get; set; }
    public DbSet<SerialActor> SerialActors { get; set; }
    public DbSet<SerialGenre> SerialGenres { get; set; }


    public AppDbContext(DbContextOptions options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
