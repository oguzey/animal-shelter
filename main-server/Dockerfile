FROM mono:4.2.3.4

EXPOSE 80

RUN mkdir /server

COPY server/* /server/

WORKDIR /server

CMD [ "mono",  "HttpServer.exe", "-h", "myhost", "-p", "80" ]
