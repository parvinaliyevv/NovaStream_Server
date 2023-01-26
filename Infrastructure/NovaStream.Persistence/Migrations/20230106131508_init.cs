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
                VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                Age = table.Column<int>(type: "int", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TrailerUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Serials", x => x.Name);
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
                { 6, "Detective" }
            });

        migrationBuilder.InsertData(
            table: "Movies",
            columns: new[] { "Name", "Age", "Description", "ImageUrl", "TrailerUrl", "VideoUrl", "Year" },
            values: new object[] { "Interstellar", 13, "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FInterstellar%2FInterstellar.png?alt=media&token=0a6c45f5-fc92-4b66-8d50-7222ae8815b8", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar%20Trailer.mp4?alt=media&token=562d9a36-a3b2-411b-9525-05ccdc65e11a", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar.mp4?alt=media&token=0fbe924c-8746-4132-8440-f8331d5214f6", 2014 });

        migrationBuilder.InsertData(
            table: "Serials",
            columns: new[] { "Name", "Age", "Description", "ImageUrl", "TrailerUrl", "Year" },
            values: new object[] { "Peaky Blinders", 18, "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky%20Blinder%20Season%201%20Trailer.mp4?alt=media&token=ce9640c0-b5ea-4f6f-9d1d-11b5a518ab32", 2013 });

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
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 1, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 1", 1, 1, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E01.mp4?alt=media&token=728dbcd3-6215-4e76-ae69-6849ba9896f5" });

        migrationBuilder.InsertData(
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 2, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 2", 2, 1, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E02.mp4?alt=media&token=f2bcb626-2a22-4a69-bdb8-69fef93ca2c9" });

        migrationBuilder.InsertData(
            table: "Episodes",
            columns: new[] { "Id", "Description", "ImageUrl", "Name", "Number", "SeasonId", "VideoUrl" },
            values: new object[] { 3, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 1", 1, 2, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%202%2FPeaky-Blinders-S02E01.mp4?alt=media&token=740e66cd-9759-4b7b-8592-8a93919a059b" });

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
            name: "Seasons");

        migrationBuilder.DropTable(
            name: "Movies");

        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Serials");
    }
}
