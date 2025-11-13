using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "site_visits",
                columns: table => new
                {
                    utm_source = table.Column<string>(type: "String", nullable: false),
                    utm_medium = table.Column<string>(type: "String", nullable: false),
                    utm_campaign = table.Column<string>(type: "String", nullable: false),
                    utm_term = table.Column<string>(type: "String", nullable: true),
                    utm_content = table.Column<string>(type: "String", nullable: true),
                    created_at = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                })
                .Annotation("ClickHouse:TableEngine", "MergeTree")
                .Annotation("ClickHouse:TableEngine:OrderBy", new[] { "created_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "site_visits");
        }
    }
}
