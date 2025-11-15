import {createClient} from "koworking-shared/api/client";
import * as process from "node:process";
import {getAuthTokens} from "@/routes/-auth/get-auth-tokens.ts";
import {UploadModel} from "koworking-shared/api";


const client = createClient({
    baseUrl: process.env?.BACKEND_URL ?? '/api',
});
if (import.meta.env.SSR) {
    client.interceptors.request.use(appendAccessToken);
}

async function appendAccessToken(request: Request) {
    const { access_token } = await getAuthTokens();
    if (access_token) {
        request.headers.set('Authorization', `Bearer ${access_token}`);
    }
    return request;
}

const pagination = (page: number, pageSize = 10)=> ({
    Limit: pageSize,
    Offset: (page - 1) * pageSize
});

const uploadUrl = (upload: UploadModel) => '/api/uploads/' + upload.id;

export { client, pagination, uploadUrl };