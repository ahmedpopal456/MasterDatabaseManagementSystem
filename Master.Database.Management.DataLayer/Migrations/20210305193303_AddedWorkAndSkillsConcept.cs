using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
    public partial class AddedWorkAndSkillsConcept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_FixCategories_CategoryId",
                table: "FixTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_FixTypes_TypeId",
                table: "FixTemplates");

            migrationBuilder.DropTable(
                name: "FixCategories");

            migrationBuilder.DropTable(
                name: "FixTypes");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplates_CategoryId",
                table: "FixTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplates_TypeId",
                table: "FixTemplates");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FixTemplates");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "FixTemplates");

            migrationBuilder.AddColumn<Guid>(
                name: "FixUnitId",
                table: "FixTemplates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkCategoryId",
                table: "FixTemplates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkTypeId",
                table: "FixTemplates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FixUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedTimestampUtc = table.Column<long>(nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedTimestampUtc = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedTimestampUtc = table.Column<long>(nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedTimestampUtc = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedTimestampUtc = table.Column<long>(nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedTimestampUtc = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedTimestampUtc = table.Column<long>(nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedTimestampUtc = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkCategorySkills",
                columns: table => new
                {
                    WorkCategoryId = table.Column<Guid>(nullable: false),
                    SkillId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCategorySkills", x => new { x.WorkCategoryId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_WorkCategorySkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkCategorySkills_WorkCategories_WorkCategoryId",
                        column: x => x.WorkCategoryId,
                        principalTable: "WorkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_FixUnitId",
                table: "FixTemplates",
                column: "FixUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_WorkCategoryId",
                table: "FixTemplates",
                column: "WorkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_WorkTypeId",
                table: "FixTemplates",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCategorySkills_SkillId",
                table: "WorkCategorySkills",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplates_FixUnits_FixUnitId",
                table: "FixTemplates",
                column: "FixUnitId",
                principalTable: "FixUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplates_WorkCategories_WorkCategoryId",
                table: "FixTemplates",
                column: "WorkCategoryId",
                principalTable: "WorkCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplates_WorkTypes_WorkTypeId",
                table: "FixTemplates",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_FixUnits_FixUnitId",
                table: "FixTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_WorkCategories_WorkCategoryId",
                table: "FixTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_WorkTypes_WorkTypeId",
                table: "FixTemplates");

            migrationBuilder.DropTable(
                name: "FixUnits");

            migrationBuilder.DropTable(
                name: "WorkCategorySkills");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "WorkCategories");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplates_FixUnitId",
                table: "FixTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplates_WorkCategoryId",
                table: "FixTemplates");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplates_WorkTypeId",
                table: "FixTemplates");

            migrationBuilder.DropColumn(
                name: "FixUnitId",
                table: "FixTemplates");

            migrationBuilder.DropColumn(
                name: "WorkCategoryId",
                table: "FixTemplates");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "FixTemplates");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "FixTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "FixTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FixCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    DeletedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    DeletedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastAccessedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UpdatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_CategoryId",
                table: "FixTemplates",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_TypeId",
                table: "FixTemplates",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplates_FixCategories_CategoryId",
                table: "FixTemplates",
                column: "CategoryId",
                principalTable: "FixCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplates_FixTypes_TypeId",
                table: "FixTemplates",
                column: "TypeId",
                principalTable: "FixTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
