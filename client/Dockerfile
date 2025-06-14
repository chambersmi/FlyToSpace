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
COPY --from=build /app/dist/client/browser /app
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN chmod -R 755 /app