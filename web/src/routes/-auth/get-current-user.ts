import {z} from "zod";
import {createServerFn} from "@tanstack/react-start";
import { getAuthTokens } from "./get-auth-tokens.ts";
import {getKoworkersMe} from "koworking-shared/api";
import { client } from "@/utils/backend.ts";


export const zUser = z.object({
    id: z.string(),
    avatar: z.url().nullable(),
    name: z.string(),
    email: z.email().nullable(),
});
export type User = z.infer<typeof zUser>;

export const zAuthenticationResult = z.object({
    accessToken: z.jwt(),
    user: zUser,
});
export type AuthenticationResult = z.infer<typeof zAuthenticationResult>;

export const getCurrentUser = createServerFn({ method: 'GET' })
    .handler(async () : Promise<AuthenticationResult | null> => {

        const { id_token, access_token } = await getAuthTokens();
        if (!id_token) {
            return null;
        }

        const idTokenPayload = id_token.split('.', 3)[1]!;
        const idTokenJson = Buffer.from(idTokenPayload, 'base64url').toString('utf-8');

        const { name, email } = z.object({
            name: z.string(),
            email: z.email().optional()
        }).parse(JSON.parse(idTokenJson));

        const meResponse = await getKoworkersMe({ client })
        if (!meResponse.data) {
            const error = meResponse.error;
            console.error('There was en error fetching user info from backend', error);
            throw error;
        }
        const { id, avatarUrl } = meResponse.data;

        return {
            accessToken: access_token!,
            user: {
                id,
                name,
                avatar: avatarUrl,
                email: email ?? null,
            }
        } satisfies AuthenticationResult
    })