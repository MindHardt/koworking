using Koworking.Api.Infrastructure.TextIds;

namespace Koworking.Api.Features.Uploads;

public partial class Upload
{
    public record Model
    {
        public required TextId Id { get; set; }
        public required TextId UploaderId { get; set; }
        public required Sha256HashString Hash { get; set; }
        public required string ContentType { get; set; }
        public required string FileName { get; set; }
        public required FileSize FileSize { get; set; }
        public required DateTimeOffset UploadTime { get; set; }
        public required UploadScope Scope { get; set; }
        
        public Uri GetDownloadUri(string baseUri)=> new UriBuilder(baseUri)
        {
            Path = "/uploads/" + Id,
            Query = string.Empty,
            Fragment = string.Empty,
        }.Uri;
    }
}