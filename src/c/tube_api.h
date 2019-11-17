#pragma once
#ifndef TUBE_API_H_INCLUDED
#define TUBE_API_H_INCLUDED
#include "export.h"
#include <sys/types.h>

// Info
#define NUMBER_OF_TUBES 7
#define NUMBER_OF_DIGITS 12

// Error codes
#define TUBE_DOES_NOT_EXIST 42
#define DIGIT_DOES_NOT_EXIST 7

EXPORT int init_tubes(int fd);

EXPORT int set_tube(int fd, int tupe, int digit, u_int8_t brightness);

#endif