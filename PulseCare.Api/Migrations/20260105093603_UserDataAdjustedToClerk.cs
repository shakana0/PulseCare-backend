using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PulseCare.Api.Migrations
{
    /// <inheritdoc />
    public partial class UserDataAdjustedToClerk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClerkId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClerkId",
                table: "Users");
        }
    }
}
