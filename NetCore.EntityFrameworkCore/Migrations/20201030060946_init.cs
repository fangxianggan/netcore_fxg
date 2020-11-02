using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    MenuCode = table.Column<string>(maxLength: 32, nullable: true),
                    MenuName = table.Column<string>(maxLength: 32, nullable: true),
                    Path = table.Column<string>(maxLength: 32, nullable: true),
                    Icon = table.Column<string>(maxLength: 32, nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    NoChildren = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    OrderStatus = table.Column<int>(nullable: false),
                    PayOutTime = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    PurchaseNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProductInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 64, nullable: true),
                    ProductName = table.Column<string>(maxLength: 64, nullable: true),
                    StockNum = table.Column<int>(nullable: false),
                    LimitedNum = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Des = table.Column<string>(maxLength: 300, nullable: true),
                    Url = table.Column<string>(maxLength: 300, nullable: true),
                    OpeningTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    RoleCode = table.Column<string>(maxLength: 32, nullable: false),
                    RoleName = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenu",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    RoleCode = table.Column<string>(nullable: true),
                    MenuCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StoreFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    FileName = table.Column<string>(maxLength: 128, nullable: true),
                    RelationFilePath = table.Column<string>(maxLength: 128, nullable: true),
                    FileExt = table.Column<string>(maxLength: 8, nullable: true),
                    FileBytes = table.Column<long>(nullable: false),
                    FileType = table.Column<string>(maxLength: 64, nullable: true),
                    FileCategory = table.Column<string>(maxLength: 16, nullable: true)
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
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    TaskGroup = table.Column<string>(maxLength: 64, nullable: true),
                    TaskName = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    ApiUrl = table.Column<string>(maxLength: 256, nullable: true),
                    RequestType = table.Column<int>(nullable: false),
                    RequestHead = table.Column<string>(nullable: true),
                    RequestParams = table.Column<string>(nullable: true),
                    ExceptionMsg = table.Column<string>(nullable: true),
                    CronExpression = table.Column<string>(maxLength: 512, nullable: true),
                    CronExpressionDescription = table.Column<string>(maxLength: 512, nullable: true),
                    StartRunTime = table.Column<DateTime>(nullable: false),
                    EndRunTime = table.Column<DateTime>(nullable: true),
                    RunCount = table.Column<int>(nullable: false),
                    TaskState = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    MailMessage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskJob", x => x.ID);
                });

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

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfo",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UserCode = table.Column<string>(maxLength: 32, nullable: true),
                    UserName = table.Column<string>(maxLength: 64, nullable: true),
                    NickName = table.Column<string>(maxLength: 64, nullable: true),
                    Password = table.Column<string>(maxLength: 64, nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    Email = table.Column<string>(maxLength: 64, nullable: true),
                    State = table.Column<int>(nullable: false),
                    Avatar = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfo", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "OrderInfo");

            migrationBuilder.DropTable(
                name: "ProductInfo");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RoleMenu");

            migrationBuilder.DropTable(
                name: "StoreFiles");

            migrationBuilder.DropTable(
                name: "TaskJob");

            migrationBuilder.DropTable(
                name: "TaskJobLog");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropTable(
                name: "UserInfo");
        }
    }
}
