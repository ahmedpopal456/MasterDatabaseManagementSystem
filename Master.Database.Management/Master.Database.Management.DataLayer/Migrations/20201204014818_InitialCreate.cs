using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
					name: "FixCategory",
					columns: table => new
					{
						Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_FixCategory", x => x.Id);
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
					name: "FixType",
					columns: table => new
					{
						Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_FixType", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "FixTemplate",
					columns: table => new
					{
						Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Status = table.Column<int>(type: "int", nullable: false),
						Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
						FixCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						FixTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						Description = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false),
						SystemCostEstimate = table.Column<double>(type: "float", nullable: false),
						IsDeleted = table.Column<bool>(type: "bit", nullable: true),
						DeletedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
						CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
						UpdatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
						CreatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
						UpdatedTimestampUtc = table.Column<long>(type: "bigint", nullable: false),
						LastAccessedTimestampUtc = table.Column<long>(type: "bigint", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_FixTemplate", x => x.Id);
						table.ForeignKey(
											name: "FK_FixTemplate_FixCategory_FixCategoryId",
											column: x => x.FixCategoryId,
											principalTable: "FixCategory",
											principalColumn: "Id",
											onDelete: ReferentialAction.Restrict);
						table.ForeignKey(
											name: "FK_FixTemplate_FixType_FixTypeId",
											column: x => x.FixTypeId,
											principalTable: "FixType",
											principalColumn: "Id",
											onDelete: ReferentialAction.Restrict);
					});

			migrationBuilder.CreateTable(
					name: "FixTemplateSectionField",
					columns: table => new
					{
						FixTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						IsDeleted = table.Column<bool>(type: "bit", nullable: true),
						DeletedTimestampUtc = table.Column<long>(type: "bigint", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_FixTemplateSectionField", x => new { x.FixTemplateId, x.SectionId, x.FieldId });
						table.ForeignKey(
											name: "FK_FixTemplateSectionField_FixTemplate_FixTemplateId",
											column: x => x.FixTemplateId,
											principalTable: "FixTemplate",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
						table.ForeignKey(
											name: "FK_FixTemplateSectionField_FixTemplateSection_SectionId",
											column: x => x.SectionId,
											principalTable: "FixTemplateSection",
											principalColumn: "Id",
											onDelete: ReferentialAction.Cascade);
					});

			migrationBuilder.CreateTable(
					name: "FixTemplateTag",
					columns: table => new
					{
						FixTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
						TagName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
						IsDeleted = table.Column<bool>(type: "bit", nullable: true),
						DeletedTimestampUtc = table.Column<long>(type: "bigint", nullable: false)
					},
					constraints: table =>
					{
						table.PrimaryKey("PK_FixTemplateTag", x => new { x.FixTemplateId, x.TagName });
						table.ForeignKey(
											name: "FK_FixTemplateTag_FixTemplate_FixTemplateId",
											column: x => x.FixTemplateId,
											principalTable: "FixTemplate",
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
											name: "FK_FixTemplateField_FixTemplateSectionField_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateSe~",
											columns: x => new { x.FixTemplateSectionFieldFixTemplateId, x.FixTemplateSectionFieldSectionId, x.FixTemplateSectionFieldFieldId },
											principalTable: "FixTemplateSectionField",
											principalColumns: new[] { "FixTemplateId", "SectionId", "FieldId" },
											onDelete: ReferentialAction.Cascade);
					});

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

			migrationBuilder.CreateIndex(
					name: "IX_FixTemplate_FixCategoryId",
					table: "FixTemplate",
					column: "FixCategoryId");

			migrationBuilder.CreateIndex(
					name: "IX_FixTemplate_FixTypeId",
					table: "FixTemplate",
					column: "FixTypeId");

			migrationBuilder.CreateIndex(
					name: "IX_FixTemplateField_FixTemplateSectionFieldFixTemplateId_FixTemplateSectionFieldSectionId_FixTemplateSectionFieldFieldId",
					table: "FixTemplateField",
					columns: new[] { "FixTemplateSectionFieldFixTemplateId", "FixTemplateSectionFieldSectionId", "FixTemplateSectionFieldFieldId" });

			migrationBuilder.CreateIndex(
					name: "IX_FixTemplateSectionField_SectionId",
					table: "FixTemplateSectionField",
					column: "SectionId",
					unique: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
					name: "FixTemplateFieldValue");

			migrationBuilder.DropTable(
					name: "FixTemplateTag");

			migrationBuilder.DropTable(
					name: "FixTemplateField");

			migrationBuilder.DropTable(
					name: "FixTemplateSectionField");

			migrationBuilder.DropTable(
					name: "FixTemplate");

			migrationBuilder.DropTable(
					name: "FixTemplateSection");

			migrationBuilder.DropTable(
					name: "FixCategory");

			migrationBuilder.DropTable(
					name: "FixType");
		}
	}
}
