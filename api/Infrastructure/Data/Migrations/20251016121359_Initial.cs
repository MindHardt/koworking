using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                // lang=postgresql
                """
                CREATE FUNCTION gen_vacancy_vector(title TEXT, text TEXT) RETURNS tsvector AS $$
                BEGIN
                    RETURN setweight(to_tsvector('russian', title), 'A') || to_tsvector('russian', text);
                END;
                $$ LANGUAGE plpgsql IMMUTABLE ;
                """);
            
            migrationBuilder.CreateTable(
                name: "vacancies",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    paycheck_amount = table.Column<int>(type: "integer", nullable: true),
                    paycheck_period = table.Column<short>(type: "smallint", nullable: true),
                    paycheck_type = table.Column<short>(type: "smallint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ts_vector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "gen_vacancy_vector(title, text)", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vacancies", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vacancies_ts_vector",
                table: "vacancies",
                column: "ts_vector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vacancies");
            
            migrationBuilder.Sql(
                // lang=postgresql
                """
                -- noinspection SqlResolve
                DROP FUNCTION gen_vacancy_vector
                """);
        }
    }
}
