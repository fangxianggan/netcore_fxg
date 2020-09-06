using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskJobLog",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    TaskJobId = table.Column<Guid>(nullable: false),
                    JobName = table.Column<string>(maxLength: 64, nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<double>(nullable: false),
                    RunLog = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskJobLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskJobLog");
        }
    }
}
