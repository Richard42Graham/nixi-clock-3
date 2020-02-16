using Iot.Device.Pwm;
using nixi_clock.Model;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace nixi_clock
{
    public class BoardRenderer
    {
        public double Brightness = 0.5f;
        public bool ToConsole { get; set; } = false;
        private readonly Pca9685[] devices;
        private readonly Dictionary<(int, int), (Pca9685, int)> devicesMap = new Dictionary<(int, int), (Pca9685, int)>();

        private Board currentState = new Board();
        public BoardRenderer(int busId = 0, double pwmFrequency = 4000)
        {
            devices = new Pca9685[6];
            for (int i = 0; i < devices.Length; i++)
                devices[i] = new Pca9685(I2cDevice.Create(new I2cConnectionSettings(busId, Pca9685.I2cAddressBase + i)), pwmFrequency);
            Array.ForEach(devices, d => d.SetDutyCycleAllChannels(0));

            // Does not map to anything:
            // - Device 2 Channel 0 - does not work, is it a hardware problem?
            // - Device 4 Channel 0
            // - Device 4 Channel 9 - 15
            // - Device 5 Channel 7 - 10

            AddMap(0, DigitType.Zero, devices[0], 1);
            AddMap(0, DigitType.One, devices[0], 3);
            AddMap(0, DigitType.Two, devices[0], 4);
            AddMap(0, DigitType.Three, devices[0], 5);
            AddMap(0, DigitType.Four, devices[0], 6);
            AddMap(0, DigitType.Five, devices[0], 7);
            AddMap(0, DigitType.Six, devices[0], 8);
            AddMap(0, DigitType.Seven, devices[0], 9);
            AddMap(0, DigitType.Eight, devices[0], 10);
            AddMap(0, DigitType.Nine, devices[0], 11);
            AddMap(0, DigitType.RightDot, devices[0], 0);
            AddMap(0, DigitType.LeftDot, devices[0], 2);


            AddMap(1, DigitType.Zero, devices[1], 6);
            AddMap(1, DigitType.One, devices[1], 9);
            AddMap(1, DigitType.Two, devices[0], 15);
            AddMap(1, DigitType.Three, devices[0], 14);
            AddMap(1, DigitType.Four, devices[0], 13);
            AddMap(1, DigitType.Five, devices[0], 12);
            AddMap(1, DigitType.Six, devices[1], 2);
            AddMap(1, DigitType.Seven, devices[1], 3);
            AddMap(1, DigitType.Eight, devices[1], 4);
            AddMap(1, DigitType.Nine, devices[1], 5);
            AddMap(1, DigitType.RightDot, devices[1], 7);
            AddMap(1, DigitType.LeftDot, devices[1], 8);

            AddMap(2, DigitType.Zero, devices[1], 14);
            AddMap(2, DigitType.One, devices[1], 12);
            AddMap(2, DigitType.Two, devices[1], 11);
            AddMap(2, DigitType.Three, devices[1], 10);
            AddMap(2, DigitType.Four, devices[1], 0);
            AddMap(2, DigitType.Five, devices[1], 1);
            AddMap(2, DigitType.Six, devices[2], 11);
            AddMap(2, DigitType.Seven, devices[2], 10);
            AddMap(2, DigitType.Eight, devices[2], 9);
            AddMap(2, DigitType.Nine, devices[2], 8);
            AddMap(2, DigitType.RightDot, devices[1], 15);
            AddMap(2, DigitType.LeftDot, devices[1], 13);

            AddMap(3, DigitType.Zero, devices[2], 1);
            AddMap(3, DigitType.One, devices[2], 6);
            AddMap(3, DigitType.Two, devices[2], 5);
            AddMap(3, DigitType.Three, devices[2], 4);
            AddMap(3, DigitType.Four, devices[2], 3);
            AddMap(3, DigitType.Five, devices[2], 2);
            AddMap(3, DigitType.Six, devices[2], 12);
            AddMap(3, DigitType.Seven, devices[2], 13);
            AddMap(3, DigitType.Eight, devices[2], 14);
            AddMap(3, DigitType.Nine, devices[2], 15);
            AddMap(3, DigitType.RightDot, devices[2], 0); // Does NOT work, is it a hardware problem?
            AddMap(3, DigitType.LeftDot, devices[2], 7);

            AddMap(4, DigitType.Zero, devices[3], 9);
            AddMap(4, DigitType.One, devices[3], 8);
            AddMap(4, DigitType.Two, devices[3], 2);
            AddMap(4, DigitType.Three, devices[3], 7);
            AddMap(4, DigitType.Four, devices[3], 5);
            AddMap(4, DigitType.Five, devices[3], 4);
            AddMap(4, DigitType.Six, devices[3], 3);
            AddMap(4, DigitType.Seven, devices[3], 0);
            AddMap(4, DigitType.Eight, devices[3], 11);
            AddMap(4, DigitType.Nine, devices[3], 10);
            AddMap(4, DigitType.RightDot, devices[3], 1);
            AddMap(4, DigitType.LeftDot, devices[3], 6);

            AddMap(5, DigitType.Zero, devices[3], 14);
            AddMap(5, DigitType.One, devices[3], 13);
            AddMap(5, DigitType.Two, devices[4], 2);
            AddMap(5, DigitType.Three, devices[4], 7);
            AddMap(5, DigitType.Four, devices[4], 5);
            AddMap(5, DigitType.Five, devices[4], 4);
            AddMap(5, DigitType.Six, devices[4], 3);
            AddMap(5, DigitType.Seven, devices[3], 12);
            AddMap(5, DigitType.Eight, devices[4], 8);
            AddMap(5, DigitType.Nine, devices[3], 15);
            AddMap(5, DigitType.RightDot, devices[4], 1);
            AddMap(5, DigitType.LeftDot, devices[4], 6);

            AddMap(6, DigitType.Zero, devices[5], 13);
            AddMap(6, DigitType.One, devices[5], 11);
            AddMap(6, DigitType.Two, devices[5], 1);
            AddMap(6, DigitType.Three, devices[5], 6);
            AddMap(6, DigitType.Four, devices[5], 4);
            AddMap(6, DigitType.Five, devices[5], 3);
            AddMap(6, DigitType.Six, devices[5], 2);
            AddMap(6, DigitType.Seven, devices[5], 12);
            AddMap(6, DigitType.Eight, devices[5], 15);
            AddMap(6, DigitType.Nine, devices[5], 14);
            AddMap(6, DigitType.RightDot, devices[5], 0);
            AddMap(6, DigitType.LeftDot, devices[5], 5);

        }

        private void AddMap(int tube, DigitType digit, Pca9685 device, int channel)
        {
            devicesMap.Add((tube, (int)digit), (device, channel));
        }

        public void Render(Board board)
        {
            for (int i = 0; i < board.Tubes.Length; i++)
            {
                var tube = board.Tubes[i];
                if (ToConsole) WriteToConsole(i, tube.Digits);


                for (int j = 0; j < tube.Digits.Length; j++)
                {
                    var digit = tube.Digits[j];
                    if (digit.DutyCycle != currentState.Tubes[i].Digits[j].DutyCycle)
                    {
                        currentState.Tubes[i].Digits[j].DutyCycle = digit.DutyCycle;
                        (Pca9685 device, int channel) = devicesMap[(i, j)];
                        device.SetDutyCycle(channel, digit.DutyCycle * Brightness);
                    }
                }
            }
        }

        private void WriteToConsole(int tube, Digit[] digits)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("T");
            builder.Append(tube);
            builder.Append(": ");
            for (int i = 0; i < digits.Length; i++)
            {
                if (i != 0)
                    builder.Append(", ");
                builder.Append(i);

                builder.Append(": ");
                builder.Append(digits[i].DutyCycle);
                builder.Append("(");
                builder.Append(digits[i].FullOn ? "t" : "f");
                builder.Append(",");
                builder.Append(digits[i].FullOff ? "t" : "f");
                builder.Append(")");
            }
            Console.WriteLine(builder.ToString());
        }
    }
}
