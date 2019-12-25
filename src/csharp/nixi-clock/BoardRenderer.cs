using System;
using System.Collections.Generic;
using System.Text;
using System.Device.I2c;
using Iot.Device.Pwm;

namespace nixi_clock
{
    public class BoardRenderer
    {
        private I2cDevice device;
        private Pca9685 board;
        public BoardRenderer(I2cDevice device)
        {
            board = new Pca9685(device);
            //board.SetDutyCycle()
        }
    }
}
