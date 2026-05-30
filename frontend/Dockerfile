FROM node:21.5.0-alpine3.19 AS build
WORKDIR /src
COPY . .
RUN rm package-lock.json
RUN npm i
RUN npm run build

FROM nginx:1.25.3-alpine3.18 AS base
WORKDIR /app
EXPOSE 80
COPY --from=build /src/dist .
COPY nginx.conf /etc/nginx/nginx.conf
ENTRYPOINT ["nginx", "-g", "daemon off;"]