using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace project.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    KRS = table.Column<int>(type: "int", nullable: true),
                    PESEL = table.Column<int>(type: "int", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "software",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdSoftware = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_discount_software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_version",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IdSoftware = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_version", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_version_software_IdSoftware",
                        column: x => x.IdSoftware,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpgradesEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Signed = table.Column<bool>(type: "bit", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdSoftwareVersion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contract_client_IdClient",
                        column: x => x.IdClient,
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contract_software_version_IdSoftwareVersion",
                        column: x => x.IdSoftwareVersion,
                        principalTable: "software_version",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<double>(type: "float", nullable: false),
                    IdContract = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payment_contract_IdContract",
                        column: x => x.IdContract,
                        principalTable: "contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "client",
                columns: new[] { "Id", "Address", "IsDeleted", "KRS", "LastName", "Mail", "Name", "PESEL", "Phone" },
                values: new object[,]
                {
                    { 1, "adr1", false, 12345, "", "@", "Dom", null, 123 },
                    { 2, "adr2", false, null, "Smith", "@@", "Ala", 1234567, 234 }
                });

            migrationBuilder.InsertData(
                table: "software",
                columns: new[] { "Id", "Category", "Cost", "Description", "Name" },
                values: new object[] { 1, "1", 200.0, "abc", "Windows" });

            migrationBuilder.InsertData(
                table: "discount",
                columns: new[] { "Id", "End", "IdSoftware", "Name", "Start", "Value" },
                values: new object[] { 1, new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4695), 1, "this", new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4640), 200.0 });

            migrationBuilder.InsertData(
                table: "software_version",
                columns: new[] { "Id", "IdSoftware", "Version" },
                values: new object[] { 1, 1, "10a" });

            migrationBuilder.InsertData(
                table: "contract",
                columns: new[] { "Id", "End", "IdClient", "IdSoftwareVersion", "Price", "Signed", "Start", "UpgradesEnd" },
                values: new object[] { 1, new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4750), 1, 1, 200.0, true, new DateTime(2024, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4745), new DateTime(2025, 6, 26, 18, 38, 34, 575, DateTimeKind.Local).AddTicks(4754) });

            migrationBuilder.InsertData(
                table: "payment",
                columns: new[] { "Id", "IdContract", "Value" },
                values: new object[] { 1, 1, 200.0 });

            migrationBuilder.CreateIndex(
                name: "IX_contract_IdClient",
                table: "contract",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_contract_IdSoftwareVersion",
                table: "contract",
                column: "IdSoftwareVersion");

            migrationBuilder.CreateIndex(
                name: "IX_discount_IdSoftware",
                table: "discount",
                column: "IdSoftware");

            migrationBuilder.CreateIndex(
                name: "IX_payment_IdContract",
                table: "payment",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_software_version_IdSoftware",
                table: "software_version",
                column: "IdSoftware");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discount");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "contract");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "software_version");

            migrationBuilder.DropTable(
                name: "software");
        }
    }
}
