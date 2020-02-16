using Mono.Unix.Native;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WiringPi;
using static WiringPi.GPIO;

namespace nixi_clock
{
    //https://github.com/danriches/WiringPi.Net
    public class GpioIExperimental
    {
        [DllImport("c")]
        static extern void setenv(string key, string value);

        private readonly ConcurrentQueue<(int, int, long)> queue;
        private readonly Stopwatch stopwatch = new Stopwatch();
        public GpioIExperimental(ConcurrentQueue<(int, int, long)> queue)
        {
            setenv("WIRINGPI_DEBUG", "TRUE");
            Init.WiringPiSetupPhys();

            this.queue = queue;
            stopwatch.Start();
        }

        public Thread TestGPIO(int pin)
        {
            var gpioPin = MiscFunctions.physPinToGpio(pin);
            var pinPath = $"/sys/class/gpio/gpio{gpioPin}/";

            Console.WriteLine($"pin: {pin}, pinPath: {pinPath}, GPIO pin: {gpioPin}");

            if (!Directory.Exists(pinPath))
            {
                using (var exportFile = File.OpenWrite("/sys/class/gpio/export"))
                {
                    string inText = gpioPin + "\n";
                    var bytes = Encoding.ASCII.GetBytes(inText);
                    exportFile.Write(bytes, 0, bytes.Length);
                    exportFile.Flush();

                }
                Thread.Sleep(100);
            }


            using (var directionFile = File.OpenWrite(pinPath + "direction"))
            {
                string inText = "in\n";
                var bytes = Encoding.ASCII.GetBytes(inText);
                directionFile.Write(bytes, 0, bytes.Length);
                directionFile.Flush();
                Thread.Sleep(100);
            }


            if (File.Exists(pinPath + "edge"))
            {
                using (var edgeFile = File.OpenWrite(pinPath + "edge"))
                {
                    string inText = "both\n";
                    var bytes = Encoding.ASCII.GetBytes(inText);
                    edgeFile.Write(bytes, 0, bytes.Length);
                    edgeFile.Flush();
                    Thread.Sleep(100);
                }
            }

            GPIO.pinMode(pin, (int)GPIOpinmode.Input);
            Thread.Sleep(100);
            GPIO.pullUpDnControl(pin, (int)PullUpDnValue.Up);
            Thread.Sleep(100);


            var t = new Thread(() =>
            {

                //var value = GPIO.digitalRead(pin);
                //Console.WriteLine($"Initial value: {value}");
                var value = 0;
                var fd = Syscall.open(pinPath + "value", OpenFlags.O_RDWR);
                IntPtr valueBuffer = Marshal.AllocHGlobal(1);
                Console.WriteLine($"fd: {fd}");



                while (true)
                {
                    //int res = PiThreadInterrupts.waitForInterrupt(pin, 1000);
                    //PiThreadInterrupts.waitForInterrupt(pin, 100000);

                    //Setup
                    Pollfd pollfd = new Pollfd();
                    pollfd.fd = fd;
                    pollfd.events = PollEvents.POLLPRI;
                    var polls = new Pollfd[] { pollfd };

                    //Clear interrupt
                    IntPtr intPtrTemp = Marshal.AllocHGlobal(1);
                    //Syscall.lseek(fd, 0L, SeekFlags.SEEK_SET);
                    long readResult = Syscall.read(fd, intPtrTemp, 1);

                    //Wait
                    //Console.WriteLine($"Waiting...");
                    int pollResult = Syscall.poll(polls, -1);
                    //Console.WriteLine($"Interrupt!");

                    //Read the value
                    Syscall.lseek(fd, 0L, SeekFlags.SEEK_SET);
                    Syscall.read(fd, valueBuffer, 1);
                    var v = Marshal.ReadByte(valueBuffer);

                    ////Clear interrupt
                    //readResult = Syscall.read(fd, intPtrTemp, 1);
                    //Marshal.FreeHGlobal(intPtrTemp);


                    var newValue = (v == '0') ? 0 : 1;

                    //var newValue = GPIO.digitalRead(pin);
                    //Console.WriteLine($"v: {v}, newValue: {newValue}");
                    //var newValue = ListenOnPin(fd, pin);
                    if (value != newValue)
                    {
                        //Console.WriteLine($"{DateTime.Now.ToString()}: Pin {pin} changed value to {value}");
                        queue.Enqueue((pin, value, stopwatch.Elapsed.Ticks));
                        value = newValue;
                    }
                }

                Marshal.FreeHGlobal(valueBuffer);


            });

            t.Start();
            return t;

            //Gpio1.ListenToPin(() => GPIO.digitalRead(pin));

        }
    }
}
