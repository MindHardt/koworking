import {ComponentProps} from "react";
import {CircleAlert} from "lucide-react";
import {cn} from "@/utils/cn.ts";

export default function ErrorMessage({ error, className, ...props } : {
    error: Error
} & ComponentProps<'div'>) {

    return <div className={cn('max-w-80 text-red-500 mx-auto flex flex-col gap-1 items-center', className)} {...props}>
        <CircleAlert />
        <h3>Произошла ошибка</h3>
        <span>{error.message}</span>
    </div>
}