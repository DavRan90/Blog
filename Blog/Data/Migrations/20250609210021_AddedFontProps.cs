using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFontProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Elements");

            migrationBuilder.AddColumn<string>(
                name: "FontColorString",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FontFamilyString",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FontColorString",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "FontFamilyString",
                table: "Sites");

            migrationBuilder.AddColumn<int>(
                name: "BackgroundColor",
                table: "Elements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
