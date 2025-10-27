using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Koworking.Api.Features.Visits;

public partial record SiteVisit
{
    public record Info
    {
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
    }
}