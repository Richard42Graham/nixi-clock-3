using Mono.Unix.Native;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WiringPi;

namespace nixi_clock
{
    public class GpioPin : IDisposable
    {
        private readonly int pin;
        private readonly IntPtr valueBuffer;
        private readonly int fileDescriptor;
        private readonly Pollfd[] pollfds;
        private int currentValue = 0;
        public event EventHandler<(int pin, int value)> OnPinChange;
        private bool doListen = false;
        private bool stopped = false;
        public GpioPin(int pin)
        {
            this.pin = pin;

            var gpioPin = MiscFunctions.physPinToGpio(pin);
            var pinPath = $"/sys/class/gpio/gpio{gpioPin}/";

            //Console.WriteLine($"pin: {pin}, pinPath: {pinPath}");

            if (!Directory.Exists(pinPath))
            {
                using (var exportFile = File.OpenWrite("/sys/class/gpio/export"))
                {
                    string inText = gpioPin + "\n";
                    var bytes = Encoding.ASCII.GetBytes(inText);
                    exportFile.Write(bytes, 0, bytes.Length);
                    exportFile.Flush();

                }
                Thread.Sleep(10);
            }


            using (var directionFile = File.OpenWrite(pinPath + "direction"))
            {
                string inText = "in\n";
                var bytes = Encoding.ASCII.GetBytes(inText);
                directionFile.Write(bytes, 0, bytes.Length);
                directionFile.Flush();
                Thread.Sleep(10);
            }


            if (File.Exists(pinPath + "edge"))
            {
                using (var edgeFile = File.OpenWrite(pinPath + "edge"))
                {
                    string inText = "both\n";
                    var bytes = Encoding.ASCII.GetBytes(inText);
                    edgeFile.Write(bytes, 0, bytes.Length);
                    edgeFile.Flush();
                    Thread.Sleep(10);
                }
            }

            GPIO.pinMode(pin, (int)GPIO.GPIOpinmode.Input);
            Thread.Sleep(10);
            GPIO.pullUpDnControl(pin, (int)GPIO.PullUpDnValue.Up);
            Thread.Sleep(10);

            fileDescriptor = Syscall.open(pinPath + "value", OpenFlags.O_RDWR);
            valueBuffer = Marshal.AllocHGlobal(1);
            if (fileDescriptor < 0)
                throw new Exception($"Failed to open {pinPath}value, for pin {pin}, return value: {fileDescriptor}");

            //Clear any interrupts
            currentValue = Read();

            pollfds = new Pollfd[] { new Pollfd
            {
                fd = fileDescriptor,
                events = PollEvents.POLLPRI
            } };

            ListenForPinChange();
        }

        private void ListenForPinChange()
        {
            doListen = true;
            Thread t = new Thread(() =>
            {
                while (doListen)
                {
                    var newValue = NextValue();
                    if (!doListen) break;
                    OnPinChange?.Invoke(this, (pin, newValue));
                }
                stopped = true;
            });
            t.Start();
        }


        /// <summary>
        /// Waits for the value to change and then returns the new value
        /// Blocking call
        /// </summary>
        /// <returns>The next read value</returns>
        private int NextValue()
        {
            int newValue = currentValue;
            while (currentValue == newValue)
            {
                // Blocking call, only returns when the value have changed
                //Console.WriteLine("waiting...");
                int pollResult = Syscall.poll(pollfds, 250);
                //Console.WriteLine($"pollResult: {pollResult}");
                if (pollResult < 0)
                    throw new Exception($"poll(fds,...) failed for pin {pin}, with return code: {pollResult}");

                // Return if we are supposed to stop
                if (!doListen) return 0;

                //Reads the value
                newValue = Read();
                //Console.WriteLine($"newValue: {newValue}");
            }
            currentValue = newValue;
            return newValue;
        }

        /// <summary>
        /// Reads the current value
        /// This also clears any interrupts
        /// </summary>
        /// <returns>The current value of the pin</returns>
        private int Read()
        {
            Syscall.lseek(fileDescriptor, 0L, SeekFlags.SEEK_SET);
            Syscall.read(fileDescriptor, valueBuffer, 1);
            var value = Marshal.ReadByte(valueBuffer);
            return (value == '0') ? 0 : 1;
        }

        public void Dispose()
        {
            doListen = false;
            while (!stopped) Thread.Sleep(10);
            Syscall.close(fileDescriptor);
            Marshal.FreeHGlobal(valueBuffer);
        }
    }
}
