#!/usr/bin/env python
"""
Very simple HTTP server in python.
Usage::
    ./dummy-web-server.py [<ip>] [<port>]
Send a GET request::
    curl http://localhost
Send a HEAD request::
    curl -I http://localhost
Send a POST request::
    curl -d "foo=bar&bin=baz" http://localhost
"""
from BaseHTTPServer import BaseHTTPRequestHandler, HTTPServer
import SocketServer

class S(BaseHTTPRequestHandler):
    def _set_headers(self):
        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()

    def do_GET(self):
        self._set_headers()
        self.wfile.write("<html><body><h1>GET received!</h1></body></html>")

    def do_HEAD(self):
        self._set_headers()
        
    def do_POST(self):
        # Doesn't do anything with posted data
        self._set_headers()
        self.wfile.write("<html><body><h1>POST received!</h1></body></html>")
        
def run(server_class=HTTPServer, handler_class=S, ip='127.0.0.1', port=80):
    server_address = (ip, port)
    httpd = server_class(server_address, handler_class)
    print 'Starting httpd...'
    httpd.serve_forever()

if __name__ == "__main__":
    from sys import argv

    if len(argv) == 3:
        run(ip=argv[1], port=int(argv[2]))
    elif len(argv) == 2:
        run(port=int(argv[1]))
        run()
