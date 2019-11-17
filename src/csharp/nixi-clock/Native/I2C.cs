using System.Runtime.InteropServices;

namespace nixi_clock.Native
{
    public static class I2C
    {

        [DllImport("a.out")]
        public static extern int open_bus(char[] bus);

        [DllImport("a.out")]
        public static extern int read_register(int fd, byte device_address, byte reg, out byte output);

        [DllImport("a.out")]
        public static extern int write_register(int fd, byte device_address, byte reg, byte input);

        [DllImport("a.out")]
        public static extern int close_bus(int fd);

    }
}
