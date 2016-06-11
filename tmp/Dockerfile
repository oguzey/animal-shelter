FROM nginx:1.10

#RUN apt-get update && apt-get install -y wget lsof

COPY html2 /usr/share/nginx/html
COPY docker-nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
