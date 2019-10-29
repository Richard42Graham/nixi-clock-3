#!/bin/bash

echo "setting up IO chip"
i2cwrite -y 1 0x20 0x00 0x00 # setup port A as all outputs
i2cwrite -y 1 0x20 0x01 0x00 # setup port B as all outputs

echo "turning on HV"
i2cwrite -y 1 0x20 0x13 0x16 # set pin A4 HIGH to enable HV

echo "setting digit"
i2cwrite -y 1 0x40 0x?? 0x01 ?? # set first (tube closest to power in) to a digit
# set it to a breightness,

# turn on enbale pin to show tube lighting up. 



# maybe do a dimming loop between some digit to test them
