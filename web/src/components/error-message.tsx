import {ComponentProps} from "react";
import {CircleAlert} from "lucide-react";
import {cn} from "@/utils/cn.ts";

export default function ErrorMessage({ error, className, ...props } : {
    error: Error
} & ComponentProps<'div'>) {

    return <div className={cn('max-w-80 text-red-500 mx-auto flex flex-col gap-1 items-center border border-red-500 p-2 rounded-xl text-center', className)} {...props}>
        <CircleAlert className='animate-pulse' />
        <h3 className='text-lg font-semibold'>Произошла ошибка</h3>
        <span className='font-mono'>{error.message}</span>
    </div>
}