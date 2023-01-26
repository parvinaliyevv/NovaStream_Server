namespace NovaStream.Persistence.Data.Contexts;

public class AppDbContext: DbContext
{


	public AppDbContext(DbContextOptions options): base(options) { }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		
	}
}
