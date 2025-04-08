using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FollwUp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAttributesToFollwupDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientFirstName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientLastName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientFirstName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ClientLastName",
                table: "Tasks");
        }
    }
}
