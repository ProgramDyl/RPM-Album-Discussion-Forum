using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicForum.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForHire",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "AspNetUsers",
                newName: "Location");

            migrationBuilder.AddColumn<string>(
                name: "FavouriteAlbum",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageFilename",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FavouriteAlbum",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageFilename",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "Bio");

            migrationBuilder.AddColumn<bool>(
                name: "IsForHire",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
