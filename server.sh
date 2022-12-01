#!/bin/zsh

# make sure server is killed before script exits
# see: down()
trap down SIGINT

# current time (24h): yyyy-mm-dd hh:mm:ss
timestamp() {
    date +"%Y-%m-%d %H:%M:%S"
}

# capture stdout from nc
parse_packet() {
    read -p packet
    echo "[$(timestamp)] client: $packet" | tee log.txt
}

# send to nc stdin
send_response() {
    response="[re: $packet] PONG"
    print -p $response
    echo "[$(timestamp)] server: $response" | tee log.txt
}

# kills server process
down() {
    kill "$COPROC_PID"
}

# start udp listener on port 3390
up() {
    coproc ( nc -uvlk 3390 )
    while true; do
        parse_packet
        send_response
    done
}

# start server
up
