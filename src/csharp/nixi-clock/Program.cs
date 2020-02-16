using Iot.Device.Pwm;
using nixi_clock.Model;
using System;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace nixi_clock
{
    class Program
    {


        //Package Mangager Console: & "C:\Program Files\PuTTY\pscp.exe" -r -i C:\users\mrjav\.ssh\ssh-key.ppk C:\Users\mrjav\Documents\nixi-clock-3\src\csharp\nixi-clock\bin\Debug\netcoreapp3.0\* richard@192.168.1.7:/home/richard/nixi-clock-debug
        //Visual Studio Command Window: DebugAdapterHost.Launch /LaunchJson:C:\Users\mrjav\Documents\nixi-clock-3\src\csharp\nixi-clock\Properties/remoteLaunch.json /EngineGuid:541B8A8A-6081-4506-9F0A-1CE771DEBC04

        private static Clock clock = new Clock();
        private static Countdown countdown = new Countdown();
        private static IBoardState boardState;

        static void Main(string[] args)
        {
            var buffer = new BufferBlock<UserInputEvent>();
            GpioInputHandler gpio2Listener = new GpioInputHandler(buffer);
            Task.Run(async () => await UserInputHandler(buffer));
            //TestMap();
            
            BoardRenderer renderer = new BoardRenderer();
            //renderer.Brightness = 0.1;
            renderer.Brightness = 0.3;
            boardState = clock;
            while (true)
            {
                Board board = boardState.GetBoard();
                renderer.Render(board);
                Thread.Sleep(5);
            }
        }

        static async Task UserInputHandler(ISourceBlock<UserInputEvent> source)
        {
            // Read from the source buffer until the source buffer has no 
            // available output data.
            while (await source.OutputAvailableAsync())
            {
                var input = source.Receive();
                if (input == UserInputEvent.ButtonPress)
                {
                    if (boardState == clock)
                        boardState = countdown;
                    else
                        boardState = clock;
                }

                if (input == UserInputEvent.RotateRight && boardState == countdown)
                    countdown.Increase();
                else if (input == UserInputEvent.RotateLeft && boardState == countdown)
                    countdown.Decrease();
            }
        }


        private static void TestMap()
        {
            BoardRenderer boardRenderer = new BoardRenderer();
            for (int tube = 0; tube < 7; tube++)
            {
                for (int digit = 0; digit < 12; digit++)
                {
                    Console.WriteLine($"Tube: {tube}, Digit: {digit}");
                    for (int i = 0; i < 100; i++)
                    {
                        var board = new Board();
                        board.Tubes[tube].Digits[digit].DutyCycle = i / 100.0;
                        boardRenderer.Render(board);
                        Thread.Sleep(1);
                    }
                    for (int i = 100; i > 0; i--)
                    {
                        var board = new Board();
                        board.Tubes[tube].Digits[digit].DutyCycle = i / 100.0;
                        boardRenderer.Render(board);
                        Thread.Sleep(1);
                    }
                    boardRenderer.Render(new Board());
                }
            }
            Console.WriteLine("Done");
        }


        private static void TestTubes()
        {
            Console.WriteLine("Connecting to devices");
            Pca9685[] devices = new Pca9685[6];
            for (int i = 0; i < devices.Length; i++)
                devices[i] = new Pca9685(I2cDevice.Create(new I2cConnectionSettings(0, Pca9685.I2cAddressBase + i)), 1000);

            Console.WriteLine("Init...");
            Array.ForEach(devices, d => d.SetDutyCycleAllChannels(0));
            Console.WriteLine("Tests all channels on the devices");
            for (int device = 0; device < devices.Length; device++)
            {
                for (int channel = 0; channel < 16; channel++)
                {
                    Console.WriteLine($"Device: {device}, Channel: {channel}");
                    for (int i = 0; i < 100; i++)
                    {
                        devices[device].SetDutyCycle(channel, i / 100.0);
                        Thread.Sleep(1);
                    }
                    for (int i = 100; i > 0; i--)
                    {
                        devices[device].SetDutyCycle(channel, i / 100.0);
                        Thread.Sleep(1);
                    }
                    devices[device].SetDutyCycle(channel, 0);
                }
            }

            Console.WriteLine("Done");
        }
    }
}

