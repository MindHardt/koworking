import { createFileRoute } from '@tanstack/react-router';
import {Hono} from "hono";
import {proxy} from "hono/proxy";
import {handle} from "hono/vercel";
import {getAuthTokens} from "@/routes/-auth/get-auth-tokens.ts";

export const Route = createFileRoute('/api/$')({
  server: {
    handlers: {
        GET: proxyRequest,
        POST: proxyRequest,
        PUT: proxyRequest,
        DELETE: proxyRequest,
        PATCH: proxyRequest,
        OPTIONS: proxyRequest,
    }
  }
})

const backendUrl = new URL(process.env.BACKEND_URL!);
const proxyServer = new Hono();
proxyServer.all('*', async ({ req }) => {
    const requestUrl = new URL(req.url);
    requestUrl.protocol = backendUrl.protocol;
    requestUrl.host = backendUrl.host;

    const { method, body, headers,} = req.raw;

    return proxy(requestUrl, {
        method, body, headers,
        // @ts-expect-error
        duplex: 'half',
        signal: null
    });
})

async function proxyRequest({ request }: { request: Request }): Promise<Response> {


    const { access_token } = await getAuthTokens();
    if (access_token) {
        request.headers.set('Authorization', 'Bearer ' + access_token);
    }

    return handle(proxyServer)(request);
}
