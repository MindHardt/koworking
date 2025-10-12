import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/api/$')({
  server: {
    handlers: {
        GET: proxyRequest,
        POST: proxyRequest,
        PUT: proxyRequest,
        DELETE: proxyRequest,
        PATCH: proxyRequest
    }
  }
})

const backendUrl = new URL(process.env.BACKEND_URL!);
async function proxyRequest({ request }: { request: Request }): Promise<Response> {
    // const { access_token } = await getAuthTokens();
    // if (access_token) {
    //     request.headers.set('Authorization', 'Bearer ' + access_token);
    // }

    const requestUrl = new URL(request.url);
    requestUrl.protocol = backendUrl.protocol;
    requestUrl.host = backendUrl.host;
    requestUrl.pathname = requestUrl.pathname.startsWith('/api')
        ? requestUrl.pathname.substring(4)
        : requestUrl.pathname;

    const proxiedRequest = new Request(requestUrl, request);
    return await fetch(proxiedRequest);
}
