using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartRunTime",
                table: "TaskJob",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartRunTime",
                table: "TaskJob",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
