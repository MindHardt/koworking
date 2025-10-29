import {z} from "zod";
import {postVisits} from "koworking-shared/api";
import {client} from "@/utils/backend.ts";
import {useNavigate, useRouterState} from "@tanstack/react-router";
import {useEffect} from "react";


export const zUtmParams = z.object({
    utm_campaign: z.string(),
    utm_medium: z.string(),
    utm_source: z.string(),
    utm_content: z.string().optional(),
    utm_term: z.string().optional(),
});
export const zUtmParamsPartial = zUtmParams.partial();

export async function processUtm(params: z.infer<typeof zUtmParamsPartial>, location: string) {
    if (Object.entries(zUtmParamsPartial.parse(params)).length === 0) {
        return;
    }

    const { success, data: utm } = zUtmParams.safeParse(params);
    if (success) {
        const { userAgent } = navigator;
        await postVisits({ client, body: { ...utm, location, userAgent }});
    }
}

// A hook that processes utm search params, registers a visit and then strips them
export function useUtmSearch() {
    const { location } = useRouterState();
    const navigate = useNavigate();
    useEffect(() => {
        (async () => {
            await processUtm(location.search, location.pathname);
            await navigate({
                to: '.',
                search: (prev) => ({
                    ...prev,
                    utm_content: undefined,
                    utm_source: undefined,
                    utm_term: undefined,
                    utm_medium: undefined,
                    utm_campaign: undefined
                })
            });
        })();
    }, []);
}