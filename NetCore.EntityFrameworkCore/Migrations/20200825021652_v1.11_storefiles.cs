using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore.EntityFrameworkCore.Migrations
{
    public partial class v111_storefiles : Migration
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
                    FileType = table.Column<string>(maxLength: 16, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    CreateBy = table.Column<string>(maxLength: 32, nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UpdateBy = table.Column<string>(maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreFiles", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreFiles");
        }
    }
}
