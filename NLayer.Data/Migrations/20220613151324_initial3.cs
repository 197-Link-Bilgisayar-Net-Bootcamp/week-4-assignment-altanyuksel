using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NLayer.Data.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_productFeatures_ProductFeatureId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productFeatures",
                table: "productFeatures");

            migrationBuilder.RenameTable(
                name: "productFeatures",
                newName: "ProductFeatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductFeatures_ProductFeatureId",
                table: "Products",
                column: "ProductFeatureId",
                principalTable: "ProductFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductFeatures_ProductFeatureId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures");

            migrationBuilder.RenameTable(
                name: "ProductFeatures",
                newName: "productFeatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productFeatures",
                table: "productFeatures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_productFeatures_ProductFeatureId",
                table: "Products",
                column: "ProductFeatureId",
                principalTable: "productFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
