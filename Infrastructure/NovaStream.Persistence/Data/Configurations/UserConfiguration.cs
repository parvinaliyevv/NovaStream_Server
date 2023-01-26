namespace NovaStream.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var user = new User()
        {
            Id = 1,
            Nickname = "Admin",
            Email = "admin@novastream.api",
            PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
            AvatarUrl = @"https://novastreamstorage.blob.core.windows.net/root/Avatars/avatar-1"
        };

        builder.HasData(user);
    }
}
