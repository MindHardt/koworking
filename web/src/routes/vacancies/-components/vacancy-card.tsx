import {VacancyModel} from "koworking-shared/api";
import {Bookmark, MapPin} from "lucide-react";
import {Link} from "@tanstack/react-router";

export default function VacancyCard({ vacancy } : {
    vacancy: VacancyModel
}) {

    return <Link to='/vacancies/$id' params={{ id: vacancy.id }}
        className='w-full rounded-4xl shadow-xl flex flex-col gap-0 transition-[scale] hover:scale-105'>
        {vacancy.imageUrl && <div className='h-50 w-full'>
            <img className='size-full rounded-t-4xl' src={vacancy.imageUrl} alt={vacancy.title} />
        </div>}
        <div className='flex flex-row gap-1 p-4'>
            <div className='flex flex-col gap-1 grow'>
                <h3>{vacancy.title}</h3>
                <div className='flex flex-row items-center gap-1'>
                    <MapPin />
                    <span className='text-xs'>{vacancy.location}</span>
                </div>
            </div>
            <div className='shrink flex items-center'>
                <Bookmark className='text-main-500' />
            </div>
        </div>
    </Link>
}