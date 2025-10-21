import {ComponentProps} from "react";
import {cn} from "@/utils/cn.ts";


export default function Button({ className, children, ...props } : ComponentProps<'button'>) {
    return <button
        className={cn('bg-main-500 hover:bg-main-600 outline-none h-16 w-full rounded-4xl text-white flex justify-center items-center', className)}
        {...props}>
        {children}
    </button>
}