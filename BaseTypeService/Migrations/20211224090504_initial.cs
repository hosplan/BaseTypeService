using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseTypeService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseRootType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseRootType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaseBranchType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseRootTypeId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseBranchType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseBranchType_BaseRootType_BaseRootTypeId",
                        column: x => x.BaseRootTypeId,
                        principalTable: "BaseRootType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseBranchType_BaseRootTypeId",
                table: "BaseBranchType",
                column: "BaseRootTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseBranchType");

            migrationBuilder.DropTable(
                name: "BaseRootType");
        }
    }
}
