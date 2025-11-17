
FROM node:18 AS build

WORKDIR /app


COPY package.json package-lock.json ./
RUN npm install

# Копіюємо весь код React
COPY . ./
RUN npm run build


FROM nginx:1.23


COPY nginx.conf /etc/nginx/nginx.conf


COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
