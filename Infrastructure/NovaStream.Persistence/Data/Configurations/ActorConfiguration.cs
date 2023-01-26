namespace NovaStream.Persistence.Data.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => new { a.Id });

        var actors = new[] 
        {
            new Actor() { Id = 1, Name= "Cillian", Surname="Murphy", About="Yaxshi Oglan" },
            new Actor() { Id = 2, Name= "Tom", Surname="Cruse", About="Babat Oglan" },
            new Actor() { Id = 3, Name= "Brad", Surname="Pitt", About="Zor Oglan" }
        };

        builder.HasData(actors);
    }
}
