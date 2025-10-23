FROM node:24-slim AS base
ENV PNPM_HOME="/pnpm"
ENV PATH="$PNPM_HOME:$PATH"

RUN corepack enable

FROM base AS build
COPY . /usr/src/app
WORKDIR /usr/src/app
RUN --mount=type=cache,id=pnpm,target=/pnpm/store pnpm install --frozen-lockfile
RUN pnpm run -r --filter=koworking-shared build --outDir /prod/shared
RUN pnpm run -r --filter=!koworking-shared build
RUN pnpm deploy --filter=koworking-web --prod /prod/web

FROM base AS web
COPY --from=build /prod/web /prod/web
COPY --from=build /prod/shared/ /prod/web/node_modules/seadox-shared
WORKDIR /prod/web
EXPOSE 3000
CMD [ "node", ".output/server/index.mjs" ]