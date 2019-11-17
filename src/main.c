#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <math.h>
#include <sys/types.h>
#include "i2c_api.h"
#include "tube_api.h"

#define BUS "/dev/i2c-0"

int main()
{
    //Opens bus
    int fd;
    if ((fd = open_bus(BUS)) < 0)
    {
        printf("Failed to open bus! %d\n", fd);
        return 1;
    }
	if (init_tubes(fd) != 0)
	{
		printf("Failed to init the tubes! %d\n", fd);
		return 1;
	}
    int brightness, tube, digit = 0;
    for (tube = 0; tube < 7; tube++)
    {
        for (digit = 0; digit < 12; digit++)
        {
            for (brightness = 1; brightness <= 0x09; brightness++)
            {
				set_tube(fd, tube, digit, brightness);
                usleep(33000);
            }
            for (brightness = 0x08; brightness >= 0; brightness--)
            {
				set_tube(fd, tube, digit, brightness);
                usleep(33000);
            }
        }
    }
    close_bus(fd);
    return 0;
}
