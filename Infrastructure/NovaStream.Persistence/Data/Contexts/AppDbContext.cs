namespace NovaStream.Persistence.Data.Contexts;

public class AppDbContext: DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<Serial> Serials { get; set; }
	public DbSet<Movie> Movies { get; set; }
	public DbSet<Episode> Episodes { get; set; }
	public DbSet<Season> Seasons { get; set; }


	public AppDbContext(DbContextOptions options): base(options) { }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new SerialConfiguration());
		modelBuilder.ApplyConfiguration(new MovieConfiguration());
		modelBuilder.ApplyConfiguration(new SeasonConfiguration());
		modelBuilder.ApplyConfiguration(new EpisodeConfiguration());
	}
}
