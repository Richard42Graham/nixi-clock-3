using nixi_clock.Model;
using System;
using System.Diagnostics;
using System.Linq;

namespace nixi_clock
{
    public class Countdown : IBoardState
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private TimeSpan count = new TimeSpan();
        private readonly Stopwatch startCountdown = new Stopwatch();

        private TimeSpan GetChange()
        {
            if (count.TotalSeconds < 1)
                return TimeSpan.FromMilliseconds(100);

            else if (count.TotalSeconds < 180)
                return TimeSpan.FromSeconds(1);

            else if (count.TotalSeconds < 300)
                return TimeSpan.FromSeconds(10);

            else if (count.TotalSeconds < 1800)
                return TimeSpan.FromSeconds(30);

            return TimeSpan.FromSeconds(60);
        }

        public void Increase()
        {
            count = count.Add(GetChange());

            startCountdown.Restart();
            stopwatch.Stop();
        }

        public void Decrease()
        {
            count = count.Subtract(GetChange());
            if (count < TimeSpan.Zero)
                count = TimeSpan.Zero;
            startCountdown.Restart();
            stopwatch.Stop();
        }


        public Board GetBoard()
        {

            if (!stopwatch.IsRunning && startCountdown.ElapsedMilliseconds > 2000)
            {
                count = count.Subtract(stopwatch.Elapsed);
                if (count < TimeSpan.Zero)
                    count = TimeSpan.Zero;
                stopwatch.Restart();
            }

            if(stopwatch.IsRunning)
            {
                if (count > stopwatch.Elapsed)
                {
                    count = count.Subtract(stopwatch.Elapsed);
                    if (count < TimeSpan.Zero)
                        count = TimeSpan.Zero;
                    stopwatch.Restart();
                }
                else
                {
                    count = TimeSpan.Zero;
                    stopwatch.Reset();
                }
            }

            Board b = new Board();
            b.Tubes[0].Digits[count.Hours / 10].DutyCycle = 1;
            b.Tubes[1].Digits[count.Hours % 10].DutyCycle = 1;
            b.Tubes[2].Digits[count.Minutes / 10].DutyCycle = 1;
            b.Tubes[3].Digits[count.Minutes % 10].DutyCycle = 1;
            b.Tubes[4].Digits[count.Seconds / 10].DutyCycle = 1;
            b.Tubes[5].Digits[count.Seconds % 10].DutyCycle = 1;
            b.Tubes[6].Digits[count.Milliseconds / 100].DutyCycle = 1;

            //Do not render the digit 0 prefixes
            for (int i = 0; i < b.Tubes.Length - 3; i++)
            {
                if (!b.Tubes[i].Digits.Skip(1).Any(d => d.DutyCycle > 0))
                {
                    //If only 0 is on
                    //Turn it off
                    b.Tubes[i].Digits[0].DutyCycle = 0;
                }
                else
                {
                    break;
                }
            }

            return b;
        }
    }
}
