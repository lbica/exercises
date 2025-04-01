using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dim_customers",
                columns: table => new
                {
                    customer_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dim_customers", x => x.customer_key);
                });

            migrationBuilder.InsertData(
                table: "dim_customers",
                columns: new[] { "customer_key", "country", "customer_id" },
                values: new object[,]
                {
                    { -2, "Unknown", "Unknown" },
                    { -1, "Undefined", "Undefined" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_dim_customers_customer_id",
                table: "dim_customers",
                column: "customer_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dim_customers");
        }
    }
}
