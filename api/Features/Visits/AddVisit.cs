using ClickHouse.Driver.ADO.Parameters;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Koworking.Api.Infrastructure.Data.Analytics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Koworking.Api.Features.Visits;

[Handler, MapPost("/visits/")]
public static partial class AddVisit
{
    public record Request : SiteVisit.Info;
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(SiteVisit))
        .WithDescription("Отметка о переходе на сайт");

    private static async ValueTask<Ok> HandleAsync(
        [FromBody] Request request,
        AnalyticsContext analytics,
        CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        (string name, object? value, bool nullable)[] parameters =
        [
            ("now", now, false),
            ("p1", request.UtmCampaign, false),
            ("p2", request.UtmContent, true),
            ("p3", request.UtmMedium, false),
            ("p4", request.UtmSource, false),
            ("p5", request.UtmTerm, true),
        ];
        
        var conn = analytics.Database.GetDbConnection();
        await using var command = conn.CreateCommand();
        command.CommandText =
            // lang=clickhouse
            """
            INSERT INTO "site_visits" ("created_at", "utm_campaign", "utm_content", "utm_medium", "utm_source", "utm_term")
            VALUES ({now:String}, {p1:String}, {p2:Nullable(String)}, {p3:String}, {p4:String}, {p5:Nullable(String)});
            """;
        foreach (var (name, value, nullable) in parameters)
        {
            var param = command.CreateParameter();
            param.IsNullable = nullable;
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        await command.ExecuteNonQueryAsync(ct);

        return TypedResults.Ok();
    }
}