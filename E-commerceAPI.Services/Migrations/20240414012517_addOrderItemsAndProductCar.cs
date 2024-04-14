using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerceAPI.Services.Migrations
{
    public partial class addOrderItemsAndProductCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Carts_Carts_CartID",
                table: "Products_Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Carts_Products_ProductID",
                table: "Products_Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_Carts",
                table: "Products_Carts");

            migrationBuilder.RenameTable(
                name: "Products_Carts",
                newName: "ProductsCarts");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Carts_ProductID",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Carts_CartID",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_CartID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsCarts",
                table: "ProductsCarts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Carts_CartID",
                table: "ProductsCarts",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Products_ProductID",
                table: "ProductsCarts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Carts_CartID",
                table: "ProductsCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Products_ProductID",
                table: "ProductsCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsCarts",
                table: "ProductsCarts");

            migrationBuilder.RenameTable(
                name: "ProductsCarts",
                newName: "Products_Carts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_ProductID",
                table: "Products_Carts",
                newName: "IX_Products_Carts_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_CartID",
                table: "Products_Carts",
                newName: "IX_Products_Carts_CartID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_Carts",
                table: "Products_Carts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Carts_Carts_CartID",
                table: "Products_Carts",
                column: "CartID",
                principalTable: "Carts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Carts_Products_ProductID",
                table: "Products_Carts",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
