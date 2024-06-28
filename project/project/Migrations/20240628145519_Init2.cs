using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "contract",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start", "UpgradesEnd" },
                values: new object[] { new DateTime(2024, 6, 28, 16, 55, 18, 497, DateTimeKind.Local).AddTicks(9498), new DateTime(2024, 6, 28, 16, 55, 18, 497, DateTimeKind.Local).AddTicks(9487), new DateTime(2025, 6, 28, 16, 55, 18, 497, DateTimeKind.Local).AddTicks(9505) });

            migrationBuilder.UpdateData(
                table: "discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2024, 6, 28, 16, 55, 18, 497, DateTimeKind.Local).AddTicks(9383), new DateTime(2024, 6, 28, 16, 55, 18, 497, DateTimeKind.Local).AddTicks(9307) });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "admin", "admin", "admin" },
                    { 2, "normal", "normal", "normal" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.UpdateData(
                table: "contract",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start", "UpgradesEnd" },
                values: new object[] { new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4750), new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4745), new DateTime(2025, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4754) });

            migrationBuilder.UpdateData(
                table: "discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "End", "Start" },
                values: new object[] { new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4695), new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4640) });
        }
    }
}
