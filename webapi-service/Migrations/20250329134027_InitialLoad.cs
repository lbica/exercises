using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialLoad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Customer_Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    InsertedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    InsertedUser = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedUser = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Customer_Id);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Customer_Id", "Country", "CustomerId", "InsertedDate", "InsertedUser", "UpdatedDate", "UpdatedUser" },
                values: new object[,]
                {
                    { -2, "Unknown", "Unknown", new DateTime(2025, 3, 29, 14, 40, 27, 801, DateTimeKind.Local).AddTicks(2228), "SYSTEM", new DateTime(2025, 3, 29, 14, 40, 27, 801, DateTimeKind.Local).AddTicks(2229), "SYSTEM" },
                    { -1, "Undefined", "Undefined", new DateTime(2025, 3, 29, 14, 40, 27, 801, DateTimeKind.Local).AddTicks(2160), "SYSTEM", new DateTime(2025, 3, 29, 14, 40, 27, 801, DateTimeKind.Local).AddTicks(2204), "SYSTEM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerId",
                table: "Customers",
                column: "CustomerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
