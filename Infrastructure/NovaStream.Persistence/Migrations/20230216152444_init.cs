using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaStream.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Soons",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrailerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soons", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoLength = table.Column<TimeSpan>(type: "time", nullable: false),
                    ProducerId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Movies_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Serials",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProducerId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serials", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Serials_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SoonGenres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    SoonName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoonGenres", x => new { x.SoonName, x.GenreId });
                    table.ForeignKey(
                        name: "FK_SoonGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoonGenres_Soons_SoonName",
                        column: x => x.SoonName,
                        principalTable: "Soons",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieActors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieActors", x => new { x.MovieName, x.ActorId });
                    table.ForeignKey(
                        name: "FK_MovieActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieActors_Movies_MovieName",
                        column: x => x.MovieName,
                        principalTable: "Movies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieName, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieName",
                        column: x => x.MovieName,
                        principalTable: "Movies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieMarks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieMarks", x => new { x.MovieName, x.UserId });
                    table.ForeignKey(
                        name: "FK_MovieMarks_Movies_MovieName",
                        column: x => x.MovieName,
                        principalTable: "Movies",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieMarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    SerialName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Serials_SerialName",
                        column: x => x.SerialName,
                        principalTable: "Serials",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialActors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false),
                    SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialActors", x => new { x.SerialName, x.ActorId });
                    table.ForeignKey(
                        name: "FK_SerialActors_Actors_ActorId",
                        column: x => x.ActorId,
                        principalTable: "Actors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SerialActors_Serials_SerialName",
                        column: x => x.SerialName,
                        principalTable: "Serials",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialGenres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialGenres", x => new { x.SerialName, x.GenreId });
                    table.ForeignKey(
                        name: "FK_SerialGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SerialGenres_Serials_SerialName",
                        column: x => x.SerialName,
                        principalTable: "Serials",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialMarks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialMarks", x => new { x.SerialName, x.UserId });
                    table.ForeignKey(
                        name: "FK_SerialMarks_Serials_SerialName",
                        column: x => x.SerialName,
                        principalTable: "Serials",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SerialMarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoLength = table.Column<TimeSpan>(type: "time", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "About", "ImageUrl", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "American actor and producer Matthew David McConaughey was born in Uvalde, Texas. His mother, Mary Kathleen (McCabe), is a substitute school teacher originally from New Jersey. His father, James Donald McConaughey, was a Mississippi-born gas station owner who ran an oil pipe supply business. He is of Irish, Scottish, German, English, and Swedish descent. Matthew grew up in Longview, Texas, where he graduated from the local High School (1988). Showing little interest in his father's oil business, which his two brothers later joined, Matthew was longing for a change of scenery, and spent a year in Australia, washing dishes and shoveling chicken manure. Back to the States, he attended the University of Texas in Austin, originally wishing to be a lawyer. But, when he discovered an inspirational Og Mandino book \"The Greatest Salesman in the World\" before one of his final exams, he suddenly knew he had to change his major from law to film.", "Images/Actors/Matthew-McConaughey-image.jpg", "Matthew", "McConaughey" },
                    { 2, "Anne Jacqueline Hathaway was born in Brooklyn, New York, to Kate McCauley Hathaway, an actress, and Gerald T. Hathaway, a lawyer, both originally from Philadelphia. She is of mostly Irish descent, along with English, German, and French. Her first major role came in the short-lived television series Get Real (1999). She gained widespread recognition for her roles in The Princess Diaries (2001) and its 2004 sequel as a young girl who discovers she is a member of royalty, opposite Julie Andrews and Heather Matarazzo.", "Images/Actors/Anne-Hathaway-image.jpg", "Anne", "Hathaway" },
                    { 3, "Jessica Michelle Chastain was born in Sacramento, California, and was raised in a middle-class household in a Northern California suburb. Her mother, Jerri Chastain, is a vegan chef whose family is originally from Kansas, and her stepfather is a fireman. She discovered dance at the age of nine and was in a dance troupe by age thirteen. She began performing in Shakespearean productions all over the Bay area.", "Images/Actors/Jessica-Chastain-image.jpg", "Jessica", "Chastain" },
                    { 4, "Pedro Pascal is a Chilean-born actor. He is best known for portraying the roles of Oberyn Martell in the fourth season of the HBO series Game of Thrones (2011), Javier Peña in the Netflix series Narcos (2015), the titular character in the Disney+ series The Mandalorian (2019) and Joel Miller in the HBO series The Last of Us (2023).", "Images/Actors/Pedro-Pascal-image.jpg", "Pedro", "Pascal" },
                    { 5, "Bella Ramsey made her professional acting debut as fierce young noblewoman Lyanna Mormont in Season 6 of HBO's 'Game of Thrones', a role that quickly became a fan favorite and saw Bella return for the next 2 seasons. Bella will be returning to HBO as the leading role of 'Ellie Williams' in their new flagship show 'The Last of Us' opposite Pedro Pascal. Bella is also known for playing the titular character Mildred Hubble in the newest adaptation of 'The Worst Witch' for which she won the Young Performer BAFTA in 2019. Bella lends her voice to 'Hilda', an award winning animation series for Netflix. Bella was recently on screens in the second season of BBC/HBO's adaptation of 'His Dark Materials'.", "Images/Actors/Bella-Ramsey-Image.jpg", "Bella", "Ramsey" },
                    { 6, "Gabriel Luna was born on December 5, 1982 in Austin, Texas, USA. He is an actor and producer, known for Terminator: Dark Fate (2019), Agents of S.H.I.E.L.D. (2013) and Bernie (2011). He has been married to Smaranda Luna since February 20, 2011.", "Images/Actors/Gabriel-Luna-image.jpg", "Gabriel", "Luna" },
                    { 7, "Few actors in the world have had a career quite as diverse as Leonardo DiCaprio's. DiCaprio has gone from relatively humble beginnings, as a supporting cast member of the sitcom Growing Pains (1985) and low budget horror movies, such as Critters 3 (1991), to a major teenage heartthrob in the 1990s, as the hunky lead actor in movies such as Romeo + Juliet (1996) and Titanic (1997), to then become a leading man in Hollywood blockbusters, made by internationally renowned directors such as Martin Scorsese and Christopher Nolan.\r\n", "Images/Actors/Leonardo-DiCaprio-image.jpg", "Leonardo", "DiCaprio" },
                    { 8, "Joseph Leonard Gordon-Levitt was born February 17, 1981 in Los Angeles, California, to Jane Gordon and Dennis Levitt. Joseph was raised in a Jewish family with his late older brother, Dan Gordon-Levitt, who passed away in October 2010. His parents worked for the Pacifica Radio station KPFK-FM and his maternal grandfather, Michael Gordon, had been a well-known movie director. Joseph first became well known for his starring role on NBC's award-winning comedy series 3rd Rock from the Sun (1996). During his six seasons on the show, he won two YoungStar Awards and also shared in three Screen Actors Guild Award® nominations for Outstanding Performance by a Comedy Series Ensemble.", "Images/Actors/Joseph-Gordon-Levitt-image.jpg", "Joseph", "Gordon-Levitt" },
                    { 9, "With his breakthrough performance as Eames in Christopher Nolan's sci-fi thriller Inception (2010), English actor Tom Hardy has been brought to the attention of mainstream audiences worldwide. However, the versatile actor has been steadily working on both stage and screen since his television debut in the miniseries Band of Brothers (2001). After being cast in the World War II drama, Hardy left his studies at the prestigious Drama Centre in London and was subsequently cast as Twombly in Ridley Scott's Black Hawk Down (2001) and as the villain Shinzon in Star Trek: Nemesis (2002).", "Images/Actors/Tom-Hardy-image.jpg", "Tom", "Hardy" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Images/Genres/drama-image.png", "Drama" },
                    { 2, "Images/Genres/adventure-image.png", "Adventure" },
                    { 3, "Images/Genres/sci-fi-image.png", "Sci-Fi" },
                    { 4, "Images/Genres/action-image.png", "Action" },
                    { 5, "Images/Genres/horror-image.png", "Horror" },
                    { 6, "Images/Genres/thriller-image.png", "Thriller" },
                    { 7, "Images/Genres/crime-image.png", "Crime" },
                    { 8, "Images/Genres/comedy-image.png", "Comedy" }
                });

            migrationBuilder.InsertData(
                table: "Producers",
                columns: new[] { "Id", "About", "ImageUrl", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "Best known for his cerebral, often nonlinear, storytelling, acclaimed writer-director Christopher Nolan was born on July 30, 1970, in London, England. Over the course of 15 years of filmmaking, Nolan has gone from low-budget independent films to working on some of the biggest blockbusters ever made.", "Images/Producers/Christopher-Nolan-image.jpg", "Christopher", "Nolan" },
                    { 2, "Craig Mazin was born on April 8, 1971 in Brooklyn, New York, USA. He is a producer and writer, known for Chernobyl (2019), The Hangover Part II (2011) and Identity Thief (2013).", "Images/Producers/Craig-Mazin-image.jpg", "Craig", "Mazin" }
                });

            migrationBuilder.InsertData(
                table: "Soons",
                columns: new[] { "Name", "Description", "OutDate", "TrailerImageUrl", "TrailerUrl" },
                values: new object[,]
                {
                    { "Guardians of the Galaxy Vol. 3", "Still reeling from the loss of Gamora, Peter Quill rallies his team to defend the universe and one of their own - a mission that could mean the end of the Guardians if not successful.", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer-image.jpg", "Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer.mp4" },
                    { "John Wick: Chapter 4", "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.", new DateTime(2023, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soons/John Wick Chapter 4/john-wick-chapter-4-trailer-image.jpg", "Soons/John Wick Chapter 4/john-wick-chapter-4-trailer.mp4" },
                    { "Transformers: Rise of the Beasts", "Plot unknown. Reportedly based on the 'Transformers' spinoff 'Beast Wars' which feature robots that transform into robotic animals.", new DateTime(2023, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soons/Transformers Rise of the Beasts/transformers-rise-of-the-beasts-trailer-image.jpg", "Soons/Transformers Rise of the Beasts/transformers-rise-of-the-beasts-trailer.mp4" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "Email", "Nickname", "OldPasswordHash", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Avatars%2Favatar-1.png?alt=media&token=4fecc3bf-9511-4186-9c25-0347128c0181", "admin@novastream.api", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" },
                    { 2, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Avatars%2Favatar-1.png?alt=media&token=4fecc3bf-9511-4186-9c25-0347128c0181", "novastream.tester@gmail.com", "tyler", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Name", "Age", "Description", "ImageUrl", "ProducerId", "SearchImageUrl", "TrailerUrl", "VideoDescription", "VideoImageUrl", "VideoLength", "VideoName", "VideoUrl", "Year" },
                values: new object[,]
                {
                    { "Inception", 16, "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.", "Movies/Inception/inception-image.jpg", 1, "Movies/Inception/inception-search-image.jpg", "Movies/Inception/inception-trailer.mp4", "A troubled thief who extracts secrets from people's dreams takes one last job: leading a dangerous mission to plant an idea in a target's subconscious.", "Movies/Inception/inception-video-image.jpg", new TimeSpan(0, 2, 28, 0, 0), "Inception", "Movies/Inception/inception-video-720p.mkv", 2010 },
                    { "Interstellar", 13, "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.", "Movies/Interstellar/interstellar-image.jpg", 1, "Movies/Interstellar/interstellar-search-image.jpg", "Movies/Interstellar/interstellar-trailer.mp4", "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.", "Movies/Interstellar/interstellar-video-image.jpg", new TimeSpan(0, 2, 50, 0, 0), "Interstellar", "Movies/Interstellar/interstellar-video-720p.mp4", 2014 }
                });

            migrationBuilder.InsertData(
                table: "Serials",
                columns: new[] { "Name", "Age", "Description", "ImageUrl", "ProducerId", "SearchImageUrl", "TrailerUrl", "Year" },
                values: new object[] { "The Last of Us", 16, "After a global pandemic destroys civilization, a hardened survivor takes charge of a 14-year-old girl, who may be humanity's last hope.", "Serials/The Last of Us/the-last-of-us-image.jpg", 2, "Serials/The Last of Us/the-last-of-us-search-image.jpg", "Serials/The Last of Us/the-last-of-us-trailer.mp4", 2023 });

            migrationBuilder.InsertData(
                table: "SoonGenres",
                columns: new[] { "GenreId", "SoonName" },
                values: new object[,]
                {
                    { 2, "Guardians of the Galaxy Vol. 3" },
                    { 3, "Guardians of the Galaxy Vol. 3" },
                    { 4, "Guardians of the Galaxy Vol. 3" },
                    { 8, "Guardians of the Galaxy Vol. 3" },
                    { 4, "John Wick: Chapter 4" },
                    { 6, "John Wick: Chapter 4" },
                    { 7, "John Wick: Chapter 4" },
                    { 2, "Transformers: Rise of the Beasts" },
                    { 3, "Transformers: Rise of the Beasts" },
                    { 4, "Transformers: Rise of the Beasts" }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieName" },
                values: new object[,]
                {
                    { 7, "Inception" },
                    { 8, "Inception" },
                    { 9, "Inception" },
                    { 1, "Interstellar" },
                    { 2, "Interstellar" },
                    { 3, "Interstellar" }
                });

            migrationBuilder.InsertData(
                table: "MovieGenres",
                columns: new[] { "GenreId", "MovieName" },
                values: new object[,]
                {
                    { 2, "Inception" },
                    { 3, "Inception" },
                    { 4, "Inception" },
                    { 6, "Inception" },
                    { 1, "Interstellar" },
                    { 2, "Interstellar" },
                    { 3, "Interstellar" }
                });

            migrationBuilder.InsertData(
                table: "MovieMarks",
                columns: new[] { "MovieName", "UserId" },
                values: new object[] { "Interstellar", 1 });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "Number", "SerialName" },
                values: new object[] { 1, 1, "The Last of Us" });

            migrationBuilder.InsertData(
                table: "SerialActors",
                columns: new[] { "ActorId", "SerialName" },
                values: new object[,]
                {
                    { 4, "The Last of Us" },
                    { 5, "The Last of Us" },
                    { 6, "The Last of Us" }
                });

            migrationBuilder.InsertData(
                table: "SerialGenres",
                columns: new[] { "GenreId", "SerialName" },
                values: new object[,]
                {
                    { 1, "The Last of Us" },
                    { 2, "The Last of Us" },
                    { 3, "The Last of Us" },
                    { 4, "The Last of Us" },
                    { 5, "The Last of Us" },
                    { 6, "The Last of Us" }
                });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoLength", "VideoUrl" },
                values: new object[] { 1, "Twenty years after a fungal outbreak ravages the planet, survivors Joel and Ellie are tasked with a mission that could change everything.", "Serials/The Last of Us/Season 1/Episode 1/the-last-of-us-S01E01-video-image.jpg", "1. When You're Lost in the Darkness", 1, 1, new TimeSpan(0, 1, 20, 0, 0), "Serials/The Last of Us/Season 1/Episode 1/the-last-of-us-S01E01-video-720p.mkv" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoLength", "VideoUrl" },
                values: new object[] { 2, "Joel, Tess, and Ellie traverse through an abandoned and flooded Boston hotel on their way to drop Ellie off with a group of Fireflies.", "Serials/The Last of Us/Season 1/Episode 2/the-last-of-us-S01E02-video-image.jpg", "2. Infected", 2, 1, new TimeSpan(0, 0, 52, 0, 0), "Serials/The Last of Us/Season 1/Episode 2/the-last-of-us-S01E02-video-720p.mkv" });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeasonId",
                table: "Episodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_ActorId",
                table: "MovieActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieMarks_UserId",
                table: "MovieMarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ProducerId",
                table: "Movies",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_SerialName",
                table: "Seasons",
                column: "SerialName");

            migrationBuilder.CreateIndex(
                name: "IX_SerialActors_ActorId",
                table: "SerialActors",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialGenres_GenreId",
                table: "SerialGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialMarks_UserId",
                table: "SerialMarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Serials_ProducerId",
                table: "Serials",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_SoonGenres_GenreId",
                table: "SoonGenres",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "MovieActors");

            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "MovieMarks");

            migrationBuilder.DropTable(
                name: "SerialActors");

            migrationBuilder.DropTable(
                name: "SerialGenres");

            migrationBuilder.DropTable(
                name: "SerialMarks");

            migrationBuilder.DropTable(
                name: "SoonGenres");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Actors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Soons");

            migrationBuilder.DropTable(
                name: "Serials");

            migrationBuilder.DropTable(
                name: "Producers");
        }
    }
}
