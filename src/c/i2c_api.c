
#include "i2c_api.h"

#include <fcntl.h> // open bus file
#include <linux/i2c-dev.h>
// #include <linux/i2c.h>
#include <sys/types.h>

int open_bus(char bus[]) {
	return open(bus, O_RDWR);
}

int close_bus(int fd) {
	return close(fd);
}

int read_register(int fd, u_int8_t device_address, u_int8_t reg, char* output)
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

int write_register(int fd, u_int8_t device_address, u_int8_t reg, u_int8_t value)
{
	ioctl(fd, I2C_SLAVE, device_address);
	struct i2c_msg msgs[1] = {
		{
			.addr = device_address,
			.flags = 0,
			.len = 2,
			.buf = (u_int8_t[2]){reg, value},
		} };
	struct i2c_rdwr_ioctl_data msgset = {
		.msgs = msgs,
		.nmsgs = 1 };
	return ioctl(fd, I2C_RDWR, &msgset);
}
