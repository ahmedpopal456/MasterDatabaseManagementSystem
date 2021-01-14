using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
    public partial class UpdatedFixTemplateSectionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplateSections_FixTemplateSectionId1",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropForeignKey(
                name: "FK_FixTemplateTags_FixTemplates_FixTemplateId1",
                table: "FixTemplateTags");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplateTags_FixTemplateId1",
                table: "FixTemplateTags");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_FixTemplateSectionId1",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropColumn(
                name: "FixTemplateId1",
                table: "FixTemplateTags");

            migrationBuilder.DropColumn(
                name: "FixTemplateSectionId1",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FixTemplateSectionFields");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "FixTemplateSectionFields",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields",
                column: "FieldId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "FixTemplateSectionFields");

            migrationBuilder.AddColumn<Guid>(
                name: "FixTemplateId1",
                table: "FixTemplateTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FixTemplateSectionId1",
                table: "FixTemplateSectionFields",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "FixTemplateSectionFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateTags_FixTemplateId1",
                table: "FixTemplateTags",
                column: "FixTemplateId1");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_FieldId",
                table: "FixTemplateSectionFields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_FixTemplateSectionFields_FixTemplateSectionId1",
                table: "FixTemplateSectionFields",
                column: "FixTemplateSectionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateSectionFields_FixTemplateSections_FixTemplateSectionId1",
                table: "FixTemplateSectionFields",
                column: "FixTemplateSectionId1",
                principalTable: "FixTemplateSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FixTemplateTags_FixTemplates_FixTemplateId1",
                table: "FixTemplateTags",
                column: "FixTemplateId1",
                principalTable: "FixTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
