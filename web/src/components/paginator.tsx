import {Link, LinkProps, NavigateOptions} from "@tanstack/react-router";
import {ReactNode} from "react";
import {ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight} from "lucide-react";

type PaginatedResponse = {
    offset: number,
    limit: number,
    total: number
}

export default function Paginator({ response, getLink } : {
    response: PaginatedResponse,
    getLink: (page: number) => NavigateOptions
}) {
    const currentPage = Math.floor(response.offset / response.limit) + 1;
    const totalPages = Math.ceil(response.total / response.limit);
    const iterator = Array.from(Array(totalPages)).map((_, i) => i + 1);

    const firstPage = currentPage === 1;
    const lastPage = currentPage === totalPages;

    const PageLink = ({ page, children, ...props } : {
        page: number,
        children: ReactNode
    } & LinkProps) =>
        <Link
            className='size-8 outline-none flex justify-center items-center rounded-md '
            activeProps={{ className: 'text-main-600 bg-main-200' }}
            inactiveProps={{ className: 'text-main-500 hover:text-main-600 bg-main-50' }}
            {...props}
            {...getLink(page)}>
            {children}
        </Link>

    return <div className='w-full px-1'>
        <div className='flex flex-row flex-wrap justify-center gap-1 rounded-lg overflow-hidden'>
            <PageLink page={1} disabled={firstPage}><ChevronsLeft /></PageLink>
            <PageLink page={Math.max(currentPage - 1, 1)} disabled={firstPage}><ChevronLeft /></PageLink>
            {iterator.map(x => <PageLink key={x} page={x}>
                <span className='font-bold'>{x}</span>
            </PageLink>)}
            <PageLink page={Math.min(currentPage + 1, totalPages)} disabled={lastPage}><ChevronRight /></PageLink>
            <PageLink page={totalPages} disabled={lastPage}><ChevronsRight /></PageLink>
        </div>
    </div>
}