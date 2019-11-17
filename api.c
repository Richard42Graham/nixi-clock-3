#include "api.h"
#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <math.h>

#include <fcntl.h>     // open bus file
#include <sys/ioctl.h> // to open I2C device
#include <unistd.h>    // read/write to i2c device,
#include <linux/i2c-dev.h>
#include <sys/types.h>
#include <linux/i2c-dev.h>

int open_i2c_bus(char bus[]) {
	int fd = 0;
	if ((fd = open(bus, O_RDWR)) < 0)
	{
		//Failed to open
		return fd;
	}
	init(fd);
	return fd;
}

int init(int fd) {
	int i = 0;
	for (i = 0; i < sizeof(chips) / sizeof(char); i++)
	{
		if (write_reg(fd, chips[i], 0x00, 0x00) < 0) {
			return 1;
		}
	}
	return 0;
}

int close_i2c_bus(int fd) {
	return close(fd);
}

int read_reg(int fd, u_int8_t device_address, u_int8_t reg, char* output)
{
	ioctl(fd, I2C_SLAVE, device_address);
	struct i2c_msg msgs[2] = {
		{
			.addr = device_address,
			.flags = 0,
			.len = 1,
			.buf = &reg,
		},
		{.addr = device_address,
		 .flags = I2C_M_RD | I2C_M_NOSTART,
		 .len = 1,
		 .buf = output},
	};
	struct i2c_rdwr_ioctl_data msgset[1] = {
		{.msgs = msgs,
		 .nmsgs = 2} };
	return ioctl(fd, I2C_RDWR, &msgset);
}

int write_reg(int fd, u_int8_t device_address, u_int8_t reg, char input)
{
	ioctl(fd, I2C_SLAVE, device_address);
	struct i2c_msg msgs[1] = {
		{
			.addr = device_address,
			.flags = 0,
			.len = 2,
			.buf = (char[2]){reg, input},
		} };
	struct i2c_rdwr_ioctl_data msgset = {
		.msgs = msgs,
		.nmsgs = 1 };
	return ioctl(fd, I2C_RDWR, &msgset);
}


int set_digit(int fd, digit digi, char value) {
	return write_reg(fd, digi.chip, digi.adress, value);
}
