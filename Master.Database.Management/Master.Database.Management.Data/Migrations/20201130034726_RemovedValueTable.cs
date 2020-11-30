using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.Data.Migrations
{
    public partial class RemovedValueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField");

            migrationBuilder.DropTable(
                name: "FixTemplateValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateFieldValues",
                table: "FixTemplateFieldValues");

            migrationBuilder.DropColumn(
                name: "ValueId",
                table: "FixTemplateFieldValues");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "FixTemplateFieldValues",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateFieldValues",
                table: "FixTemplateFieldValues",
                columns: new[] { "FieldId", "Value" });

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionFields",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixTemplateFieldValues",
                table: "FixTemplateFieldValues");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "FixTemplateFieldValues");

            migrationBuilder.AddColumn<Guid>(
                name: "ValueId",
                table: "FixTemplateFieldValues",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixTemplateFieldValues",
                table: "FixTemplateFieldValues",
                columns: new[] { "FieldId", "ValueId" });

            migrationBuilder.CreateTable(
                name: "FixTemplateValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixTemplateFieldValueFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FixTemplateFieldValueValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixTemplateValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                        columns: x => new { x.FixTemplateFieldValueFieldId, x.FixTemplateFieldValueValueId },
                        principalTable: "FixTemplateFieldValues",
                        principalColumns: new[] { "FieldId", "ValueId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateValue_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue",
                columns: new[] { "FixTemplateFieldValueFieldId", "FixTemplateFieldValueValueId" });

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionFields",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
