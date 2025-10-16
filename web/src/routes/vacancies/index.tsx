import {createFileRoute, linkOptions, LinkOptions, useNavigate} from '@tanstack/react-router'
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client, pagination} from "@/utils/backend.ts";
import ErrorMessage from "@/components/error-message.tsx";
import {z} from "zod";
import InfoMessage from "@/components/info-message.tsx";
import {useCallback} from "react";
import Paginator from "@/components/paginator.tsx";
import {getVacancies} from "koworking-shared/api";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
  validateSearch: z.object({
    search: z.string().optional(),
    page: z.int().min(1).optional().default(1)
  }),
  beforeLoad: async ({ search }) => {
    return getVacancies({ client, query: { Search: search.search, ...pagination(search.page) } })
        .then(x => ({ vacancies: x.data }));
  }
});

function RouteComponent() {

  const navigation = linkOptions({ from: Route.fullPath });
  const navigate = useNavigate(navigation);
  const { search, page } = Route.useSearch();
  const { data: res, isFetching, error } = useQuery({
    ...getVacanciesOptions({ client, query: { Search: search, ...pagination(page) }}),
    placeholderData: keepPreviousData,
    staleTime: 30 * 1000,
    initialData: Route.useRouteContext().vacancies
  });
  const setSearch = useCallback(async (search: string) => {
    await navigate({ search: prev => ({ ...prev, page: 1, search: search.trim() })})
  }, []);
  const getPage = useCallback((page: number) => ({
    ...navigation,
    search: prev => ({ ...prev, page })
  } satisfies LinkOptions), [])


  const vacancies = res?.data;

  return <div className='p-4 flex flex-col gap-4 items-center'>
    <SearchBar initialSearch={search} searching={isFetching} onSearch={setSearch} />
    {res && <Paginator response={res} getLink={getPage} />}
    <div className='flex flex-col gap-2 max-w-192 mx-auto'>
      {error && <ErrorMessage error={error} />}
      {vacancies?.length === 0
          ? <InfoMessage title='Ничего не найдено' message='По вашему запросу мы ничего не нашли' />
          : vacancies?.map(v => <VacancyCard key={v.id} vacancy={v} />)}
    </div>
    {res && <Paginator response={res} getLink={getPage} />}
  </div>
}
