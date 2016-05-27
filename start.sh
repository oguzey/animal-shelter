docker build -t web-nginx:1.10 . && docker run --name web-server -p 800:80 -d web-nginx:1.10
