FROM node:alpine AS build

ENV NODE_OPTIONS="--max-old-space-size=2048"

WORKDIR /app
COPY ./frontend/nippofrontend/package*.json ./
RUN npm install
COPY ./frontend/nippofrontend ./
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY ./Nginx/nginx.conf /etc/nginx
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]