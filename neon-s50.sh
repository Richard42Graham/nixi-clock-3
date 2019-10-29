#!/bin/bash
x=0
y=42


while [ $y -le 44 ]
do

while [ $x -le 256 ]
do
#  echo "Welcome $x times"

i2cset -y 1 0x2a 0x03 $x
# sleep 0.05
  x=$(( $x + 1 ))
done


while [ $x -gt 0  ]
do
#  echo "Welcome $x times"

i2cset -y 1 0x2a 0x03 $x
# sleep 0.05
  x=$(( $x - 1 ))
done

done


echo "done"
