using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagesDetail",
                table: "ProductInfo",
                newName: "ImageDetail");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "ProductInfo",
                newName: "Image");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageDetail",
                table: "ProductInfo",
                newName: "ImagesDetail");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ProductInfo",
                newName: "Images");
        }
    }
}
