using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                   
                    FileName = table.Column<string>(maxLength: 128, nullable: true),
                    RelationFilePath = table.Column<string>(maxLength: 128, nullable: true),
                    FileExt = table.Column<string>(maxLength: 8, nullable: true),
                    FileBytes = table.Column<long>(nullable: false),
                    FileType = table.Column<string>(maxLength: 64, nullable: true),
                    FileCategory = table.Column<string>(maxLength: 16, nullable: true),
                     CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreFiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskJob",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    TaskGroup = table.Column<string>(maxLength: 64, nullable: true),
                    TaskName = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ApiUrl = table.Column<string>(maxLength: 256, nullable: true),
                    RequestType = table.Column<string>(maxLength: 8, nullable: true),
                    RequestHead = table.Column<string>(maxLength: 1024, nullable: true),
                    RequestParams = table.Column<string>(nullable: true),
                    ExceptionMsg = table.Column<string>(nullable: true),
                    CronExpression = table.Column<string>(maxLength: 512, nullable: true),
                    CronExpressionDescription = table.Column<string>(maxLength: 512, nullable: true),
                    StartRunTime = table.Column<DateTime>(nullable: true),
                    EndRunTime = table.Column<DateTime>(nullable: true),
                    RunCount = table.Column<int>(nullable: false),
                    TaskState = table.Column<string>(maxLength: 8, nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskJob", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreFiles");

            migrationBuilder.DropTable(
                name: "TaskJob");

            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}
