import {ComponentProps} from "react";
import {cn} from "@/utils/cn.ts";


export default function Card({ className, children, grow = true, ...rest } : {
    grow?: boolean
} & ComponentProps<'div'>) {
    return <div className={cn(
        'w-full rounded-4xl shadow-md flex flex-col gap-0 px-4 py-8 transition-[scale]',
        (grow ? 'hover:scale-105' : ''),
        className)} {...rest}>
        {children}
    </div>
}