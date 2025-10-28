import {z} from "zod";
import {postVisits} from "koworking-shared/api";
import {client} from "@/utils/backend.ts";
import {redirect} from "@tanstack/react-router";


export const zUtmParams = z.object({
    utm_campaign: z.string(),
    utm_medium: z.string(),
    utm_source: z.string(),
    utm_content: z.string().optional(),
    utm_term: z.string().optional()
});
export const zUtmParamsPartial = zUtmParams.partial();

export async function processUtm(params: z.infer<typeof zUtmParamsPartial>, location: string) {
    if (Object.entries(zUtmParamsPartial.parse(params)).length === 0) {
        return;
    }

    const { success, data: utm } = zUtmParams.safeParse(params);
    if (success) {
        await postVisits({ client, body: { ...utm, location }});
    }

    throw redirect({ search: (prev) => ({
            ...prev,
            utm_source: undefined,
            utm_medium: undefined,
            utm_term: undefined,
            utm_content: undefined,
            utm_campaign: undefined
        })});
}