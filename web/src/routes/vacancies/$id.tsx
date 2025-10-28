import {createFileRoute, useLinkProps} from '@tanstack/react-router'
import {useQuery} from "@tanstack/react-query";
import {getVacanciesByIdOptions} from "koworking-shared/api/@tanstack/react-query.gen.ts";
import {client} from "@/utils/backend.ts";
import ErrorMessage from "@/components/error-message.tsx";
import Loading from "@/components/loading.tsx";
import Card from "@/components/card.tsx";
import Markdown, {Components} from "react-markdown";
import {cn} from "@/utils/cn.ts";
import {getVacanciesById} from "koworking-shared/api";
import {seo} from "@/utils/seo.ts";
import ClipboardButton from "@/components/clipboard-button.tsx";
import {Share} from "lucide-react";
import {useCallback} from "react";

export const Route = createFileRoute('/vacancies/$id')({
    component: RouteComponent,
    loader: async ({ params }) => ({
        vacancy: await getVacanciesById({ client, path: { Id: params.id }}).then(x => x.data)
    }),
    head: (ctx) => {
        if (!ctx.loaderData?.vacancy) {
            return {}
        }

        const { title, imageUrl } = ctx.loaderData.vacancy;
        return {
            meta: seo({ title, image: imageUrl ?? undefined })
        }
    }
})

const components: Components = {
    ol: ({ className, ...rest })=>
        <ol className={cn('list-decimal ms-4', className)} {...rest}></ol>
}
function RouteComponent() {

    const { id } = Route.useParams();
    const loader = Route.useLoaderData();
    const { data: vacancy, error } = useQuery({
        ...getVacanciesByIdOptions({ throwOnError: true, client, path: { Id: id } }),
        initialData: loader.vacancy
    });
    const { href } = useLinkProps({
        to: '/vacancies/$id',
        params: { id },
        search: {
            utm_source: 'koworking',
            utm_campaign: 'referral',
            utm_medium: 'copy_link'
        }
    });
    const getShareUrl = useCallback(() =>
            new URL(href!, window.location.href).toString(), [href]);

    if (error) {
        return <ErrorMessage error={error} />
    }
    if (!vacancy) {
        return <Loading className='py-5' />
    }

    return <div className='flex flex-col gap-3 p-5 max-w-xl mx-auto'>
        {vacancy.imageUrl && <Card className='px-0 py-0'>
            <img src={vacancy.imageUrl} alt={vacancy.title} className="w-full rounded-2xl object-fill" />
        </Card>}
        <div className='flex flex-row gap-1'>
            <h1 className='text-2xl font-semibold'>{vacancy.title}</h1>
            <ClipboardButton getText={getShareUrl} icon={<Share />} className='w-24 h-12' />
        </div>
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
