import { createFileRoute } from '@tanstack/react-router';

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
async function proxyRequest({ request }: { request: Request }): Promise<Response> {

    const requestUrl = new URL(request.url);
    requestUrl.protocol = backendUrl.protocol;
    requestUrl.host = backendUrl.host;

    const proxiedRequest = new Request(requestUrl, {
        ...request,
        body: request.body,
        method: request.method,
        // @ts-expect-error
        duplex: 'half',
        redirect: 'manual',
        headers: new Headers(request.headers)
    });
    proxiedRequest.headers.delete('Host');
    console.log('Proxying request', proxiedRequest)

    try {
        const res = await fetch(proxiedRequest);
        console.log('Proxied response', res);
        return res;
    } catch (err) {
        console.error('There was an error proxying request', { err });
        return Response.json({ error: 'cannot proxy request to backend' }, {
            status: 503
        })
    }
}
