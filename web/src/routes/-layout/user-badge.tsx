import {User} from "@/routes/-auth/get-current-user.ts";
import Button from "@/components/button.tsx";
import UserAvatar from "@/components/user-avatar.tsx";
import {Menu} from "@base-ui-components/react";
import {Link, useNavigate} from "@tanstack/react-router";
import {useCallback} from "react";
import {DoorClosed, User as UserIcon} from "lucide-react";

export default function UserBadge({ user } : {
    user: User
}) {

    const navigate = useNavigate();
    const redirectToSignout = useCallback(async () => {
        await navigate({ to: '/api/auth/signout', reloadDocument: true, search: { returnUrl: window.location.href }});
    }, [])

    return <Menu.Root>
        <Menu.Trigger className='flex flex-row gap-1'>
            <UserAvatar
                user={user}
                className='size-12 hover:filter-[brightness(0.8)] transition-[filter]'
                imageProps={{ className: 'h-full rounded-4xl' }}
                fallbackProps={{ className: 'block h-full rounded-4xl bg-main-200 text-main-800 flex justify-center items-center text-xl font-bold' }} />
            <Button className='h-full w-32 hidden md:inline'>{user.name}</Button>
        </Menu.Trigger>
        <Menu.Portal>
            <Menu.Positioner>
                <Menu.Popup className={
                    'w-64 bg-white m-2 p-2 border-2 border-gray-200 rounded-2xl transition-[scale,opacity] duration-150 shadow ' +
                    'data-ending-style:opacity-0 data-starting-style:opacity-0 ' +
                    'data-ending-style:scale-90 data-starting-style:scale-90 ' +
                    'scale-100 opacity-100 flex flex-col gap-1 text-lg font-semibold'
                }>
                    <Menu.Item>
                        <Link to='/profile' className='hover:bg-main-100 rounded p-2 flex flex-row gap-1'>
                            <UserIcon />
                            Профиль
                        </Link>
                    </Menu.Item>
                    <Menu.Separator className='bg-main-200 h-[1px] my-1' />
                    <Menu.Item
                        className='hover:bg-main-100 rounded p-2 flex flex-row gap-1'
                        role='button' onClick={redirectToSignout}>
                        <DoorClosed />
                        Выйти
                    </Menu.Item>
                </Menu.Popup>
            </Menu.Positioner>
        </Menu.Portal>
    </Menu.Root>
}