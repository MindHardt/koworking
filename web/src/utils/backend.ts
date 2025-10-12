import {createClient} from "koworking-shared/api/client";
import * as process from "node:process";


export const client = createClient({
    baseUrl: process.env?.BACKEND_URL ?? '/api'
})