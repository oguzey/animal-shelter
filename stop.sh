docker stop web-server
docker rm web-server
docker rmi web-nginx:1.10

#docker stop server-handler
#docker rm server-handler
#docker rmi test-server

docker stop http-server
docker rm http-server
docker stop http-server2
docker rm http-server2
docker rmi mono-http-server:1.0


docker stop hazelcast-cluster
docker stop cassandra-cluster
docker rm hazelcast-cluster
docker rm cassandra-cluster

docker network rm my-network
