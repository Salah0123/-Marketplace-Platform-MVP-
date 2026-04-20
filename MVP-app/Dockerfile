# Stage 1: Build
FROM node:22-alpine AS build

WORKDIR /app

COPY package*.json ./

# ← بدل npm ci استخدم npm install
RUN npm install --legacy-peer-deps

COPY . .
RUN npm run build

# Stage 2: Runtime
FROM node:22-alpine

WORKDIR /app

COPY --from=build /app/dist ./dist

ENV PORT=4000
ENV NODE_ENV=production

EXPOSE 4000

CMD ["node", "dist/MVP-app/server/server.mjs"]