namespace NovaStream.Persistence.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        var users = new List<User>() 
        { 
            new()
            {
                Id = 1,
                Nickname = "Admin",
                Email = "admin@novastream.api",
                PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                OldPasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                AvatarUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-999admin.appspot.com/o/NovaStream%20Profile%20Avatar%2FA8.png?alt=media&token=c1712e32-5eaa-4808-8802-c4416ef2a3d4"
            },
            new()
            {
                Id = 2,
                Nickname = "tyler",
                Email = "novastream.tester@gmail.com",
                PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                OldPasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                AvatarUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-999admin.appspot.com/o/NovaStream%20Profile%20Avatar%2FA8.png?alt=media&token=c1712e32-5eaa-4808-8802-c4416ef2a3d4"
            }
        };

        builder.HasData(users);
    }
}
