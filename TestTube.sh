#!/bin/bash

echo "setting up IO chip"
i2cset -y 1 0x20 0x00 0x00 # setup port A as all outputs
i2cset -y 1 0x20 0x01 0x00 # setup port B as all outputs

echo "turning on HV"
i2cset -y 1 0x20 0x14 0x16 # set pin A4 HIGH to enable HV



i2cset -y 1 0x40 0x00 0x00 # set mode 1

i2cset -y 2 0x40 0x06 0
i2cset -y 2 0x40 0x07 0
# i2cset -y 2 0x40 0x08 88
i2cset -y 2 0x40 0x09 2 # 2 is the var
### https://www.raspberrypi.org/forums/viewtopic.php?t=9007 ### 




echo "setting digit"
i2cset -y 1 0x40 0x?? 0x01 ?? # set first (tube closest to power in) to a digit
# set it to a breightness,

# turn on enbale pin to show tube lighting up. 



# maybe do a dimming loop between some digit to test them
