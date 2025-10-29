using System.ComponentModel;
using System.Text.Json.Serialization;
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
    public record Request
    {
        [DefaultValue("/vacancies")]
        public required string Location { get; set; }
        [JsonPropertyName("utm_source"), DefaultValue("koworking")]
        public required string UtmSource { get; set; }
        [JsonPropertyName("utm_medium"), DefaultValue("copy_link")]
        public required string UtmMedium { get; set; }
        [JsonPropertyName("utm_campaign"), DefaultValue("referral")]
        public required string UtmCampaign { get; set; }
        [JsonPropertyName("utm_term")]
        public string? UtmTerm { get; set; }
        [JsonPropertyName("utm_content")]
        public string? UtmContent { get; set; }
        [DefaultValue("scalar")]
        public string? UserAgent { get; set; }
    }
    
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) => endpoint
        .WithTags(nameof(SiteVisit))
        .WithDescription("Отметка о переходе на сайт");

    private static async ValueTask<Ok> HandleAsync(
        [FromBody] Request request,
        IHttpContextAccessor contextAccessor,
        AnalyticsContext analytics,
        ILogger<Handler> logger,
        CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        (string name, object? value, bool nullable)[] parameters =
        [
            ("now", now, false),
            ("p0", request.Location, false),
            ("p1", request.UtmCampaign, false),
            ("p2", request.UtmContent, true),
            ("p3", request.UtmMedium, false),
            ("p4", request.UtmSource, false),
            ("p5", request.UtmTerm, true),
            ("p6", request.UserAgent, true)
        ];
        
        var conn = analytics.Database.GetDbConnection();
        await using var command = conn.CreateCommand();
        command.CommandText =
            // lang=clickhouse
            """
            -- noinspection SqlResolve
            INSERT INTO "site_visits" ("created_at", "location", "utm_campaign", "utm_content", "utm_medium", "utm_source", "utm_term", "user_agent")
            VALUES ({now:DateTime}, {p0:String}, {p1:String}, {p2:Nullable(String)}, {p3:String}, {p4:String}, {p5:Nullable(String)}, {p6:Nullable(String)});
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
        logger.LogInformation("Registered visit {@Visit} at {Now}", request, now);

        return TypedResults.Ok();
    }
}