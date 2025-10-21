import {Ellipsis, Search} from "lucide-react";
import {useDebouncedCallback} from "use-debounce";
import Button from "@/components/button.tsx";


export default function SearchBar({ initialSearch, searching, onSearch }: {
    initialSearch?: string,
    searching?: boolean,
    onSearch: (search: string) => void | Promise<void>
}) {

    const startSearch = useDebouncedCallback(onSearch, 500);

    return <div className='grid grid-cols-4 md:grid-cols-6 gap-5 w-full'>
        <Button className='hidden md:flex'>Сортировка</Button>
        <div className='p-1 rounded-4xl flex flex-row shadow col-span-4 h-16'>
            <Search className={'m-4' + (searching ? ' animate-ping text-main-500' : '')} />
            <input className='border-none h-full outline-none grow' placeholder='Введите...' type='text' defaultValue={initialSearch}
                   onChange={e => startSearch(e.target.value)} />
            <Button className='h-14 md:hidden w-1/4'><Ellipsis /></Button>
            <Button className='h-14 hidden md:flex w-1/4' onClick={startSearch.flush}>Поиск</Button>
        </div>
        <Button className='hidden md:flex'>Карта</Button>
    </div>
}