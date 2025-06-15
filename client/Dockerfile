# Stage 1: Build Angular app
FROM node:24-slim AS build

WORKDIR /app

# Copy only package.json and package-lock.json first (for caching)
COPY package*.json ./

# Install dependencies inside container for Linux
RUN npm install --legacy-peer-deps

# Now copy rest of the source files
COPY . .

# Build Angular app in production mode
RUN npm run build -- --configuration production

# Stage 2: Serve with nginx
FROM nginx:alpine
RUN rm -rf /usr/share/nginx/html
# Remove the default.conf if it causes issues, before copying our own
# This is a precaution; Nginx will pick up our new .conf file anyway.
RUN rm -f /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist/client/browser /app/ 
RUN chown -R www-data:www-data /app && chmod -R 755 /app
COPY flytospace-client.conf /etc/nginx/conf.d/flytospace-client.conf
RUN chmod -R 755 /app 