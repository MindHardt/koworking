import {createFileRoute, useNavigate} from '@tanstack/react-router'
import {keepPreviousData, useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client} from "@/utils/backend.ts";
import ErrorMessage from "@/components/error-message.tsx";
import {z} from "zod";
import InfoMessage from "@/components/info-message.tsx";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
  validateSearch: z.object({
    search: z.string().optional()
  })
});

function RouteComponent() {

  const navigate = useNavigate({ from: Route.fullPath });
  const { search } = Route.useSearch();
  const { data: vacancies, isFetching, error } = useQuery({
    ...getVacanciesOptions({ client, query: { Search: search }}),
    select: res => res.data,
    placeholderData: keepPreviousData
  });

  return <div className='p-4 flex flex-col gap-4 items-center'>
    <SearchBar initialSearch={search} searching={isFetching} onSearch={search => navigate({ search: { search }})} />
    <div className='flex flex-col gap-2 max-w-192 mx-auto'>
      {error && <ErrorMessage error={error} />}
      {vacancies?.length === 0
          ? <InfoMessage title='Ничего не найдено' message='По вашему запросу мы ничего не нашли' />
          : vacancies?.map(v => <VacancyCard key={v.id} vacancy={v} />)}
    </div>
  </div>
}
