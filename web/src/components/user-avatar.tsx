import {User} from "@/routes/-auth/get-current-user.ts";
import {ComponentProps} from "react";
import {Avatar} from "@base-ui-components/react";
import {cn} from "@/utils/cn.ts";


export default function UserAvatar({ user, className, imageProps, fallbackProps, ...props } : {
    user: User,
    imageProps?: Omit<ComponentProps<typeof Avatar.Image>, 'src'>,
    fallbackProps?: ComponentProps<typeof Avatar.Fallback>
} & ComponentProps<typeof Avatar.Root>) {

    const letters = user.name
        .split(' ', 2)
        .map(x => x[0]?.toUpperCase())
        .join('');
    const { className: imageClass, ...imageRest } = imageProps ?? {};

    return <Avatar.Root
        className={cn('flex justify-center items-center w-full bg-main-200 text-main-800 text-2xl font-semibold rounded', className)}
        {...props}>
        {user.avatar ? <>
            <Avatar.Image
                src={user.avatar}
                className={cn('w-full', imageClass)}
                {...imageRest}/>
            <Avatar.Fallback {...fallbackProps}>{letters}</Avatar.Fallback>
        </> : <span>{letters}</span>}
    </Avatar.Root>

}