# current time (24h): yyyy-mm-dd hh:mm:ss
timestamp() {
    date +"%Y-%m-%d %H:%M:%S"
}

nc -4u localhost 3390 | while read line
do
  echo "server: $line"
done

