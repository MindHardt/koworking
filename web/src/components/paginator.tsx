import {Link, LinkProps, NavigateOptions} from "@tanstack/react-router";
import {ReactNode} from "react";

type PaginatedResponse = {
    offset: number,
    limit: number,
    total: number
}

export default function Paginator({ response, getLink } : {
    response: PaginatedResponse,
    getLink: (page: number) => NavigateOptions
}) {
    const totalPages = Math.ceil(response.total / response.limit);
    const iterator = Array.from(Array(totalPages)).map((_, i) => i + 1);

    const PageLink = ({ page, children, ...props } : {
        page: number,
        children: ReactNode
    } & LinkProps) =>
        <Link
            className='size-14 outline-none flex justify-center items-center transition-[color]'
            activeProps={{ className: 'text-main-50 font-bold' }}
            inactiveProps={{ className: 'text-main-200 hover:text-main-50' }}
            {...props}
            {...getLink(page)}>
            {children}
        </Link>

    return <div className='flex flex-row flex-wrap justify-center gap-1 px-15 rounded-4xl bg-main-500'>
        {iterator.map(x => <PageLink key={x} page={x}>
            <span>{x}</span>
        </PageLink>)}
    </div>
}