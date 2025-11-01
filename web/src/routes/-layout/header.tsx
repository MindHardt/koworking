import {useQuery} from "@tanstack/react-query";
import currentUserOptions from "@/routes/-auth/current-user-options.ts";
import Button from "@/components/button.tsx";
import {Link, useNavigate} from "@tanstack/react-router";
import {useCallback} from "react";
import {CircleUserRound} from "lucide-react";
import UserBadge from "@/routes/-layout/user-badge.tsx";

export default function Header() {

    const { data: user } = useQuery({
        ...currentUserOptions()
    });
    const navigate = useNavigate();
    const redirectToLogin = useCallback(async () => {
        await navigate({ to: '/api/auth/signin', search: { returnUrl: window.location.href }})
    }, []);

    return <header className='bg-main-700 shadow-md w-full h-16 p-2 flex flex-row gap-2 justify-between'>
        <Link className='h-12' to='/'>
            <img className='size-12 rounded transition-[scale] hover:scale-105' src='/logo192.png' alt='logo' />
        </Link>
        {user
            ? <UserBadge user={user} />
            : <Button className='w-32 h-full' onClick={redirectToLogin}>
                <CircleUserRound />
                <span className='hidden md:inline'>Войти</span>
            </Button>}
    </header>

}