import {Pencil} from "lucide-react";
import {ComponentProps} from "react";
import {cn} from "@/utils/cn.ts";


export default function Loading({ className, ...props } : ComponentProps<typeof Pencil>) {
    return <Pencil className={cn('text-main animate-bounce', className)} {...props} />
}