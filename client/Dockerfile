FROM node:24-slim AS build

WORKDIR /app

COPY package*.json ./
RUN npm install --legacy-peer-deps
COPY . .
RUN npm run build -- --configuration production

FROM nginx:alpine
RUN rm -rf /usr/share/nginx/html
RUN rm -f /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/client/browser /app/
RUN chown -R nginx:nginx /app && chmod -R 755 /app
COPY flytospace-client.conf /etc/nginx/conf.d/flytospace-client.conf
RUN chmod -R 755 /app