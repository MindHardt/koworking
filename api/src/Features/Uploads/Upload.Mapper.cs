using Koworking.Api.Features.Users;
using Koworking.Api.Infrastructure.TextIds;
using Riok.Mapperly.Abstractions;
using Sqids;

namespace Koworking.Api.Features.Uploads;

public partial class Upload
{
    [Mapper(AutoUserMappings = false), RegisterSingleton]
    public partial class Mapper(TextIdEncoders encoders, Koworker.Mapper koworkerMapper)
    {
        public SqidsEncoder<long> Encoder => encoders.Uploads;
        
        [MapProperty(nameof(Id), nameof(Id), Use = nameof(EncodeUploadId))]
        [MapProperty(nameof(UploaderId), nameof(UploaderId), Use = nameof(EncodeUploaderId))]
        public partial Model ToModel(Upload upload);

        public partial IQueryable<Model> ProjectToModel(IQueryable<Upload> uploads);

        public TextId EncodeUploadId(long uploadId) => Encoder.EncodeTextId(uploadId);
        public TextId EncodeUploaderId(long uploaderId) => koworkerMapper.Encoder.EncodeTextId(uploaderId);
    }
}