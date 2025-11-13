using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserAgent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_agent",
                table: "site_visits",
                type: "String",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_agent",
                table: "site_visits");
        }
    }
}
