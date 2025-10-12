import {VacancyModel} from "koworking-shared/api";
import {Bookmark, MapPin} from "lucide-react";


export default function VacancyCard({ vacancy } : {
    vacancy: VacancyModel
}) {

    const content = <div className='flex flex-row gap-1 p-4'>
        <div className='flex flex-col gap-1 grow'>
            <h3>{vacancy.name}</h3>
            <div className='flex flex-row items-center gap-1'>
                <MapPin />
                <span className='text-xs'>{vacancy.location}</span>
            </div>
        </div>
        <div className='shrink flex items-center'>
            <Bookmark className='text-main' />
        </div>
    </div>;

    return <div className={'w-full rounded-4xl shadow flex flex-col gap-0' + (vacancy.imageUrl ? 'h-96' : 'h-36')}>
        {vacancy.imageUrl && <div className='h-50 overflow-hidden'>
            <img className='size-full rounded-t-4xl' src={vacancy.imageUrl} alt={vacancy.name} />
        </div>}
        {content}
    </div>
}