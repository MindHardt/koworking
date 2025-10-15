import {createFileRoute, useNavigate} from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client} from "@/utils/backend.ts";
import {useEffect, useRef} from "react";
import ErrorMessage from "@/components/error-message.tsx";
import {z} from "zod";
import {PaginatedResponseOfVacancyModel,} from "koworking-shared/api";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
  validateSearch: z.object({
    search: z.string().optional()
  })
})

function RouteComponent() {

  const navigate = useNavigate({ from: Route.fullPath });
  const { search } = Route.useSearch();
  const previous = useRef<PaginatedResponseOfVacancyModel>(null);
  const { data: vacancies, isFetching, error } = useQuery({
    ...getVacanciesOptions({ client, query: { Search: search } }),
    placeholderData: previous.current ?? undefined
  });
  useEffect(() => {
    previous.current = vacancies ?? previous.current;
  }, [vacancies]);

  return <div className='p-4 flex flex-col gap-4 items-center'>
    <SearchBar initialSearch={search} searching={isFetching} onSearch={search => navigate({ search: { search }})} />
    <div className='flex flex-col gap-2 max-w-192 mx-auto'>
      {error && <ErrorMessage error={error} />}
      {vacancies?.data.map(v => <VacancyCard key={v.id} vacancy={v} />)}
    </div>
  </div>
}
