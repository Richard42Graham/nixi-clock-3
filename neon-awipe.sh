#!/bin/bash
x=0
y=0

i2cset -y 1 0x2a 0x03 0xFF

while [ 1 -eq 1 ]
do
    if [ $x -eq 255 ] && [ $y -eq 255 ]
    then
    	x=$(( $x - 1 ))
    elif [ $x -eq 0 ] && [ $y -eq 0 ]
    then
        x=$(( $x + 1 ))
    elif [ $x -eq 255 ]
    then
        y=$(( $y + 1 ))
    elif [ $y -eq 0 ]
    then
        x=$(( $x + 1 ))
    elif [ $x -eq 0 ]
    then
        y=$(( $y - 1 ))
    else
       x=$(( $x - 1 ))
   fi
   i2cset -y 1 0x2a 0x01 $x
   i2cset -y 1 0x2a 0x02 $y
done
echo "done"
