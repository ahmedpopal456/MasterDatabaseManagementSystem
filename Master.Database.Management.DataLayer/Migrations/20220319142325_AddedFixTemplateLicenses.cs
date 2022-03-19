using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
  public partial class AddedFixTemplateLicenses : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "License",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            ReferenceId = table.Column<float>(nullable: false),
            Name = table.Column<string>(maxLength: 128, nullable: false),
            Description = table.Column<string>(maxLength: 2147483647, nullable: false),
            CreatedTimestampUtc = table.Column<long>(nullable: false),
            UpdatedTimestampUtc = table.Column<long>(nullable: false),
            LastAccessedTimestampUtc = table.Column<long>(nullable: false),
            IsDeleted = table.Column<bool>(nullable: false),
            DeletedTimestampUtc = table.Column<long>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_License", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FixTemplateLicense",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            LicenseId = table.Column<Guid>(nullable: false),
            FixTemplateId = table.Column<Guid>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTemplateLicense", x => x.Id);
            table.ForeignKey(
                      name: "FK_FixTemplateLicense_FixTemplates_FixTemplateId",
                      column: x => x.FixTemplateId,
                      principalTable: "FixTemplates",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_FixTemplateLicense_License_LicenseId",
                      column: x => x.LicenseId,
                      principalTable: "License",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "LicenseTag",
          columns: table => new
          {
            LicenseId = table.Column<Guid>(nullable: false),
            Name = table.Column<string>(maxLength: 32, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_LicenseTag", x => new { x.LicenseId, x.Name });
            table.ForeignKey(
                      name: "FK_LicenseTag_License_LicenseId",
                      column: x => x.LicenseId,
                      principalTable: "License",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateLicense_FixTemplateId",
          table: "FixTemplateLicense",
          column: "FixTemplateId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateLicense_LicenseId",
          table: "FixTemplateLicense",
          column: "LicenseId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "FixTemplateLicense");

      migrationBuilder.DropTable(
          name: "LicenseTag");

      migrationBuilder.DropTable(
          name: "License");
    }
  }
}
