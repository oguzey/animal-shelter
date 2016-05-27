FROM nginx:1.10

RUN apt-get update && apt-get install -y wget lsof

COPY html /usr/share/nginx/html
COPY nginx.conf /etc/nginx/sites-enabled/default

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
