namespace NovaStream.Persistence.Data.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        var actors = new Actor[] 
        {
            new() { Id = 1, Name= "Cillian", Surname="Murphy", About="Yaxshi Oglan", ImageUrl = "baza" },
            new() { Id = 2, Name= "Tom", Surname="Cruse", About="Babat Oglan", ImageUrl = "baza" },
            new() { Id = 3, Name= "Brad", Surname="Pitt", About="Zor Oglan", ImageUrl = "baza" }
        };

        builder.HasData(actors);
    }
}
