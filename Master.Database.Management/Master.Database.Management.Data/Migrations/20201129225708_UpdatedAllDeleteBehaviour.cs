using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.Data.Migrations
{
    public partial class UpdatedAllDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionFields");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionFields",
                column: "SectionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionFields",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue",
                columns: new[] { "FixTemplateFieldValueFieldId", "FixTemplateFieldValueValueId" },
                principalTable: "FixTemplateFieldValues",
                principalColumns: new[] { "FieldId", "ValueId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionFields");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_SectionId",
                table: "FixTemplateSectionFields",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateField_FixTemplateSectionFields_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateS~",
                table: "FixTemplateField",
                columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" },
                principalTable: "FixTemplateSectionFields",
                principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateValue_FixTemplateFieldValues_FixTemplateFieldValueFieldId_FixTemplateFieldValueValueId",
                table: "FixTemplateValue",
                columns: new[] { "FixTemplateFieldValueFieldId", "FixTemplateFieldValueValueId" },
                principalTable: "FixTemplateFieldValues",
                principalColumns: new[] { "FieldId", "ValueId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
