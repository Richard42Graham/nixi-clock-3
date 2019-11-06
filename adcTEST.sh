#!/bin/bash

addr=0x55

i2cset -y 1 $addr 0x02 0x20 # set automatic convertion mode
sleep 0.5

var1=i2cget -y 1 $addr 0x00 # MSB
var2=i2cget -y 1 $addr 0x01 # LSB

oupput =((($var1 & 0x0F)*256) + $var2)

echo $ouptut
