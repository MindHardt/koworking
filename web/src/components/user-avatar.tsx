import {User} from "@/routes/-auth/get-current-user.ts";
import {ComponentProps} from "react";
import {Avatar} from "@base-ui-components/react";


export default function UserAvatar({ user, imageProps, fallbackProps, ...props } : {
    user: User,
    imageProps?: Omit<ComponentProps<typeof Avatar.Image>, 'src'>,
    fallbackProps?: ComponentProps<typeof Avatar.Fallback>
} & ComponentProps<typeof Avatar.Root>) {
    const fallback = user.name
        .split(' ', 2)
        .map(x => x[0]?.toUpperCase())
        .join('');

    return <Avatar.Root {...props}>
        {user.avatar && <Avatar.Image src={user.avatar} {...imageProps}/>}
        <Avatar.Fallback {...fallbackProps}>{fallback}</Avatar.Fallback>
    </Avatar.Root>

}