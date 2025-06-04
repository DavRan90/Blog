using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Elements");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Elements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Elements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements",
                column: "SiteId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Elements");

            migrationBuilder.AlterColumn<int>(
                name: "SiteId",
                table: "Elements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Elements",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements",
                column: "SiteId",
                principalTable: "Pages",
                principalColumn: "Id");
        }
    }
}
