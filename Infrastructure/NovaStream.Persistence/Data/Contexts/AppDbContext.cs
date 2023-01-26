namespace NovaStream.Persistence.Data.Contexts;

public class AppDbContext: DbContext
{
	public DbSet<User> Users { get; set; }
	public DbSet<Movie> Movies { get; set; }
	public DbSet<Serial> Serials { get; set; }
	public DbSet<Season> Seasons { get; set; }
	public DbSet<Episode> Episodes { get; set; }
	public DbSet<InComing> InComings { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<MovieMark> MovieMarks { get; set; }
	public DbSet<SerialMark> SerialMarks { get; set; }
	public DbSet<MovieCategory> MovieCategories { get; set; }
	public DbSet<SerialCategory> SerialCategories { get; set; }
	public DbSet<InComingCategory> InComingCategories { get; set; }


	public AppDbContext(DbContextOptions options): base(options) { }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new MovieConfiguration());
		modelBuilder.ApplyConfiguration(new SerialConfiguration());
		modelBuilder.ApplyConfiguration(new SeasonConfiguration());
		modelBuilder.ApplyConfiguration(new EpisodeConfiguration());
		modelBuilder.ApplyConfiguration(new CategoryConfiguration());
		modelBuilder.ApplyConfiguration(new InComingConfiguration());
		modelBuilder.ApplyConfiguration(new MovieMarkConfiguration());
		modelBuilder.ApplyConfiguration(new SerialMarkConfiguration());
		modelBuilder.ApplyConfiguration(new MovieCategoryConfiguration());
		modelBuilder.ApplyConfiguration(new SerialCategoryConfiguration());
		modelBuilder.ApplyConfiguration(new InComingCategoryConfiguration());
	}
}
