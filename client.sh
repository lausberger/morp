nc -4u localhost 3390 | while read line
do
  echo "[$(date)] SERVER: $line"
done

