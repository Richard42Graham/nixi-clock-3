#!/bin/bash

delay=2

echo "setting up IO chip"
i2cset -y 1 0x20 0x00 0x00 # setup port A as all outputs
i2cset -y 1 0x20 0x01 0x00 # setup port B as all outputs

echo "turning on HV"
i2cset -y 1 0x20 0x14 0x00 # set pin A4 HIGH to enable HV and enable ouput on PWM chips

for adress in 0x40 0x41 0x42 0x43 0x44 0x45
do

i2cset -y 1 $adress 0x00 0x00


	for digit in 0x09 0x0d 0x11 0x15 0x19 0x1D 0x21 0x25 0x29 0x2D 0x31 0x35 0x39 0x3D 0x41 0x45
	do

#	    	echo $adress $digit
		echo "............."

		i2cset -y 1 $adress $digit 9

                echo "tube"
		read IN
                echo "Digit"
                read DIG
		echo $IN
                echo "$IN,$DIG,$adress,$digit" >> out.txt

		i2cset -y 1 $adress $digit 0

	done
echo "done, next chip"
done

i2cset -y 1 0x20 0x14 0xFF # turn of the stunningly tingly HV 
