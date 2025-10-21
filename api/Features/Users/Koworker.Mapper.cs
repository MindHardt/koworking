using Koworking.Api.Infrastructure.TextIds;
using Riok.Mapperly.Abstractions;
using Sqids;

namespace Koworking.Api.Features.Users;

public partial record Koworker
{
    [Mapper(AutoUserMappings = false), RegisterSingleton]
    public partial class Mapper(TextIdEncoders encoders)
    {
        public SqidsEncoder<long> Encoder => encoders.Koworkers;
        
        [MapProperty(nameof(Id), nameof(Id), Use = nameof(MapId))]
        [MapperIgnoreSource(nameof(EqualityContract))]
        public partial Model ToModel(Koworker koworker);
        
        public partial IQueryable<Model> ProjectToModels(IQueryable<Koworker> queryable);

        private TextId MapId(long id) => Encoder.EncodeTextId(id);
    }
}