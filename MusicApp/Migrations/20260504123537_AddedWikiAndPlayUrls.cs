using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedWikiAndPlayUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayURL",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WikiURL",
                table: "Songs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayURL",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "WikiURL",
                table: "Songs");
        }
    }
}
