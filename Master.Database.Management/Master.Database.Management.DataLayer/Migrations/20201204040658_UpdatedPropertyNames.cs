using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
    public partial class UpdatedPropertyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplate_FixCategory_FixCategoryId",
                table: "FixTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplate_FixType_FixTypeId",
                table: "FixTemplate");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionField_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateSe~",
                table: "FixTemplateField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateSectionField_FixTemplate_FixTemplateId",
                table: "FixTemplateSectionField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateSectionField_FixTemplateSection_SectionId",
                table: "FixTemplateSectionField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateTag_FixTemplate_FixTemplateId",
                table: "FixTemplateTag");

            migrationBuilder.DropTable(
                name: "FixTemplateFieldValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixType",
                table: "FixType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateTag",
                table: "FixTemplateTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateSectionField",
                table: "FixTemplateSectionField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplate",
                table: "FixTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixCategory",
                table: "FixCategory");

            migrationBuilder.RenameTable(
                name: "FixType",
                newName: "FixTypes");

            migrationBuilder.RenameTable(
                name: "FixTemplateTag",
                newName: "FixTemplateTags");

            migrationBuilder.RenameTable(
                name: "FixTemplateSectionField",
                newName: "FixTemplateSectionFields");

            migrationBuilder.RenameTable(
                name: "FixTemplate",
                newName: "FixTemplates");

            migrationBuilder.RenameTable(
                name: "FixCategory",
                newName: "FixCategories");

            migrationBuilder.RenameColumn(
                name: "TagName",
                table: "FixTemplateTags",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplateSectionField_SectionId",
                table: "FixTemplateSectionFields",
                newName: "IX_FixTemplateSectionFields_SectionId");

            migrationBuilder.RenameColumn(
                name: "FixTypeId",
                table: "FixTemplates",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "FixCategoryId",
                table: "FixTemplates",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplate_FixTypeId",
                table: "FixTemplates",
                newName: "IX_FixTemplates_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplate_FixCategoryId",
                table: "FixTemplates",
                newName: "IX_FixTemplates_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTypes",
                table: "FixTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateTags",
                table: "FixTemplateTags",
                columns: new[] { "FixTemplateId", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateSectionFields",
                table: "FixTemplateSectionFields",
                columns: new[] { "FixTemplateId", "SectionId", "FieldId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplates",
                table: "FixTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixCategories",
                table: "FixCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FixTemplateFieldValues",
                columns: table => new
                {
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateFieldValues", x => new { x.FieldId, x.Value });
                    table.ForeignKey(
                        name: "FK_FixTemplateFieldValues_FixTemplateField_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FixTemplateField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionFields",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplates_FixTemplateId",
                table: "FixTemplateSectionFields",
                column: "FixTemplateId",
                principalTable: "FixTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplateSection_SectionId",
                table: "FixTemplateSectionFields",
                column: "SectionId",
                principalTable: "FixTemplateSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateTags_FixTemplates_FixTemplateId",
                table: "FixTemplateTags",
                column: "FixTemplateId",
                principalTable: "FixTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_FixCategories_CategoryId",
                table: "FixTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplates_FixTypes_TypeId",
                table: "FixTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplates_FixTemplateId",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplateSection_SectionId",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateTags_FixTemplates_FixTemplateId",
                table: "FixTemplateTags");

            migrationBuilder.DropTable(
                name: "FixTemplateFieldValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTypes",
                table: "FixTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateTags",
                table: "FixTemplateTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateSectionFields",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplates",
                table: "FixTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixCategories",
                table: "FixCategories");

            migrationBuilder.RenameTable(
                name: "FixTypes",
                newName: "FixType");

            migrationBuilder.RenameTable(
                name: "FixTemplateTags",
                newName: "FixTemplateTag");

            migrationBuilder.RenameTable(
                name: "FixTemplateSectionFields",
                newName: "FixTemplateSectionField");

            migrationBuilder.RenameTable(
                name: "FixTemplates",
                newName: "FixTemplate");

            migrationBuilder.RenameTable(
                name: "FixCategories",
                newName: "FixCategory");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FixTemplateTag",
                newName: "TagName");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionField",
                newName: "IX_FixTemplateSectionField_SectionId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "FixTemplate",
                newName: "FixTypeId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "FixTemplate",
                newName: "FixCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplates_TypeId",
                table: "FixTemplate",
                newName: "IX_FixTemplate_FixTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FixTemplates_CategoryId",
                table: "FixTemplate",
                newName: "IX_FixTemplate_FixCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixType",
                table: "FixType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateTag",
                table: "FixTemplateTag",
                columns: new[] { "FixTemplateId", "TagName" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateSectionField",
                table: "FixTemplateSectionField",
                columns: new[] { "FixTemplateId", "SectionId", "FieldId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplate",
                table: "FixTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixCategory",
                table: "FixCategory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FixTemplateFieldValue",
                columns: table => new
                {
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateFieldValue", x => new { x.FieldId, x.Value });
                    table.ForeignKey(
                        name: "FK_FixTemplateFieldValue_FixTemplateField_FieldId",
                        column: x => x.FieldId,
                        principalTable: "FixTemplateField",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplate_FixCategory_FixCategoryId",
                table: "FixTemplate",
                column: "FixCategoryId",
                principalTable: "FixCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplate_FixType_FixTypeId",
                table: "FixTemplate",
                column: "FixTypeId",
                principalTable: "FixType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionField_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateSe~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionField",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateSectionField_FixTemplate_FixTemplateId",
                table: "FixTemplateSectionField",
                column: "FixTemplateId",
                principalTable: "FixTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateSectionField_FixTemplateSection_SectionId",
                table: "FixTemplateSectionField",
                column: "SectionId",
                principalTable: "FixTemplateSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateTag_FixTemplate_FixTemplateId",
                table: "FixTemplateTag",
                column: "FixTemplateId",
                principalTable: "FixTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
