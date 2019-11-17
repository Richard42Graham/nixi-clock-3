#!/bin/bash

echo "setting up IO chip"
i2cset -y 1 0x20 0x00 0x00 # setup port A as all outputs
i2cset -y 1 0x20 0x01 0x00 # setup port B as all outputs

echo "turning on HV"
i2cset -y 1 0x20 0x14 0x00 # set pin A4 HIGH to enable HV and enable ouput on PWM chips

i2cset -y 1 0x40 0x00 0x00 # set mode 1
i2cset -y 1 0x40 0x06 0
i2cset -y 1 0x40 0x07 0
# i2cset -y 2 0x40 0x08 88
i2cset -y 1 0x40 0x09 2 # 2 is the var
### https://www.raspberrypi.org/forums/viewtopic.php?t=9007 ### 
sleep 0.5

for counter in 1 2 3 4 5 6 7 8 9
do
echo $counter
i2cset -y 1 0x40 0x09 $counter 
sleep 0.3
done

for i in 10 9 8 7 6 5 4 3 2 1  0
do 
echo $i
i2cset -y 1 0x40 0x09 $i
sleep 0.3  
done 


sleep 1
echo "changing digit"


i2cset -y 1 0x40 0x0A 0		# setup PWM for digit 0
i2cset -y 1 0x40 0x0B 0
i2cset -y 1 0x40 0x0C 0

for counter in 1 2 3 4 5 6 7 8 9
do
echo $counter
i2cset -y 1 0x40 0x0D $counter
sleep 0.3
done

for i in 10 9 8 7 6 5 4 3 2 1  0
do
echo $i
i2cset -y 1 0x40 0x0D $i
sleep 0.3
done


sleep 1
echo "changing digit"


i2cset -y 1 0x40 0x0E 0         # setup PWM for digit 0
i2cset -y 1 0x40 0x0F 0
i2cset -y 1 0x40 0x10 0

for counter in 1 2 3 4 5 6 7 8 9
do
echo $counter
i2cset -y 1 0x40 0x11 $counter
sleep 0.3
done

for i in 10 9 8 7 6 5 4 3 2 1  0
do
echo $i
i2cset -y 1 0x40 0x11 $i
sleep 0.3
done


sleep 1
echo "changing digit"


i2cset -y 1 0x40 0x12 0         # setup PWM for digit 0
i2cset -y 1 0x40 0x13 0
i2cset -y 1 0x40 0x14 0

for counter in 1 2 3 4 5 6 7 8 9
do
echo $counter
i2cset -y 1 0x40 0x15 $counter
sleep 0.3
done

for i in 10 9 8 7 6 5 4 3 2 1  0
do
echo $i
i2cset -y 1 0x40 0x15 $i
sleep 0.3
done

