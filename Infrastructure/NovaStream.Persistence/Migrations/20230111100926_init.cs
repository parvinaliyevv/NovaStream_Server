using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaStream.Persistence.Migrations;

public partial class init : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Movies",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                VideoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                VideoDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                VideoImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                Age = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SearchImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Movies", x => x.Name);
            });

        migrationBuilder.CreateTable(
            name: "Serials",
            columns: table => new
            {
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                Age = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                SearchImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Serials", x => x.Name);
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
                Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Email);
            });

        migrationBuilder.CreateTable(
            name: "MovieCategories",
            columns: table => new
            {
                MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MovieCategories", x => new { x.MovieName, x.CategoryId });
                table.ForeignKey(
                    name: "FK_MovieCategories_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MovieCategories_Movies_MovieName",
                    column: x => x.MovieName,
                    principalTable: "Movies",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Seasons",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Number = table.Column<int>(type: "int", nullable: false),
                SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
            name: "SerialCategories",
            columns: table => new
            {
                SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SerialCategories", x => new { x.SerialName, x.CategoryId });
                table.ForeignKey(
                    name: "FK_SerialCategories_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SerialCategories_Serials_SerialName",
                    column: x => x.SerialName,
                    principalTable: "Serials",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SoonCategories",
            columns: table => new
            {
                SoonName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SoonCategories", x => new { x.SoonName, x.CategoryId });
                table.ForeignKey(
                    name: "FK_SoonCategories_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SoonCategories_Soons_SoonName",
                    column: x => x.SoonName,
                    principalTable: "Soons",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "MovieMarks",
            columns: table => new
            {
                MovieName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MovieMarks", x => new { x.MovieName, x.UserEmail });
                table.ForeignKey(
                    name: "FK_MovieMarks_Movies_MovieName",
                    column: x => x.MovieName,
                    principalTable: "Movies",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MovieMarks_Users_UserEmail",
                    column: x => x.UserEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SerialMarks",
            columns: table => new
            {
                SerialName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SerialMarks", x => new { x.SerialName, x.UserEmail });
                table.ForeignKey(
                    name: "FK_SerialMarks_Serials_SerialName",
                    column: x => x.SerialName,
                    principalTable: "Serials",
                    principalColumn: "Name",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SerialMarks_Users_UserEmail",
                    column: x => x.UserEmail,
                    principalTable: "Users",
                    principalColumn: "Email",
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
                SeasonId = table.Column<int>(type: "int", nullable: false)
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
            table: "Categories",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                { 1, "Drama" },
                { 2, "Crime" },
                { 3, "Historical" },
                { 4, "Adventure" },
                { 5, "Science fiction" },
                { 6, "Detective" },
                { 7, "Action" },
                { 8, "Thriller" }
            });

        migrationBuilder.InsertData(
            table: "Movies",
            columns: new[] { "Name", "Age", "Description", "ImageUrl", "SearchImageUrl", "TrailerImageUrl", "TrailerUrl", "VideoDescription", "VideoImageUrl", "VideoName", "VideoUrl", "Year" },
            values: new object[] { "Interstellar", 13, "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival. Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-image.png?alt=media&token=776eca25-eae0-426e-b103-8ed2c47a0811", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-search-image.jpg?alt=media&token=5799d5f0-f87d-424c-9c62-b0ba5904f499", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-trailer-image.jpg?alt=media&token=1da9a664-0a92-4e35-90d9-7905fd33dfdf", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-trailer.mp4?alt=media&token=3e5984d4-5fb8-438b-aa9a-3778e3f52ba8", "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-video-image.jpg?alt=media&token=5a18e02b-3976-4116-a591-d6e334c50772", "Episode", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Movies%2FInterstellar%2Finterstellar-video.mp4?alt=media&token=aa6a5dca-7570-45de-87ed-22d35d25189b", 2014 });

        migrationBuilder.InsertData(
            table: "Serials",
            columns: new[] { "Name", "Age", "Description", "ImageUrl", "SearchImageUrl", "TrailerImageUrl", "TrailerUrl", "Year" },
            values: new object[] { "Peaky Blinders", 18, "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-image.jpg?alt=media&token=356b23bd-755e-4daf-822e-50a029c87f9c", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-search-image.jpg?alt=media&token=8ea5abb6-b969-4bf4-a20a-13ffcd3a07fd", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-trailer-image.jpg?alt=media&token=a99966d3-1793-4cac-97fc-80b9b75686f0", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2Fpeaky-blinders-trailer.mp4?alt=media&token=c5e7aef9-cfcf-4a31-8e77-8c678d95bd7b", 2013 });

        migrationBuilder.InsertData(
            table: "Soons",
            columns: new[] { "Name", "Description", "OutDate", "TrailerImageUrl", "TrailerUrl" },
            values: new object[] { "John Wick: Chapter 4", "John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, Wick must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.", new DateTime(2023, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Soons%2FJohn%20Wick%3A%20Chapter%204%2Fjohn-wick-chapter-4-trailer-image.jpg?alt=media&token=410d8c6b-fe9c-4da4-a654-1980eb72c78e", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Soons%2FJohn%20Wick%3A%20Chapter%204%2Fjohn-wick-chapter-4-trailer.mp4?alt=media&token=2c1e4eea-4a4f-4e00-add3-eb10b9dd5030" });

        migrationBuilder.InsertData(
            table: "Users",
            columns: new[] { "Email", "Id", "PasswordHash" },
            values: new object[] { "admin@novastream.api", 1, "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918" });

        migrationBuilder.InsertData(
            table: "MovieCategories",
            columns: new[] { "CategoryId", "MovieName" },
            values: new object[,]
            {
                { 1, "Interstellar" },
                { 4, "Interstellar" },
                { 5, "Interstellar" },
                { 6, "Interstellar" }
            });

        migrationBuilder.InsertData(
            table: "MovieMarks",
            columns: new[] { "MovieName", "UserEmail" },
            values: new object[] { "Interstellar", "admin@novastream.api" });

        migrationBuilder.InsertData(
            table: "Seasons",
            columns: new[] { "Id", "Number", "SerialName" },
            values: new object[,]
            {
                { 1, 1, "Peaky Blinders" },
                { 2, 2, "Peaky Blinders" }
            });

        migrationBuilder.InsertData(
            table: "SerialCategories",
            columns: new[] { "CategoryId", "SerialName" },
            values: new object[,]
            {
                { 1, "Peaky Blinders" },
                { 2, "Peaky Blinders" },
                { 3, "Peaky Blinders" }
            });

        migrationBuilder.InsertData(
            table: "SoonCategories",
            columns: new[] { "CategoryId", "SoonName" },
            values: new object[,]
            {
                { 2, "John Wick: Chapter 4" },
                { 7, "John Wick: Chapter 4" },
                { 8, "John Wick: Chapter 4" }
            });

        migrationBuilder.InsertData(
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 1, "Thomas Shelby, leader of the Birmingham gang, the Peaky Blinders, comes into possession of a shipment of guns from the local BSA factory. Aware that keeping the guns could lead to trouble with the law, Thomas nonetheless wants to use the guns to increase the Peaky's power.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-image.jpg?alt=media&token=10d57b41-f838-4110-acf7-63f8c01abebf", "1. Episode 1", 1, 1, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%201%2Fpeaky-blinders-S01E01-video.mp4?alt=media&token=1ddc4b67-579d-4081-ae49-aea3c95b9402" });

        migrationBuilder.InsertData(
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 2, "Thomas Shelby sets out to get work with Billy Kimber - the man who can help Thomas achieve his dream of running a legal bookmaking business. Meanwhile, Polly is alarmed when she realizes Ada is pregnant and when Thomas discovers the news, he forces Ada to admit that Freddie Thorne is the father.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-image.jpg?alt=media&token=7e07da2d-90b7-443e-b72a-088ebf61e2a3", "1. Episode 2", 2, 1, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%201%2FEpisode%202%2Fpeaky-blinders-S01E02-video.mp4?alt=media&token=c41cfa67-da90-4ee4-86af-7d5264617a69" });

        migrationBuilder.InsertData(
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 3, "As the 1920s begin to roar, business is booming for the Peaky Blinders gang. Tommy Shelby starts to expand his legal and illegal operations, with an eye on the racetracks of the south. Meanwhile, an enemy from Tommy's past returns to Birmingham.", "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-image.jpg?alt=media&token=acc1de8b-c971-41ee-9b0d-fff713ce484c", "2. Episode 1", 1, 2, "https://firebasestorage.googleapis.com/v0/b/novastream-a8167.appspot.com/o/Serials%2FPeaky%20Blinders%2FSeason%202%2FEpisode%201%2Fpeaky-blinders-S02E01-video.mp4?alt=media&token=1be97130-46ef-43d6-a195-c8e2e7d107d6" });

        migrationBuilder.CreateIndex(
            name: "IX_Episodes_SeasonId",
            table: "Episodes",
            column: "SeasonId");

        migrationBuilder.CreateIndex(
            name: "IX_MovieCategories_CategoryId",
            table: "MovieCategories",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_MovieMarks_UserEmail",
            table: "MovieMarks",
            column: "UserEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Seasons_SerialName",
            table: "Seasons",
            column: "SerialName");

        migrationBuilder.CreateIndex(
            name: "IX_SerialCategories_CategoryId",
            table: "SerialCategories",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_SerialMarks_UserEmail",
            table: "SerialMarks",
            column: "UserEmail");

        migrationBuilder.CreateIndex(
            name: "IX_SoonCategories_CategoryId",
            table: "SoonCategories",
            column: "CategoryId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Episodes");

        migrationBuilder.DropTable(
            name: "MovieCategories");

        migrationBuilder.DropTable(
            name: "MovieMarks");

        migrationBuilder.DropTable(
            name: "SerialCategories");

        migrationBuilder.DropTable(
            name: "SerialMarks");

        migrationBuilder.DropTable(
            name: "SoonCategories");

        migrationBuilder.DropTable(
            name: "Seasons");

        migrationBuilder.DropTable(
            name: "Movies");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Soons");

        migrationBuilder.DropTable(
            name: "Serials");
    }
}
