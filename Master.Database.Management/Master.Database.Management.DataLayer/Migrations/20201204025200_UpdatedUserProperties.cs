using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
  public partial class UpdatedUserProperties : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CreatedByUser",
          table: "FixTemplate");

      migrationBuilder.DropColumn(
          name: "UpdatedByUser",
          table: "FixTemplate");

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplateTag",
          type: "bit",
          nullable: false,
          defaultValue: false,
          oldClrType: typeof(bool),
          oldType: "bit",
          oldNullable: true);

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplateSectionField",
          type: "bit",
          nullable: false,
          defaultValue: false,
          oldClrType: typeof(bool),
          oldType: "bit",
          oldNullable: true);

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplate",
          type: "bit",
          nullable: false,
          defaultValue: false,
          oldClrType: typeof(bool),
          oldType: "bit",
          oldNullable: true);

      migrationBuilder.AddColumn<Guid>(
          name: "CreatedByUserId",
          table: "FixTemplate",
          type: "uniqueidentifier",
          nullable: false,
          defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

      migrationBuilder.AddColumn<Guid>(
          name: "UpdatedByUserId",
          table: "FixTemplate",
          type: "uniqueidentifier",
          nullable: false,
          defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CreatedByUserId",
          table: "FixTemplate");

      migrationBuilder.DropColumn(
          name: "UpdatedByUserId",
          table: "FixTemplate");

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplateTag",
          type: "bit",
          nullable: true,
          oldClrType: typeof(bool),
          oldType: "bit");

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplateSectionField",
          type: "bit",
          nullable: true,
          oldClrType: typeof(bool),
          oldType: "bit");

      migrationBuilder.AlterColumn<bool>(
          name: "IsDeleted",
          table: "FixTemplate",
          type: "bit",
          nullable: true,
          oldClrType: typeof(bool),
          oldType: "bit");

      migrationBuilder.AddColumn<Guid>(
          name: "CreatedByUser",
          table: "FixTemplate",
          type: "uniqueidentifier",
          nullable: true);

      migrationBuilder.AddColumn<Guid>(
          name: "UpdatedByUser",
          table: "FixTemplate",
          type: "uniqueidentifier",
          nullable: true);
    }
  }
}
