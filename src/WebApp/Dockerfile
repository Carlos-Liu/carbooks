# Build the SPA, then serve the static output from Nginx. Nginx also reverse-proxies /api to the
# API container so the browser only ever sees one origin.
FROM node:22-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM nginx:1.29-alpine AS runtime

# API_HOST lets compose point the proxy at the API service without rebuilding the image.
ENV API_HOST=http://carbooks-api:8080

COPY nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 8080
