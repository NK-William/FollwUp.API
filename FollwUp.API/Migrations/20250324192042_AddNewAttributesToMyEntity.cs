using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FollwUp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAttributesToMyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientEmail",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientPhone",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientEmail",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ClientPhone",
                table: "Tasks");
        }
    }
}
