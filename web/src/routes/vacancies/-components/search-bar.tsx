import {Search} from "lucide-react";


export default function SearchBar() {

    return <div className='p-1 rounded-4xl flex flex-row shadow w-full h-16'>
        <Search className='m-4' />
        <input className='border-none m-4 outline-none grow' placeholder='Введите...' type='text' />
        <button className='bg-main outline-none h-14 w-30 rounded-4xl text-3xl text-white flex justify-center tracking-wider'>
            ...
        </button>
    </div>
}