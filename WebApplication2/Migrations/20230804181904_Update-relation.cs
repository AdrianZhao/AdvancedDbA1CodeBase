using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class Updaterelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreLocations",
                columns: table => new
                {
                    StoreNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreLocations", x => x.StoreNumber);
                });

            migrationBuilder.CreateTable(
                name: "LaptopQuantities",
                columns: table => new
                {
                    LaptopNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreLocationStoreNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopQuantities", x => new { x.LaptopNumber, x.StoreNumber });
                    table.ForeignKey(
                        name: "FK_LaptopQuantities_Laptops_LaptopNumber",
                        column: x => x.LaptopNumber,
                        principalTable: "Laptops",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaptopQuantities_StoreLocations_StoreLocationStoreNumber",
                        column: x => x.StoreLocationStoreNumber,
                        principalTable: "StoreLocations",
                        principalColumn: "StoreNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LaptopQuantities_StoreLocationStoreNumber",
                table: "LaptopQuantities",
                column: "StoreLocationStoreNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaptopQuantities");

            migrationBuilder.DropTable(
                name: "StoreLocations");
        }
    }
}
