import { createFileRoute } from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesByIdOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import {client} from "@/utils/backend.ts";
import ErrorMessage from "@/components/error-message.tsx";
import Loading from "@/components/loading.tsx";
import Card from "@/components/card.tsx";
import Markdown, {Components} from "react-markdown";
import {cn} from "@/utils/cn.ts";

export const Route = createFileRoute('/vacancies/$id')({
    component: RouteComponent
})

const components: Components = {
    ol: ({ className, ...rest })=>
        <ol className={cn('list-decimal ms-4', className)} {...rest}></ol>
}
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

    return <div className='flex flex-col gap-3 p-5 max-w-lg mx-auto'>
        {vacancy.imageUrl && <Card className='px-0 py-0'>
            <img src={vacancy.imageUrl} alt={vacancy.title} className="w-full rounded-2xl object-fill" />
        </Card>}
        <h1 className='text-2xl font-semibold'>{vacancy.title}</h1>
        <Card>
            <h2 className='text-xl font-semibold'>Описание работы</h2>
            <Markdown components={components}>{vacancy.description}</Markdown>
        </Card>
        <Card>
            <h2 className='text-xl font-semibold'>Предлагаем</h2>
            <Markdown components={components}>{vacancy.conditions}</Markdown>
        </Card>
        <Card>
            <h2 className='text-xl font-semibold'>Требования к работе</h2>
            <Markdown components={components}>{vacancy.expectations}</Markdown>
        </Card>
    </div>
}
