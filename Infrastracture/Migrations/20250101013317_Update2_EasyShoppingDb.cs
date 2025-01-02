using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class Update2_EasyShoppingDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductThumbnail_Products_ProductId",
                table: "ProductThumbnail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductThumbnail",
                table: "ProductThumbnail");

            migrationBuilder.RenameTable(
                name: "ProductThumbnail",
                newName: "ProductThumbnails");

            migrationBuilder.RenameIndex(
                name: "IX_ProductThumbnail_ProductId",
                table: "ProductThumbnails",
                newName: "IX_ProductThumbnails_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductThumbnails",
                table: "ProductThumbnails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductThumbnails_Products_ProductId",
                table: "ProductThumbnails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductThumbnails_Products_ProductId",
                table: "ProductThumbnails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductThumbnails",
                table: "ProductThumbnails");

            migrationBuilder.RenameTable(
                name: "ProductThumbnails",
                newName: "ProductThumbnail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductThumbnails_ProductId",
                table: "ProductThumbnail",
                newName: "IX_ProductThumbnail_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductThumbnail",
                table: "ProductThumbnail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductThumbnail_Products_ProductId",
                table: "ProductThumbnail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
