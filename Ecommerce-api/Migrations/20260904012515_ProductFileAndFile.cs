using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_api.Migrations
{
    /// <inheritdoc />
    public partial class ProductFileAndFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_products_ProductId",
                table: "files");

            migrationBuilder.DropIndex(
                name: "IX_files_ProductId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "files");

            migrationBuilder.DropColumn(
                name: "StorageUrl",
                table: "files");

            migrationBuilder.CreateTable(
                name: "product_files",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_files", x => new { x.ProductId, x.FileId });
                    table.ForeignKey(
                        name: "FK_product_files_files_FileId",
                        column: x => x.FileId,
                        principalTable: "files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_files_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_files_FileId",
                table: "product_files",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_files");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StorageUrl",
                table: "files",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_files_ProductId",
                table: "files",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_files_products_ProductId",
                table: "files",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
