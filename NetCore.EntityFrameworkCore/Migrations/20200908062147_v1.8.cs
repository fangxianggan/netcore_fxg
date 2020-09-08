using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RequestType",
                table: "TaskJob",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MailMessage",
                table: "TaskJob",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailMessage",
                table: "TaskJob");

            migrationBuilder.AlterColumn<string>(
                name: "RequestType",
                table: "TaskJob",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
