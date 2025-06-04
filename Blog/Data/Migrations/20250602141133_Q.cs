using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Data.Migrations
{
    /// <inheritdoc />
    public partial class Q : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Pages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Elements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Elements_SiteId",
                table: "Elements",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements",
                column: "SiteId",
                principalTable: "Pages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Pages_SiteId",
                table: "Elements");

            migrationBuilder.DropIndex(
                name: "IX_Elements_SiteId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Elements");
        }
    }
}
