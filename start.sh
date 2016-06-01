docker network create --subnet=172.18.0.0/16 my-network || exit -1


#cd simple-http-server && docker build -t test-server . && docker run -d --name server-handler --net=my-network --net-alias=server-handler-net test-server
cd main-server 
docker build -t mono-http-server:1.0 . || exit -1
docker run -d --name http-server --net=my-network --ip 172.18.0.20 --add-host myproject:172.18.0.20 --add-host myhost:172.18.0.20 mono-http-server:1.0 || exit -1
docker run -d --name http-server2 --net=my-network --ip 172.18.0.21 --add-host myproject:172.18.0.21 --add-host myhost:172.18.0.21  mono-http-server:1.0 || exit -1
cd ..

docker build -t web-nginx:1.10 . || exit -1 
docker run --name web-server -p 80:80 -d --net=my-network --ip 172.18.0.22 web-nginx:1.10 || exit -1
