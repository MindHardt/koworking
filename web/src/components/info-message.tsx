import {ComponentProps, JSX} from "react";
import {Info} from "lucide-react";
import {cn} from "@/utils/cn.ts";

export default function InfoMessage({ icon, title, message, className, ...props } : {
    icon?: JSX.Element,
    title: string,
    message?: string
} & ComponentProps<'div'>) {

    return <div className={cn('max-w-80 text-main-500 mx-auto flex flex-col gap-1 items-center border border-main-500 p-2 rounded-xl text-center', className)} {...props}>
        {icon ?? <Info className='animate-pulse' />}
        <h3 className='text-lg font-semibold'>{title}</h3>
        {message && <span className='font-mono'>{message}</span>}
    </div>
}