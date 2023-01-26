﻿namespace NovaStream.Persistence.Data.Contexts;

public class AppDbContext: DbContext
{
	public DbSet<User> Users { get; set; }


	public AppDbContext(DbContextOptions options): base(options) { }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new UserConfiguration());
	}
}
