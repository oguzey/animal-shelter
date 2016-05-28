docker network create my-network || exit -1
docker build -t web-nginx:1.10 . || exit -1 
docker run --name web-server -p 80:80 -d --net=my-network --net-alias=web-server-net web-nginx:1.10 || exit -1
cd simple-http-server && docker build -t test-server . && docker run -d --name server-handler --net=my-network --net-alias=server-handler-net test-server
