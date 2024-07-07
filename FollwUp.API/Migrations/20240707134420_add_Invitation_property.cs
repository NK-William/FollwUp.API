using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FollwUp.API.Migrations
{
    /// <inheritdoc />
    public partial class add_Invitation_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleType",
                table: "Invitations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "Invitations");
        }
    }
}
