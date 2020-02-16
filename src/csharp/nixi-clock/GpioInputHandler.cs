using nixi_clock.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using WiringPi;

namespace nixi_clock
{
    class GpioInputHandler :IDisposable
    {

        [DllImport("c")]
        static extern void setenv(string key, string value);

        private int rotate1Pin = 10;
        private int rotate2Pin = 12;

        private int rotate1Value = 1;
        private int rotate2Value = 1;

        private RotationState rotationState = RotationState.Stationary;
        private readonly ITargetBlock<UserInputEvent> producerBlock;

        private GpioPin button;
        private GpioPin rotate1;
        private GpioPin rotate2;

        private enum RotationState
        {
            Stationary,
            RotatingRightTick1,
            RotatingRightTick2,
            RotatingRightTick3,
            RotatingLeftTick1,
            RotatingLeftTick2,
            RotatingLeftTick3,
        }
        public GpioInputHandler(ITargetBlock<UserInputEvent> producerBlock)
        {
            this.producerBlock = producerBlock;
            //setenv("WIRINGPI_DEBUG", "TRUE");
            Init.WiringPiSetupPhys();

            button = new GpioPin(8);
            rotate1 = new GpioPin(10);
            rotate2 = new GpioPin(12);
            button.OnPinChange += Button_OnPinChange;
            rotate1.OnPinChange += Rotate_OnPinChange;
            rotate2.OnPinChange += Rotate_OnPinChange;
        }

        private void Button_OnPinChange(object sender, (int pin, int value) e)
        {
            if (e.value == 0)
                producerBlock.Post(UserInputEvent.ButtonPress);
            else
                producerBlock.Post(UserInputEvent.ButtonRelease);
        }

        private void Rotate_OnPinChange(object sender, (int pin, int value) e)
        {
            if (e.pin == rotate1Pin)
            {
                //Turning right
                if (e.value == 0 && rotate2Value == 1 && rotationState == RotationState.Stationary)
                    rotationState = RotationState.RotatingRightTick1;
                if (e.value == 1 && rotate2Value == 1 && rotationState == RotationState.RotatingRightTick1)
                    rotationState = RotationState.Stationary; // Turned back
                else if (e.value == 1 && rotate2Value == 0 && rotationState == RotationState.RotatingRightTick2)
                    rotationState = RotationState.RotatingRightTick3;
                else if (e.value == 0 && rotate2Value == 0 && rotationState == RotationState.RotatingRightTick3)
                    rotationState = RotationState.RotatingRightTick2; // Turned back

                //Turning Left
                else if (e.value == 0 && rotate2Value == 0 && rotationState == RotationState.RotatingLeftTick1)
                    rotationState = RotationState.RotatingLeftTick2;
                else if (e.value == 1 && rotate2Value == 0 && rotationState == RotationState.RotatingLeftTick2)
                    rotationState = RotationState.RotatingLeftTick1; // Turned back
                else if (e.value == 1 && rotate2Value == 1 && rotationState == RotationState.RotatingLeftTick3)
                {
                    rotationState = RotationState.Stationary;
                    producerBlock.Post(UserInputEvent.RotateLeft);
                    //Console.WriteLine($"{DateTime.Now} - Left rotation!");
                }
                rotate1Value = e.value;
            }
            else if (e.pin == rotate2Pin)
            {
                // Turning right
                if (rotate1Value == 0 && e.value == 0 && rotationState == RotationState.RotatingRightTick1)
                    rotationState = RotationState.RotatingRightTick2;
                else if (rotate1Value == 0 && e.value == 1 && rotationState == RotationState.RotatingRightTick2)
                    rotationState = RotationState.RotatingRightTick1; // Turned back
                else if (rotate1Value == 1 && e.value == 1 && rotationState == RotationState.RotatingRightTick3)
                {
                    rotationState = RotationState.Stationary;
                    producerBlock.Post(UserInputEvent.RotateRight);
                    //Console.WriteLine($"{DateTime.Now} - Right rotation!");
                }

                //Turning Left
                else if (rotate1Value == 1 && e.value == 0 && rotationState == RotationState.Stationary)
                    rotationState = RotationState.RotatingLeftTick1;
                else if (rotate1Value == 1 && e.value == 1 && rotationState == RotationState.RotatingLeftTick1)
                    rotationState = RotationState.Stationary; // Turned back
                else if (rotate1Value == 0 && e.value == 1 && rotationState == RotationState.RotatingLeftTick2)
                    rotationState = RotationState.RotatingLeftTick3;
                else if (rotate1Value == 0 && e.value == 0 && rotationState == RotationState.RotatingLeftTick3)
                    rotationState = RotationState.RotatingLeftTick2; // Turned back
                rotate2Value = e.value;
            }
            //Console.WriteLine($"Rotation event - pin: {e.pin}, value {e.value}");
        }

        public void Dispose()
        {
            button.Dispose();
            rotate1.Dispose();
            rotate2.Dispose();
            producerBlock.Complete();
        }
    }
}
