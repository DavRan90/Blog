using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedColorsAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuLink",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "NumberOfTitles",
                table: "Elements");

            migrationBuilder.AddColumn<int>(
                name: "BackgroundColor",
                table: "Sites",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BackgroundColorString",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BackgroundColor",
                table: "Elements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "BackgroundColorString",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Elements");

            migrationBuilder.AddColumn<string>(
                name: "MenuLink",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTitles",
                table: "Elements",
                type: "int",
                nullable: true);
        }
    }
}
