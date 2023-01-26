namespace NovaStream.Persistence.Data.Configurations;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        var actors = new Actor[] 
        {
            new()
            {
                Id = 1,
                Name = "Matthew",
                Surname = "McConaughey",
                ImageUrl = @"Images/Actors/Matthew-McConaughey-image.jpg",
                About = "American actor and producer Matthew David McConaughey was born in Uvalde, Texas. His mother, Mary Kathleen (McCabe), is a substitute school teacher originally from New Jersey. His father, James Donald McConaughey, was a Mississippi-born gas station owner who ran an oil pipe supply business. He is of Irish, Scottish, German, English, and Swedish descent. Matthew grew up in Longview, Texas, where he graduated from the local High School (1988). Showing little interest in his father's oil business, which his two brothers later joined, Matthew was longing for a change of scenery, and spent a year in Australia, washing dishes and shoveling chicken manure. Back to the States, he attended the University of Texas in Austin, originally wishing to be a lawyer. But, when he discovered an inspirational Og Mandino book \"The Greatest Salesman in the World\" before one of his final exams, he suddenly knew he had to change his major from law to film."
            },
            new()
            {
                Id = 2,
                Name = "Anne",
                Surname = "Hathaway",
                ImageUrl = @"Images/Actors/Anne-Hathaway-image.jpg",
                About = "Anne Jacqueline Hathaway was born in Brooklyn, New York, to Kate McCauley Hathaway, an actress, and Gerald T. Hathaway, a lawyer, both originally from Philadelphia. She is of mostly Irish descent, along with English, German, and French. Her first major role came in the short-lived television series Get Real (1999). She gained widespread recognition for her roles in The Princess Diaries (2001) and its 2004 sequel as a young girl who discovers she is a member of royalty, opposite Julie Andrews and Heather Matarazzo."
            },
            new()
            {
                Id = 3,
                Name = "Jessica",
                Surname = "Chastain",
                ImageUrl = @"Images/Actors/Jessica-Chastain-image.jpg",
                About = "Jessica Michelle Chastain was born in Sacramento, California, and was raised in a middle-class household in a Northern California suburb. Her mother, Jerri Chastain, is a vegan chef whose family is originally from Kansas, and her stepfather is a fireman. She discovered dance at the age of nine and was in a dance troupe by age thirteen. She began performing in Shakespearean productions all over the Bay area."
            },
            new()
            {
                Id = 4,
                Name = "Pedro",
                Surname = "Pascal",
                ImageUrl = @"Images/Actors/Pedro-Pascal-image.jpg",
                About = "Pedro Pascal is a Chilean-born actor. He is best known for portraying the roles of Oberyn Martell in the fourth season of the HBO series Game of Thrones (2011), Javier Peña in the Netflix series Narcos (2015), the titular character in the Disney+ series The Mandalorian (2019) and Joel Miller in the HBO series The Last of Us (2023)."
            },
            new()
            {
                Id = 5,
                Name = "Bella",
                Surname = "Ramsey",
                ImageUrl = @"Images/Actors/Bella-Ramsey-Image.jpg",
                About = "Bella Ramsey made her professional acting debut as fierce young noblewoman Lyanna Mormont in Season 6 of HBO's 'Game of Thrones', a role that quickly became a fan favorite and saw Bella return for the next 2 seasons. Bella will be returning to HBO as the leading role of 'Ellie Williams' in their new flagship show 'The Last of Us' opposite Pedro Pascal. Bella is also known for playing the titular character Mildred Hubble in the newest adaptation of 'The Worst Witch' for which she won the Young Performer BAFTA in 2019. Bella lends her voice to 'Hilda', an award winning animation series for Netflix. Bella was recently on screens in the second season of BBC/HBO's adaptation of 'His Dark Materials'."
            },
            new()
            {
                Id = 6,
                Name = "Gabriel",
                Surname = "Luna",
                ImageUrl = @"Images/Actors/Gabriel-Luna-image.jpg",
                About = "Gabriel Luna was born on December 5, 1982 in Austin, Texas, USA. He is an actor and producer, known for Terminator: Dark Fate (2019), Agents of S.H.I.E.L.D. (2013) and Bernie (2011). He has been married to Smaranda Luna since February 20, 2011."
            },
            new()
            {
                Id = 7,
                Name = "Leonardo",
                Surname = "DiCaprio",
                ImageUrl = @"Images/Actors/Leonardo-DiCaprio-image.jpg",
                About = "Few actors in the world have had a career quite as diverse as Leonardo DiCaprio's. DiCaprio has gone from relatively humble beginnings, as a supporting cast member of the sitcom Growing Pains (1985) and low budget horror movies, such as Critters 3 (1991), to a major teenage heartthrob in the 1990s, as the hunky lead actor in movies such as Romeo + Juliet (1996) and Titanic (1997), to then become a leading man in Hollywood blockbusters, made by internationally renowned directors such as Martin Scorsese and Christopher Nolan.\r\n"
            },
            new()
            {
                Id = 8,
                Name = "Joseph",
                Surname = "Gordon-Levitt",
                ImageUrl = @"Images/Actors/Joseph-Gordon-Levitt-image.jpg",
                About = "Joseph Leonard Gordon-Levitt was born February 17, 1981 in Los Angeles, California, to Jane Gordon and Dennis Levitt. Joseph was raised in a Jewish family with his late older brother, Dan Gordon-Levitt, who passed away in October 2010. His parents worked for the Pacifica Radio station KPFK-FM and his maternal grandfather, Michael Gordon, had been a well-known movie director. Joseph first became well known for his starring role on NBC's award-winning comedy series 3rd Rock from the Sun (1996). During his six seasons on the show, he won two YoungStar Awards and also shared in three Screen Actors Guild Award® nominations for Outstanding Performance by a Comedy Series Ensemble."
            },
            new()
            {
                Id = 9,
                Name = "Tom",
                Surname = "Hardy",
                ImageUrl = @"Images/Actors/Tom-Hardy-image.jpg",
                About = "With his breakthrough performance as Eames in Christopher Nolan's sci-fi thriller Inception (2010), English actor Tom Hardy has been brought to the attention of mainstream audiences worldwide. However, the versatile actor has been steadily working on both stage and screen since his television debut in the miniseries Band of Brothers (2001). After being cast in the World War II drama, Hardy left his studies at the prestigious Drama Centre in London and was subsequently cast as Twombly in Ridley Scott's Black Hawk Down (2001) and as the villain Shinzon in Star Trek: Nemesis (2002)."
            }
        };

        builder.HasData(actors);
    }
}
