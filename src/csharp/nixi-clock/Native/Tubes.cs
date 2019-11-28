using System.Runtime.InteropServices;

namespace nixi_clock.Native
{
    public static class Tubes
    {
        [DllImport("a.out")]
        public static extern int init_tubes(int fd);

        [DllImport("a.out")]
        public static extern int set_tube(int fd, int tupe, int digit, byte brightness);

        // Info
        public const int NUMBER_OF_TUBES = 7;
        public const int NUMBER_OF_DIGITS = 12;

        // Error codes
        public const int TUBE_DOES_NOT_EXIST = 42;
        public const int DIGIT_DOES_NOT_EXIST = 7;
    }
}
