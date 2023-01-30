﻿namespace NovaStream.Persistence.Data.Configurations;

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
                ConfirmationPIN = "",
                AvatarUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Avatars%2Favatar-1.png?alt=media&token=4fecc3bf-9511-4186-9c25-0347128c0181"
            },
            new()
            {
                Id = 2,
                Nickname = "tyler",
                Email = "novastream.tester@gmail.com",
                PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                OldPasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                ConfirmationPIN = "",
                AvatarUrl = @"https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Avatars%2Favatar-1.png?alt=media&token=4fecc3bf-9511-4186-9c25-0347128c0181"
            }
        };

        builder.HasData(users);
    }
}
