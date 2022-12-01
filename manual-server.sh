nc -uvkl 3390 | while read line
do
  echo "[$(date)] CLIENT: $line" 
done
