import {Search} from "lucide-react";
import {useDebouncedCallback} from "use-debounce";


export default function SearchBar({ searching, onSearch }: {
    searching?: boolean,
    onSearch: (search: string) => void | Promise<void>
}) {

    const startSearch = useDebouncedCallback(onSearch, 500);

    return <div className='p-1 rounded-4xl flex flex-row shadow w-full h-16'>
        <Search className={'m-4' + (searching ? ' animate-ping text-main-500' : '')} />
        <input className='border-none h-full outline-none grow' placeholder='Введите...' type='text'
               onChange={e => startSearch(e.target.value)} />
        <button className='bg-main-500 hover:bg-main-600 outline-none h-14 w-30 rounded-4xl text-3xl text-white flex justify-center tracking-wider'>
            ...
        </button>
    </div>
}