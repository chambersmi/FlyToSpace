# Stage 1: Build Angular app
FROM node:24-slim AS build

WORKDIR /app
COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

RUN mkdir -p /usr/share/nginx/client && chmod -R 755 /usr/share/nginx/client
# Copy Angular build output
COPY --from=build /app/dist/client/browser /usr/share/nginx/client
