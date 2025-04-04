FROM node:22-alpine AS build
WORKDIR /app

RUN npm install --global corepack@latest
RUN corepack enable
RUN corepack prepare pnpm@latest-10 --activate

COPY package.json pnpm-lock.yaml pnpm-workspace.yaml .npmrc ./

COPY . ./

RUN pnpm install

RUN pnpm --filter checkout -r build


FROM node:22-alpine
WORKDIR /app

COPY --from=build /app/src/CheckoutApp/.output/ ./

ENV PORT=80
ENV HOST=0.0.0.0

EXPOSE 80

CMD ["node", "/app/server/index.mjs"]
