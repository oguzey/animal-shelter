FROM nginx:1.10

#RUN apt-get update && apt-get install -y wget lsof

#COPY ./web-page /usr/share/nginx/html
COPY ./web-server.conf /etc/nginx/conf.d/default.conf
#RUN chown nginx:nginx -R /usr/share/nginx/html
#RUN chmod 777 -R /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
