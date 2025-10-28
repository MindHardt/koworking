using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class AddedLocationToStartVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "site_visits",
                type: "String",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "site_visits");
        }
    }
}
