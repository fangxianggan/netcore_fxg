using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v112_storefiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "StoreFiles",
                maxLength: 16,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "test",
                table: "StoreFiles");
        }
    }
}
