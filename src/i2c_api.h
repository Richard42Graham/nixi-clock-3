#pragma once
#ifndef I2C_API_H_INCLUDED
#define I2C_API_H_INCLUDED
#include <sys/types.h>
#include "export.h"

EXPORT int open_bus(char bus[]);

EXPORT int read_register(int fd, u_int8_t device_address, u_int8_t reg, char* output);

EXPORT int write_register(int fd, u_int8_t device_address, u_int8_t reg, u_int8_t input);

EXPORT int close_bus(int fd);

#endif