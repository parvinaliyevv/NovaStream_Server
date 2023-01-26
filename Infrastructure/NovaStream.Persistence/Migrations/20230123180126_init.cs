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
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    { 1, "Yaxshi Oglan", "baza", "Cillian", "Murphy" },
                    { 2, "Babat Oglan", "baza", "Tom", "Cruse" },
                    { 3, "Zor Oglan", "baza", "Brad", "Pitt" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "baza", "Drama" },
                    { 2, "baza", "Crime" },
                    { 3, "baza", "Historical" },
                    { 4, "baza", "Adventure" },
                    { 5, "baza", "Science fiction" },
                    { 6, "baza", "Detective" },
                    { 7, "baza", "Action" },
                    { 8, "baza", "Thriller" },
                    { 9, "baza", "Comedy horror" },
                    { 10, "baza", "Coming-of-age" },
                    { 11, "baza", "Supernatural" },
                    { 12, "baza", "Comedy" },
                    { 13, "baza", "Sci-Fi" }
                });

            migrationBuilder.InsertData(
                table: "Producers",
                columns: new[] { "Id", "About", "ImageUrl", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, "zor oglan", "baza", "Murad", "Musayev" },
                    { 2, "zor oglan", "baza", "Parvin", "Aliyev" },
                    { 3, "zor oglan", "baza", "Rustem", "Bayramov" }
                });

            migrationBuilder.InsertData(
                table: "Soons",
                columns: new[] { "Name", "Description", "OutDate", "TrailerImageUrl", "TrailerUrl" },
                values: new object[,]
                {
                    { "Guardians of the Galaxy Vol. 3", "Still reeling from the loss of Gamora, Peter Quill rallies his team to defend the universe and one of their own - a mission that could mean the end of the Guardians if not successful.", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://novastreamstorage.blob.core.windows.net/root/Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer-image", "https://novastreamstorage.blob.core.windows.net/root/Soons/Guardians of the Galaxy Vol. 3/guardians-of-the-galaxy-vol-3-trailer" },
                    { "John Wick: Chapter 4", "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.", new DateTime(2023, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://novastreamstorage.blob.core.windows.net/root/Soons/John Wick Chapter 4/john-wick-chapter-4-trailer-image", "https://novastreamstorage.blob.core.windows.net/root/Soons/John Wick Chapter 4/john-wick-chapter-4-trailer" },
                    { "Transformers: Rise of the Beasts", "Plot unknown. Reportedly based on the 'Transformers' spinoff 'Beast Wars' which feature robots that transform into robotic animals.", new DateTime(2023, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://novastreamstorage.blob.core.windows.net/root/Soons/Transformers Rise of the Beasts/transformers-rise-of-the-beasts-trailer-image", "https://novastreamstorage.blob.core.windows.net/root/Soons/Transformers Rise of the Beasts/transformers-rise-of-the-beasts-trailer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "Email", "Nickname", "PasswordHash" },
                values: new object[] { 1, "https://novastreamstorage.blob.core.windows.net/root/Avatars/avatar-1", "admin@novastream.api", "Admin", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Name", "Age", "Description", "ImageUrl", "ProducerId", "SearchImageUrl", "TrailerUrl", "VideoDescription", "VideoImageUrl", "VideoName", "VideoUrl", "Year" },
                values: new object[] { "Interstellar", 13, "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.", "https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-image", 2, "https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-search-image", "https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-trailer", "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.", "https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-video-image", "Episode", "https://novastreamstorage.blob.core.windows.net/root/Movies/Interstellar/interstellar-video", 2014 });

            migrationBuilder.InsertData(
                table: "Serials",
                columns: new[] { "Name", "Age", "Description", "ImageUrl", "ProducerId", "SearchImageUrl", "TrailerUrl", "Year" },
                values: new object[,]
                {
                    { "Peaky Blinders", 18, "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-image", 1, "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-search-image", "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/peaky-blinders-trailer", 2013 },
                    { "Wednesday", 14, "Follows Wednesday Addams' years as a student, when she attempts to master her emerging psychic ability, thwart and solve the mystery that embroiled her parents.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-image", 3, "https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-search-image", "https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/wednesday-trailer", 2022 }
                });

            migrationBuilder.InsertData(
                table: "SoonGenres",
                columns: new[] { "GenreId", "SoonName" },
                values: new object[,]
                {
                    { 4, "Guardians of the Galaxy Vol. 3" },
                    { 7, "Guardians of the Galaxy Vol. 3" },
                    { 12, "Guardians of the Galaxy Vol. 3" },
                    { 2, "John Wick: Chapter 4" },
                    { 7, "John Wick: Chapter 4" },
                    { 8, "John Wick: Chapter 4" },
                    { 4, "Transformers: Rise of the Beasts" },
                    { 7, "Transformers: Rise of the Beasts" },
                    { 13, "Transformers: Rise of the Beasts" }
                });

            migrationBuilder.InsertData(
                table: "MovieActors",
                columns: new[] { "ActorId", "MovieName" },
                values: new object[,]
                {
                    { 2, "Interstellar" },
                    { 3, "Interstellar" }
                });

            migrationBuilder.InsertData(
                table: "MovieGenres",
                columns: new[] { "GenreId", "MovieName" },
                values: new object[,]
                {
                    { 1, "Interstellar" },
                    { 4, "Interstellar" },
                    { 5, "Interstellar" },
                    { 6, "Interstellar" }
                });

            migrationBuilder.InsertData(
                table: "MovieMarks",
                columns: new[] { "MovieName", "UserId" },
                values: new object[] { "Interstellar", 1 });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "Number", "SerialName" },
                values: new object[,]
                {
                    { 1, 1, "Peaky Blinders" },
                    { 2, 2, "Peaky Blinders" },
                    { 3, 1, "Wednesday" }
                });

            migrationBuilder.InsertData(
                table: "SerialActors",
                columns: new[] { "ActorId", "SerialName" },
                values: new object[,]
                {
                    { 1, "Peaky Blinders" },
                    { 2, "Peaky Blinders" }
                });

            migrationBuilder.InsertData(
                table: "SerialGenres",
                columns: new[] { "GenreId", "SerialName" },
                values: new object[,]
                {
                    { 1, "Peaky Blinders" },
                    { 2, "Peaky Blinders" },
                    { 3, "Peaky Blinders" },
                    { 9, "Wednesday" },
                    { 10, "Wednesday" },
                    { 11, "Wednesday" }
                });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
                values: new object[,]
                {
                    { 1, "Ambitious gang leader Thomas Shelby recognizes an opportunity to move up in the world thanks to a missing crate of guns.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 1/peaky-blinders-S01E01-video-image", "1. Episode 1", 1, 1, "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 1/peaky-blinders-S01E01-video.mp4" },
                    { 2, "Thomas provokes a local kingpin by fixing a horse race and starts a war with a gypsy family; Inspector Campbell carries out a vicious raid.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 2/peaky-blinders-S01E02-video-image", "2. Episode 2", 2, 1, "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 1/Episode 2/peaky-blinders-S01E02-video.mp4" },
                    { 3, "When Birmingham crime boss Tommy Shelby's beloved Garrison pub is bombed, he finds himself blackmailed into murdering an Irish dissident.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 2/Episode 1/peaky-blinders-S02E01-video-image", "1. Episode 1", 1, 2, "https://novastreamstorage.blob.core.windows.net/root/Serials/Peaky Blinders/Season 2/Episode 1/peaky-blinders-S02E01-video.mp4" },
                    { 4, "When a deliciously wicked prank gets Wednesday expelled, her parents ship her off to Nevermore Academy, the boarding school where they fell in love.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/Season 1/Episode 1/wednesday-S01E01-video-image", "1. Wednesday's Child Is Full of Woe", 1, 3, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E01-video.mkv?alt=media&token=ac3169c1-7e09-46f4-baa9-cad6e290244b" },
                    { 5, "The sheriff questions Wednesday about the night's strange happenings. Later, Wednesday faces off against a fierce rival in the cutthroat Poe Cup race.", "https://novastreamstorage.blob.core.windows.net/root/Serials/Wednesday/Season 1/Episode 2/wednesday-S01E02-video-image", "2. Woe Is the Loneliest Number", 2, 3, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FWednesday%2FSeason%201%2Fwednesday-S01E02-video.mkv?alt=media&token=3fdc19c2-224e-4c06-afda-c7d5fd1e2db1" }
                });

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
