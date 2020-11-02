using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ProductInfo",
                newName: "ImagesDetail");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "ProductInfo",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ProductInfo",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "ProductInfo",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "ProductInfo");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ProductInfo");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "ProductInfo");

            migrationBuilder.RenameColumn(
                name: "ImagesDetail",
                table: "ProductInfo",
                newName: "Url");
        }
    }
}
