#include "tube_api.h"
#include "i2c_api.h"
#include <sys/types.h>

typedef struct tube_digit tube_digit;
struct tube_digit
{
	u_int8_t chip;
	u_int8_t adress;
};
const u_int8_t chips[] = { 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 };
//			       0             1           2	       3	      4		    5	         6	     7		  8	       9	  .L		.R
tube_digit tubes[NUMBER_OF_TUBES][NUMBER_OF_DIGITS] = {
	{{0x40, 0x0d}, {0x40, 0x15}, {0x40, 0x19}, {0x40, 0x1d}, {0x40, 0x21}, {0x40, 0x25}, {0x40, 0x29}, {0x40, 0x2d}, {0x40, 0x31}, {0x40, 0x35}, {0x40, 0x11}, {0x40, 0x09}}, // H _:_ _ : _ _ : _
	{{0x41, 0x21}, {0x41, 0x2D}, {0x40, 0x45}, {0x40, 0x41}, {0x40, 0x30}, {0x40, 0x39}, {0x41, 0x11}, {0x41, 0x15}, {0x41, 0x19}, {0x41, 0x1d}, {0x41, 0x29}, {0x41, 0x25}}, // _ H : _ _ : _ _ : _
	{{0x41, 0x41}, {0x41, 0x39}, {0x41, 0x35}, {0x41, 0x31}, {0x41, 0x09}, {0x41, 0x0d}, {0x42, 0x35}, {0x42, 0x41}, {0x42, 0x2d}, {0x42, 0x29}, {0x41, 0x3d}, {0x41, 0x45}}, // _ _ : M _ : _ _ : _
	{{0x42, 0x0d}, {0x42, 0x21}, {0x42, 0x1d}, {0x42, 0x19}, {0x42, 0x15}, {0x42, 0x11}, {0x42, 0x39}, {0x42, 0x3d}, {0x42, 0x41}, {0x42, 0x45}, {0x42, 0x25}, {0x42, 0x0d}}, // _ _ : _ M : _ _ : _
	{{0x00, 0x00}, {0x43, 0x29}, {0x43, 0x11}, {0x43, 0x25}, {0x43, 0x1d}, {0x43, 0x19}, {0x43, 0x15}, {0x43, 0x09}, {0x43, 0x35}, {0x43, 0x31}, {0x43, 0x21}, {0x43, 0x0d}}, // _ _ : _ _ : S _ : _
	{{0x43, 0x41}, {0x43, 0x3D}, {0x44, 0x11}, {0x44, 0x25}, {0x44, 0x1d}, {0x44, 0x19}, {0x44, 0x15}, {0x43, 0x39}, {0x44, 0x29}, {0x43, 0x45}, {0x44, 0x21}, {0x44, 0x0d}}, // _ _ : _ _ : _ S : _
	{{0x45, 0x3d}, {0x45, 0x35}, {0x45, 0x0d}, {0x45, 0x21}, {0x45, 0x19}, {0x45, 0x15}, {0x45, 0x11}, {0x45, 0x39}, {0x45, 0x45}, {0x45, 0x41}, {0x45, 0x1d}, {0x45, 0x09}}  // _ _ : _ _ : _ _ : D
};


int init_tubes(int fd)
{
	int i = 0;
	for (i = 0; i < sizeof(chips) / sizeof(char); i++)
	{
		if (write_register(fd, chips[i], 0x00, 0x00) < 0) {
			return 1;
		}
	}
	return 0;
}

int set_tube(int fd, int tube, int digit, u_int8_t brightness)
{
	if (tube < 0 && tube >= NUMBER_OF_TUBES)
	{
		return TUBE_DOES_NOT_EXIST;
	}

	if (digit < 0 && digit > NUMBER_OF_DIGITS)
	{
		return DIGIT_DOES_NOT_EXIST;
	}

	tube_digit d = tubes[tube][digit];
	return write_register(fd, d.chip, d.adress, brightness);
}
