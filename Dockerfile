FROM nginx:1.10

COPY html /usr/share/nginx/html
COPY load-balancer.conf /etc/nginx/conf.d/

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
