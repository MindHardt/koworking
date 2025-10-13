import {createFileRoute} from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client} from "@/utils/backend.ts";
import {useDeferredValue, useState} from "react";
import {CircleAlert} from "lucide-react";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
})

function RouteComponent() {

  const [search, setSearch] = useState('');
  const { data: vacancies, isPending, error } = useQuery({
    ...getVacanciesOptions({ client, query: { Search: search } }),
    select: res => res.data
  });
  const stale = useDeferredValue(vacancies);

  return <div className='p-4 flex flex-col gap-4 items-center'>
    <SearchBar searching={isPending} onSearch={setSearch} />
    <div className='flex flex-col gap-2 max-w-192 mx-auto'>
      {error && <div className='flex flex-col gap-2 items-center text-red-500'>
        <CircleAlert />
        <h3 className='font-mono'>{error.message}</h3>
      </div>}
      {(vacancies ?? stale)?.map(v => <VacancyCard key={v.id} vacancy={v} />)}
    </div>
  </div>
}
