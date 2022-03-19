using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
  public partial class RemovedWorkCategorySkills : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "WorkCategorySkills");

      migrationBuilder.DropTable(
          name: "Skills");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Skills",
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
            table.PrimaryKey("PK_Skills", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "WorkCategorySkills",
          columns: table => new
          {
            WorkCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
          name: "IX_WorkCategorySkills_SkillId",
          table: "WorkCategorySkills",
          column: "SkillId");
    }
  }
}
