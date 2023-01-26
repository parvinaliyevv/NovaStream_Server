using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaStream.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
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
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                table: "Movies",
                columns: new[] { "Name", "Description", "ImagePath", "VideoPath", "Year" },
                values: new object[] { "Interstellar", "With humanity teetering on the brink of extinction, a group of astronauts travels through a wormhole in search of another inhabitable planet.", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FInterstellar%2FInterstellar.png?alt=media&token=0a6c45f5-fc92-4b66-8d50-7222ae8815b8", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FMovies%2FInterstellar%2FInterstellar.mp4?alt=media&token=0fbe924c-8746-4132-8440-f8331d5214f6", 2014 });

            migrationBuilder.InsertData(
                table: "Serials",
                columns: new[] { "Name", "Description", "ImagePath", "Year" },
                values: new object[] { "Peaky Blinders", "A notorious gang in 1919 Birmingham, England, is led by the fierce Tommy Shelby, a crime boss set on moving up in the world no matter the cost.", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders.jpg?alt=media&token=aef25bbd-482b-49df-b286-b5ba1a1740d0", 2013 });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "Number", "SerialName" },
                values: new object[] { 1, 1, "Peaky Blinders" });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "Number", "SerialName" },
                values: new object[] { 2, 2, "Peaky Blinders" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImagePath", "Name", "Number", "SeasonId", "VideoPath" },
                values: new object[] { 1, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 1", 1, 1, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E01.mp4?alt=media&token=728dbcd3-6215-4e76-ae69-6849ba9896f5" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImagePath", "Name", "Number", "SeasonId", "VideoPath" },
                values: new object[] { 2, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 2", 2, 1, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%201%2FPeaky-Blinders-S01E02.mp4?alt=media&token=f2bcb626-2a22-4a69-bdb8-69fef93ca2c9" });

            migrationBuilder.InsertData(
                table: "Episodes",
                columns: new[] { "Id", "Description", "ImagePath", "Name", "Number", "SeasonId", "VideoPath" },
                values: new object[] { 3, "Baza", "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Images%2FPeaky%20Blinders%2FPeaky%20Blinders.jpg?alt=media&token=872defbb-acbc-4e8f-8a02-f61a27ff3988", "Episode 3", 1, 2, "https://firebasestorage.googleapis.com/v0/b/neftlixtestapi.appspot.com/o/Videos%2FSerials%2FPeaky%20Blinders%2FSeason%202%2FPeaky-Blinders-S02E01.mp4?alt=media&token=740e66cd-9759-4b7b-8592-8a93919a059b" });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SeasonId",
                table: "Episodes",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_SerialName",
                table: "Seasons",
                column: "SerialName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Serials");
        }
    }
}
