#pragma once
#ifndef I2C_API_H_INCLUDED
#define I2C_API_H_INCLUDED
#include <sys/types.h>

int open_bus(char bus[]);

int read_register(int fd, u_int8_t device_address, u_int8_t reg, char* output);

int write_register(int fd, u_int8_t device_address, u_int8_t reg, u_int8_t input);

int close_bus(int fd);

#endif