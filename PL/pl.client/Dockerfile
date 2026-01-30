# Stage 1: Build
FROM node:18-alpine AS build
WORKDIR /app

# Especificamos la ruta desde la raíz (Contexto del compose)
COPY PL/pl.client/package*.json ./
RUN npm install

# Copiamos todo el contenido de la carpeta del cliente
COPY PL/pl.client/ ./
RUN npm run build

# Stage 2: Serve con Nginx
FROM nginx:alpine
# IMPORTANTE: Revisa si en tu carpeta 'dist' se crea una subcarpeta 
# (ej. /app/dist/pl.client). Si es así, ajusta la ruta abajo.
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
