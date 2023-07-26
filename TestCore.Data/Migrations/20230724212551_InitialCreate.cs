using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestCore.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PricePerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Document = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReturnLogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnProcessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Recharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rents_Cars_CarID",
                        column: x => x.CarID,
                        principalTable: "Cars",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rents_CarID",
                table: "Rents",
                column: "CarID");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_CustomerID",
                table: "Rents",
                column: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
