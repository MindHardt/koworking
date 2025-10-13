import {createFileRoute} from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import Loading from "@/components/loading.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client} from "@/utils/backend.ts";
import {useDeferredValue, useState} from "react";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
})

function RouteComponent() {

  const [search, setSearch] = useState('');
  const { data: vacancies, isPending } = useQuery({
    ...getVacanciesOptions({ client, query: { Search: search } }),
    select: res => res.data
  });
  const stale = useDeferredValue(vacancies);

  return <div className='p-4 flex flex-col gap-4'>
    <SearchBar onSearch={setSearch} />
    {isPending && <Loading />}
    <div className='flex flex-col gap-2 max-w-192 mx-auto'>
      {(vacancies ?? stale)?.map(v => <VacancyCard key={v.id} vacancy={v} />)}
    </div>
  </div>
}
