using Microsoft.EntityFrameworkCore.Migrations;

namespace Master.Database.Management.DataLayer.Migrations
{
  public partial class AddExtensionsToModels : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<long>(
          name: "CreatedTimestampUtc",
          table: "FixTypes",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "DeletedTimestampUtc",
          table: "FixTypes",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<bool>(
          name: "IsDeleted",
          table: "FixTypes",
          nullable: false,
          defaultValue: false);

      migrationBuilder.AddColumn<long>(
          name: "LastAccessedTimestampUtc",
          table: "FixTypes",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "UpdatedTimestampUtc",
          table: "FixTypes",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "CreatedTimestampUtc",
          table: "FixCategories",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "DeletedTimestampUtc",
          table: "FixCategories",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<bool>(
          name: "IsDeleted",
          table: "FixCategories",
          nullable: false,
          defaultValue: false);

      migrationBuilder.AddColumn<long>(
          name: "LastAccessedTimestampUtc",
          table: "FixCategories",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "UpdatedTimestampUtc",
          table: "FixCategories",
          nullable: false,
          defaultValue: 0L);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CreatedTimestampUtc",
          table: "FixTypes");

      migrationBuilder.DropColumn(
          name: "DeletedTimestampUtc",
          table: "FixTypes");

      migrationBuilder.DropColumn(
          name: "IsDeleted",
          table: "FixTypes");

      migrationBuilder.DropColumn(
          name: "LastAccessedTimestampUtc",
          table: "FixTypes");

      migrationBuilder.DropColumn(
          name: "UpdatedTimestampUtc",
          table: "FixTypes");

      migrationBuilder.DropColumn(
          name: "CreatedTimestampUtc",
          table: "FixCategories");

      migrationBuilder.DropColumn(
          name: "DeletedTimestampUtc",
          table: "FixCategories");

      migrationBuilder.DropColumn(
          name: "IsDeleted",
          table: "FixCategories");

      migrationBuilder.DropColumn(
          name: "LastAccessedTimestampUtc",
          table: "FixCategories");

      migrationBuilder.DropColumn(
          name: "UpdatedTimestampUtc",
          table: "FixCategories");
    }
  }
}
