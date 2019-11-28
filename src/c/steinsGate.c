#include <stdio.h>
#include <stdbool.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <math.h>
#include <sys/types.h>
#include "i2c_api.h"
#include "tube_api.h"

int guess[] = { 0, 1, 0, 0, 0, 0, 0 };   // current values
int target[] = { 5, 0, 2, 6, 6, 5, 3 };   // desired values

void RenderTubes(int fd, int brightness, int tubevalue[])
{
	int tube = 0;
	for (tube = 0; tube < 7; tube++)
	{
		int digit;
		for (digit = 0; digit < 12; digit++)
		{
			if (digit == tubevalue[tube])
			{
				set_tube(fd, tube, tubevalue[tube], brightness);
			}
			else
			{
				set_tube(fd, tube, digit, 0);
			}
		}
	}
}

void UpdateRandom()
{
	int tube;
	for (tube = 0; tube < sizeof(target) / sizeof(target[0]); tube++)
	{
		if (guess[tube] != target[tube])
		{
			//Make a new guess
			guess[tube] = (rand() % (9 - 0 + 1)) + (0);
		}
	}
}

bool IsDone()
{
	int tube;
	for (tube = 0; tube < sizeof(target) / sizeof(target[0]); tube++)
	{
		if (guess[tube] != target[tube])
		{
			return false;
		}
	}
	return true;
}

void ResetGuess()
{
	int tube;
	for (tube = 0; tube < sizeof(guess) / sizeof(guess[0]); tube++)
	{
		guess[tube] = 1;
	}
}

void SteinsGate(int fd)
{
	while (1 == 1)
	{
		RenderTubes(fd, 1, guess);
		UpdateRandom();
		usleep(33000);
		if (IsDone())
		{
			RenderTubes(fd, 4, guess);
			usleep(2500000);
			ResetGuess();
		}
	}
}
