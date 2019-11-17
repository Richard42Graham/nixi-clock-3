#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <math.h>
#include <sys/types.h>
#include "api.h"
#define BUS "/dev/i2c-1"

int main()
{
    //Opens bus
    int fd;
    if ((fd = open_i2c_bus(BUS)) < 0)
    {
        printf("Couldn't open bus! %d\n", fd);
        return 1;
    }
    int i, tube, digi = 0;
    for (tube = 0; tube < 7; tube++)
    {
        for (digi = 0; digi < 12; digi++)
        {
            digit d = tubes[tube][digi];
            for (i = 1; i <= 0x09; i++)
            {
                set_digit(fd, d, i);
                usleep(33000);
            }
            for (i = 0x08; i >= 0; i--)
            {
                set_digit(fd, d, i);
                usleep(33000);
            }
        }
    }
    printf("hello... \n");
    close_i2c_bus(fd);
    return 0;
}
