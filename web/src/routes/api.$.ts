import { createFileRoute } from '@tanstack/react-router';
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

    const requestUrl = new URL(request.url);
    requestUrl.protocol = backendUrl.protocol;
    requestUrl.host = backendUrl.host;

    const proxiedRequest = new Request(requestUrl, {
        ...request,
        method: request.method,
        redirect: 'manual',
        headers: new Headers(request.headers)
    });
    proxiedRequest.headers.delete('Host');

    try {
        return await fetch(proxiedRequest);
    } catch (error) {
        return Response.json({ error: 'cannot proxy request to backend' }, {
            status: 503
        })
    }
}
