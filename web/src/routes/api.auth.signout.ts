
import {createFileRoute, redirect} from '@tanstack/react-router'
import z from 'zod'
import {keycloak} from "@/routes/-auth/keycloak.ts";
import {clearTokens, retrieveTokens} from "@/routes/-auth/persistence.ts";

const zSearch = z.object({
    returnUrl: z.string().optional()
});
export const Route = createFileRoute('/api/auth/signout')({
    validateSearch: zSearch,
    server: {
        handlers: {
            GET: async ({ request }) => {
                const requestUrl = new URL(request.url);
                const { returnUrl } = zSearch.parse(Object.fromEntries(requestUrl.searchParams.entries()));

                const { refresh_token } = retrieveTokens();
                if (refresh_token) {
                    await keycloak.revokeTokens({ refreshToken: refresh_token });
                }

                clearTokens();
                throw redirect({ href: returnUrl ?? '/' });
            }
        }
    }
})
