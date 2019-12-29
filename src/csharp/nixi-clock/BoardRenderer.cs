using System;
using System.Collections.Generic;
using System.Text;
using System.Device.I2c;
using Iot.Device.Pwm;
using nixi_clock.Model;

namespace nixi_clock
{
    public class BoardRenderer
    {
        private I2cDevice device;
        private Pca9685 board;
        public bool RenderToConsole { get; set; } = false;

        public BoardRenderer()
        {
            //I2cDevice device;
            //board = new Pca9685(device);
            //board.SetDutyCycle()
        }

        public void Render(Board board)
        {
            if (this.RenderToConsole)
                Console.Clear();
            for (int i = 0; i < board.Tubes.Length; i++)
            {
                var tube = board.Tubes[i];
                if (this.RenderToConsole)
                    ToConsole(i, tube.Digits);
                //for (int j = 0; j < tube.Digits.Length; j++)
                //{
                //    var digit = tube.Digits[j];
                //}
            }
        }

        private void ToConsole(int tube, Digit[] digits)
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
