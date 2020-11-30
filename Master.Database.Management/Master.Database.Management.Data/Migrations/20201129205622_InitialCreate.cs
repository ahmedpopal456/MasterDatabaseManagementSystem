using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FixCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FixCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
                    SystemCostEstimate = table.Column<double>(type: "float", nullable: false),
                    CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTimestampUtc = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UpdatedTimestampUtc = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LastAccessedTimestampUtc = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Frequency = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    DeletedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTimestampUtc = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixTemplates_FixCategories_FixCategoryId",
                        column: x => x.FixCategoryId,
                        principalTable: "FixCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FixTemplates_FixTypes_FixTypeId",
                        column: x => x.FixTypeId,
                        principalTable: "FixTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateSectionFields",
                columns: table => new
                {
                    FixTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateSectionFields", x => new { x.FixTemplateId, x.SectionId, x.FieldId });
                    table.ForeignKey(
                        name: "FK_FixTemplateSectionFields_FixTemplates_FixTemplateId",
                        column: x => x.FixTemplateId,
                        principalTable: "FixTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FixTemplateSectionFields_FixTemplateSection_SectionId",
                        column: x => x.SectionId,
                        principalTable: "FixTemplateSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateTags",
                columns: table => new
                {
                    FixTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateTags", x => new { x.FixTemplateId, x.TagName });
                    table.ForeignKey(
                        name: "FK_FixTemplateTags_FixTemplates_FixTemplateId",
                        column: x => x.FixTemplateId,
                        principalTable: "FixTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateField",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FixTemplateSectionFieldFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FixTemplateSectionFieldFixTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FixTemplateSectionFieldSectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                        columns: x => new { x.FixTemplateSectionFieldFixTemplateId, x.FixTemplateSectionFieldSectionId, x.FixTemplateSectionFieldFieldId },
                        principalTable: "FixTemplateSectionFields",
                        principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateFieldValues",
                columns: table => new
                {
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateFieldValues", x => new { x.FieldId, x.ValueId });
                    table.ForeignKey(
                        name: "FK_FixTemplateFieldValues_FixTemplateField_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FixTemplateField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FixTemplateValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    FixTemplateFieldValueFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FixTemplateFieldValueValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                        columns: x => new { x.FixTemplateFieldValueFieldId, x.FixTemplateFieldValueValueId },
                        principalTable: "FixTemplateFieldValues",
                        principalColumns: new[] { "FieldId", "ValueId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateField_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateSectionFieldFieldId",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" });

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_FixCategoryId",
                table: "FixTemplates",
                column: "FixCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplates_FixTypeId",
                table: "FixTemplates",
                column: "FixTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionFields",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateValue_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue",
                columns: new[] { "FixTemplateFieldValueFieldId", "FixTemplateFieldValueValueId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixTemplateTags");

            migrationBuilder.DropTable(
                name: "FixTemplateValue");

            migrationBuilder.DropTable(
                name: "FixTemplateFieldValues");

            migrationBuilder.DropTable(
                name: "FixTemplateField");

            migrationBuilder.DropTable(
                name: "FixTemplateSectionFields");

            migrationBuilder.DropTable(
                name: "FixTemplates");

            migrationBuilder.DropTable(
                name: "FixTemplateSection");

            migrationBuilder.DropTable(
                name: "FixCategories");

            migrationBuilder.DropTable(
                name: "FixTypes");
        }
    }
}
