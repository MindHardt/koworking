import {Search} from "lucide-react";
import {useDebouncedCallback} from "use-debounce";


export default function SearchBar({ onSearch }: {
    onSearch: (search: string) => void | Promise<void>;
}) {

    const startSearch = useDebouncedCallback(onSearch, 200);

    return <div className='p-1 rounded-4xl flex flex-row shadow w-full h-16'>
        <Search className='m-4' />
        <input className='border-none m-4 outline-none grow' placeholder='Введите...' type='text'
               onChange={e => startSearch(e.target.value)} />
        <button className='bg-main outline-none h-14 w-30 rounded-4xl text-3xl text-white flex justify-center tracking-wider'>
            ...
        </button>
    </div>
}