import {setCookie} from "@tanstack/react-start/server";
import {oauth} from "@/routes/-auth/oauth.ts";
import {keycloak} from "@/routes/-auth/keycloak.ts";
import {createFileRoute, redirect} from "@tanstack/react-router";
import { type CookieSerializeOptions } from "cookie-es";
import {z} from "zod";


export const Route = createFileRoute('/api/auth/signin')({
    validateSearch: z.object({
        returnUrl: z.url().optional()
    }),
    server: {
        handlers: {
            GET: async ({ request }) => {
                const redirectUri = new URL('/api/auth/callback', request.url);
                const cookieOpts : CookieSerializeOptions = {
                    httpOnly: true,
                    maxAge: 600,
                    path: '/',
                    sameSite: 'lax' as const,
                    secure: process.env.NODE_ENV === 'production',
                }

                const returnUrl = new URL(request.url).searchParams.get('returnUrl');
                if (returnUrl) {
                    setCookie('returnUrl', returnUrl, cookieOpts);
                }

                const { codeVerifier, codeChallenge } = oauth.generateCodeVerifier();
                setCookie('codeVerifier', codeVerifier, cookieOpts);

                const state = oauth.generateState();
                setCookie('state', state, cookieOpts);

                const authUrl = keycloak.buildAuthorizationUrl({
                    state,
                    codeChallenge,
                    redirectUri
                });
                return redirect({ href: authUrl.href });
            }
        }
    }
})