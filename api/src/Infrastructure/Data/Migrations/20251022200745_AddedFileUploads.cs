using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileUploads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "koworkers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "uploads",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, collation: "C"),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    scope = table.Column<short>(type: "smallint", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    uploader_id = table.Column<long>(type: "bigint", nullable: false),
                    upload_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_uploads", x => x.id);
                    table.ForeignKey(
                        name: "fk_uploads_koworkers_uploader_id",
                        column: x => x.uploader_id,
                        principalTable: "koworkers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_uploads_hash",
                table: "uploads",
                column: "hash");

            migrationBuilder.CreateIndex(
                name: "ix_uploads_uploader_id",
                table: "uploads",
                column: "uploader_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uploads");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "koworkers");
        }
    }
}
