namespace NovaStream.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => new { u.Email });

        var user = new User()
        {
            Id = 1,
            Email = "admin@novastream.api",
            PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918"
        };

        builder.HasData(user);
    }
}
