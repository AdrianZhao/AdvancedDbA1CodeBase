using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    public partial class SeedDataUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopQuantities_StoreLocations_StoreLocationStoreNumber",
                table: "LaptopQuantities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaptopQuantities",
                table: "LaptopQuantities");

            migrationBuilder.DropColumn(
                name: "StoreNumber",
                table: "LaptopQuantities");

            migrationBuilder.RenameColumn(
                name: "StoreNumber",
                table: "StoreLocations",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "StoreLocationStoreNumber",
                table: "LaptopQuantities",
                newName: "StoreLocationNumber");

            migrationBuilder.RenameIndex(
                name: "IX_LaptopQuantities_StoreLocationStoreNumber",
                table: "LaptopQuantities",
                newName: "IX_LaptopQuantities_StoreLocationNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaptopQuantities",
                table: "LaptopQuantities",
                columns: new[] { "LaptopNumber", "StoreLocationNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopQuantities_StoreLocations_StoreLocationNumber",
                table: "LaptopQuantities",
                column: "StoreLocationNumber",
                principalTable: "StoreLocations",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaptopQuantities_StoreLocations_StoreLocationNumber",
                table: "LaptopQuantities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaptopQuantities",
                table: "LaptopQuantities");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "StoreLocations",
                newName: "StoreNumber");

            migrationBuilder.RenameColumn(
                name: "StoreLocationNumber",
                table: "LaptopQuantities",
                newName: "StoreLocationStoreNumber");

            migrationBuilder.RenameIndex(
                name: "IX_LaptopQuantities_StoreLocationNumber",
                table: "LaptopQuantities",
                newName: "IX_LaptopQuantities_StoreLocationStoreNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreNumber",
                table: "LaptopQuantities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaptopQuantities",
                table: "LaptopQuantities",
                columns: new[] { "LaptopNumber", "StoreNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_LaptopQuantities_StoreLocations_StoreLocationStoreNumber",
                table: "LaptopQuantities",
                column: "StoreLocationStoreNumber",
                principalTable: "StoreLocations",
                principalColumn: "StoreNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
