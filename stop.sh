docker stop web-server
docker rm web-server
docker rmi web-nginx:1.10

docker stop server-handler
docker rm server-handler
docker rmi test-server

docker network rm my-network
