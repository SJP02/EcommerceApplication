using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_CompanyName",
                table: "Companys",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_Companys_Location",
                table: "Companys",
                column: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Price",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Companys_CompanyName",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_Companys_Location",
                table: "Companys");
        }
    }
}
