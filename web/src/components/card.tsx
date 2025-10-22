import {ComponentProps} from "react";
import {cn} from "@/utils/cn.ts";


export default function Card({ className, children, ...rest } : ComponentProps<'div'>) {
    return <div className={cn(className, 'w-full rounded-4xl shadow-md flex flex-col gap-0 px-4 py-8')} {...rest}>
        {children}
    </div>
}