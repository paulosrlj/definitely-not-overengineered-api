using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Ecommerce_api.Migrations
{
    /// <inheritdoc />
    public partial class AddTsVectorToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "products",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector('portuguese', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_SearchVector",
                table: "products",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_SearchVector",
                table: "products");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "products");
        }
    }
}
