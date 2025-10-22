using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Koworking.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreVacancyFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                // lang=postgresql
                "TRUNCATE TABLE vacancies");
            
            migrationBuilder.RenameColumn(
                name: "text",
                table: "vacancies",
                newName: "expectations");

            migrationBuilder.AddColumn<string>(
                name: "conditions",
                table: "vacancies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "vacancies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(
                // lang=postgresql
                """
                CREATE OR REPLACE FUNCTION gen_vacancy_vector(title TEXT, rest TEXT[]) RETURNS tsvector AS $$
                BEGIN
                    RETURN setweight(to_tsvector('russian', title), 'A') || to_tsvector('russian', array_to_string(rest, ' '));
                END;
                $$ LANGUAGE plpgsql IMMUTABLE ;
                """);

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "ts_vector",
                table: "vacancies",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "gen_vacancy_vector(title, array [description, conditions, expectations])",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldComputedColumnSql: "gen_vacancy_vector(title, text)",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "conditions",
                table: "vacancies");

            migrationBuilder.DropColumn(
                name: "description",
                table: "vacancies");

            migrationBuilder.RenameColumn(
                name: "expectations",
                table: "vacancies",
                newName: "text");
            
            migrationBuilder.Sql(
                // lang=postgresql
                """
                CREATE OR REPLACE FUNCTION gen_vacancy_vector(title TEXT, text TEXT) RETURNS tsvector AS $$
                BEGIN
                    RETURN setweight(to_tsvector('russian', title), 'A') || to_tsvector('russian', text);
                END;
                $$ LANGUAGE plpgsql IMMUTABLE ;
                """);

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "ts_vector",
                table: "vacancies",
                type: "tsvector",
                nullable: false,
                computedColumnSql: "gen_vacancy_vector(title, text)",
                stored: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldComputedColumnSql: "gen_vacancy_vector(title, array [description, conditions, expectations])",
                oldStored: true);
        }
    }
}
