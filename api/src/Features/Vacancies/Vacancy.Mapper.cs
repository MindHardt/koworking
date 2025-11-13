using Koworking.Api.Infrastructure.TextIds;
using Riok.Mapperly.Abstractions;
using Sqids;

namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy
{
    [Mapper(AutoUserMappings = false), RegisterSingleton]
    public partial class Mapper(TextIdEncoders encoders)
    {
        public SqidsEncoder<long> Encoder => encoders.Vacancies;
        
        [MapperIgnoreSource(nameof(EqualityContract))]
        [MapProperty(nameof(Id), nameof(Id), Use = nameof(MapId))]
        public partial Model ToModel(Vacancy vacancy);

        public partial IQueryable<Model> ProjectToModels(IQueryable<Vacancy> query);

        private TextId MapId(long id) => Encoder.EncodeTextId(id);
    }
}