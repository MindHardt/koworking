import {User} from "@/routes/-auth/get-current-user.ts";
import Button from "@/components/button.tsx";
import UserAvatar from "@/components/user-avatar.tsx";


export default function UserBadge({ user } : {
    user: User
}) {

    return <div className='flex flex-row gap-1' role='button'>
        <UserAvatar
            user={user}
            className='size-12'
            imageProps={{ className: 'h-full rounded-4xl hover:filter-[brightness(0.8)]' }} />
        <Button className='h-full w-32 hidden md:inline'>Профиль</Button>
    </div>;
}