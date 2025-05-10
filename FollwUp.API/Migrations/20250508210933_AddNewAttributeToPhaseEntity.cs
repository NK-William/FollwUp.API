using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FollwUp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAttributeToPhaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IconType",
                table: "Phases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconType",
                table: "Phases");
        }
    }
}
