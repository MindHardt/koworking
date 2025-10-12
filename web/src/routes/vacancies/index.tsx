import {createFileRoute} from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import SearchBar from "@/routes/vacancies/-components/search-bar.tsx";
import Loading from "@/components/loading.tsx";
import VacancyCard from "@/routes/vacancies/-components/vacancy-card.tsx";
import {client} from "@/utils/backend.ts";

export const Route = createFileRoute('/vacancies/')({
  component: RouteComponent,
})

function RouteComponent() {

  const { data: vacancies } = useQuery({
    ...getVacanciesOptions({ client }),
    select: res => res.data
  });

  return <div className='p-4 flex flex-col gap-4'>
    <SearchBar />
    {vacancies
        ? <div className='flex flex-col gap-2 max-w-192 mx-auto'>
          {vacancies.map(v => <VacancyCard vacancy={v} />)}
        </div>
        : <Loading />}
  </div>
}
