using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
    public partial class UpdatedFieldsRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields",
                column: "FieldId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields",
                column: "FieldId",
                unique: true);
        }
    }
}
