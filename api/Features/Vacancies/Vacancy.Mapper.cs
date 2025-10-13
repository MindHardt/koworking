using Koworking.Api.Infrastructure.TextIds;
using Riok.Mapperly.Abstractions;

namespace Koworking.Api.Features.Vacancies;

public partial record Vacancy
{
    [Mapper(AutoUserMappings = false), RegisterSingleton]
    public partial class Mapper(TextIdEncoders encoders)
    {
        [MapperIgnoreSource(nameof(EqualityContract))]
        [MapProperty(nameof(Id), nameof(Id), Use = nameof(MapId))]
        public partial Model ToModel(Vacancy vacancy);

        public partial IQueryable<Model> ProjectToModels(IQueryable<Vacancy> query);

        private TextId MapId(long id) => encoders.Vacancies.EncodeTextId(id);
    }
}