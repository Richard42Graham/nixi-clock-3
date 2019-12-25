using System;
using System.Text;
using System.Threading;

namespace nixi_clock
{
    class Program
    {
        const string BUS = "/dev/i2c-0";
        static void Main(string[] args)
        {
            ////Opens bus
            //int fd;
            //if ((fd = I2C.open_bus(Encoding.UTF8.GetBytes(BUS))) < 0)
            //{
            //    Console.WriteLine("Failed to open bus! %d\n", fd);
            //    return;
            //}
            //if (Tubes.init_tubes(fd) != 0)
            //{
            //    Console.WriteLine("Failed to init the tubes! %d\n", fd);
            //    return;
            //}

//            TubesController tubesController = new TubesController(fd);
//            tubesController.Run();
//            TestTubes(fd);
            //Tubes.set_tube(fd, 0, 5, (byte)255);
            //I2C.close_bus(fd);
        }

        private static void TestTubes(int fd)
        {
            int brightness, tube, digit = 0;
            for (tube = 0; tube < 7; tube++)
            {
                for (digit = 0; digit < 12; digit++)
                {
                    for (brightness = 1; brightness <= 0x09; brightness++)
                    {
                        //Tubes.set_tube(fd, tube, digit, (byte)brightness);
                        Thread.Sleep(33);
                    }
                    for (brightness = 0x08; brightness >= 0; brightness--)
                    {
                        //Tubes.set_tube(fd, tube, digit, (byte)brightness);
                        Thread.Sleep(33);
                    }
                }
            }
        }
    }
}
