using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Fields",
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
            table.PrimaryKey("PK_Fields", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FixCategories",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            Name = table.Column<string>(maxLength: 32, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixCategories", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FixTypes",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            Name = table.Column<string>(maxLength: 32, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTypes", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Sections",
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
            table.PrimaryKey("PK_Sections", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "FixTemplates",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            Status = table.Column<int>(nullable: false),
            Name = table.Column<string>(maxLength: 32, nullable: false),
            CategoryId = table.Column<Guid>(nullable: false),
            TypeId = table.Column<Guid>(nullable: false),
            Description = table.Column<string>(maxLength: 2147483647, nullable: false),
            SystemCostEstimate = table.Column<double>(nullable: false),
            CreatedByUserId = table.Column<Guid>(nullable: false),
            UpdatedByUserId = table.Column<Guid>(nullable: false),
            IsDeleted = table.Column<bool>(nullable: false),
            DeletedTimestampUtc = table.Column<long>(nullable: false),
            CreatedTimestampUtc = table.Column<long>(nullable: false),
            UpdatedTimestampUtc = table.Column<long>(nullable: false),
            LastAccessedTimestampUtc = table.Column<long>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTemplates", x => x.Id);
            table.ForeignKey(
                      name: "FK_FixTemplates_FixCategories_CategoryId",
                      column: x => x.CategoryId,
                      principalTable: "FixCategories",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_FixTemplates_FixTypes_TypeId",
                      column: x => x.TypeId,
                      principalTable: "FixTypes",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "FixTemplateSections",
          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            SectionId = table.Column<Guid>(nullable: false),
            FixTemplateId = table.Column<Guid>(nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTemplateSections", x => x.Id);
            table.ForeignKey(
                      name: "FK_FixTemplateSections_FixTemplates_FixTemplateId",
                      column: x => x.FixTemplateId,
                      principalTable: "FixTemplates",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_FixTemplateSections_Sections_SectionId",
                      column: x => x.SectionId,
                      principalTable: "Sections",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "FixTemplateTags",
          columns: table => new
          {
            FixTemplateId = table.Column<Guid>(nullable: false),
            Name = table.Column<string>(maxLength: 32, nullable: false),
            FixTemplateId1 = table.Column<Guid>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTemplateTags", x => new { x.FixTemplateId, x.Name });
            table.ForeignKey(
                      name: "FK_FixTemplateTags_FixTemplates_FixTemplateId",
                      column: x => x.FixTemplateId,
                      principalTable: "FixTemplates",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_FixTemplateTags_FixTemplates_FixTemplateId1",
                      column: x => x.FixTemplateId1,
                      principalTable: "FixTemplates",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateTable(
          name: "FixTemplateSectionFields",
          columns: table => new
          {
            FixTemplateSectionId = table.Column<Guid>(nullable: false),
            FieldId = table.Column<Guid>(nullable: false),
            Id = table.Column<Guid>(nullable: false),
            FixTemplateSectionId1 = table.Column<Guid>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_FixTemplateSectionFields", x => new { x.FixTemplateSectionId, x.FieldId });
            table.ForeignKey(
                      name: "FK_FixTemplateSectionFields_Fields_FieldId",
                      column: x => x.FieldId,
                      principalTable: "Fields",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_FixTemplateSectionFields_FixTemplateSections_FixTemplateSectionId",
                      column: x => x.FixTemplateSectionId,
                      principalTable: "FixTemplateSections",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_FixTemplateSectionFields_FixTemplateSections_FixTemplateSectionId1",
                      column: x => x.FixTemplateSectionId1,
                      principalTable: "FixTemplateSections",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplates_CategoryId",
          table: "FixTemplates",
          column: "CategoryId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplates_TypeId",
          table: "FixTemplates",
          column: "TypeId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateSectionFields_FieldId",
          table: "FixTemplateSectionFields",
          column: "FieldId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateSectionFields_FixTemplateSectionId1",
          table: "FixTemplateSectionFields",
          column: "FixTemplateSectionId1");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateSections_FixTemplateId",
          table: "FixTemplateSections",
          column: "FixTemplateId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateSections_SectionId",
          table: "FixTemplateSections",
          column: "SectionId");

      migrationBuilder.CreateIndex(
          name: "IX_FixTemplateTags_FixTemplateId1",
          table: "FixTemplateTags",
          column: "FixTemplateId1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "FixTemplateSectionFields");

      migrationBuilder.DropTable(
          name: "FixTemplateTags");

      migrationBuilder.DropTable(
          name: "Fields");

      migrationBuilder.DropTable(
          name: "FixTemplateSections");

      migrationBuilder.DropTable(
          name: "FixTemplates");

      migrationBuilder.DropTable(
          name: "Sections");

      migrationBuilder.DropTable(
          name: "FixCategories");

      migrationBuilder.DropTable(
          name: "FixTypes");
    }
  }
}
