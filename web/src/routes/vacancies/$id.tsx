import { createFileRoute } from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesByIdOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import {client} from "@/utils/backend.ts";
import ErrorMessage from "@/components/error-message.tsx";
import Loading from "@/components/loading.tsx";

export const Route = createFileRoute('/vacancies/$id')({
    component: RouteComponent
})

function RouteComponent() {

    const { id } = Route.useParams();
    const { data: vacancy, error } = useQuery({
        ...getVacanciesByIdOptions({ throwOnError: true, client, path: { Id: id } })
    })

    if (error) {
        return <ErrorMessage error={error} />
    }
    if (!vacancy) {
        return <Loading className='py-5' />
    }
    return <div className='flex flex-col gap-3 p-5'>
        {vacancy.imageUrl && <img src={vacancy.imageUrl} alt={vacancy.title} className="w-full max-h-64 rounded-2xl object-fill" />}
        <h1 className='text-2xl font-semibold'>{vacancy.title}</h1>
        <p>{vacancy.text}</p>
    </div>
}
