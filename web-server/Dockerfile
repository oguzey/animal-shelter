FROM nginx:1.10


#COPY ./web-page /usr/share/nginx/html
COPY ./web-server.conf /etc/nginx/conf.d/default.conf
RUN sed -i 's/user  nginx/user root/' /etc/nginx/nginx.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
