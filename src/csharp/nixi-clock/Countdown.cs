using nixi_clock.Model;
using System;
using System.Diagnostics;

namespace nixi_clock
{
    public class Countdown : IBoardState
    {
        private readonly Stopwatch stopwatch = new Stopwatch();
        private TimeSpan count = new TimeSpan();

        public void Increase()
        {
            if (count.TotalSeconds < 1)
                count = count.Add(TimeSpan.FromMilliseconds(100));
             else 
                count = count.Add(TimeSpan.FromSeconds(count.TotalSeconds * 0.1));
        }

        public void Decrease()
        {
            count = count.Subtract(TimeSpan.FromSeconds(count.TotalSeconds * 0.1));
        }

        public void Start()
        {
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public void Reset()
        {
            count = new TimeSpan();
            stopwatch.Stop();
            stopwatch.Reset();
        }

        public Board GetBoard()
        {
            TimeSpan timeLeft;
            if (count > stopwatch.Elapsed)
                timeLeft = count.Subtract(stopwatch.Elapsed);
            else
                timeLeft = TimeSpan.Zero;

            Board b = new Board();
            b.Tubes[0].Digits[timeLeft.Hours / 10].DutyCycle = 1;
            b.Tubes[1].Digits[timeLeft.Hours % 10].DutyCycle = 1;
            b.Tubes[2].Digits[timeLeft.Minutes / 10].DutyCycle = 1;
            b.Tubes[3].Digits[timeLeft.Minutes % 10].DutyCycle = 1;
            b.Tubes[4].Digits[timeLeft.Seconds / 10].DutyCycle = 1;
            b.Tubes[5].Digits[timeLeft.Seconds % 10].DutyCycle = 1;
            b.Tubes[6].Digits[timeLeft.Milliseconds / 100].DutyCycle = 1;
            return b;
        }
    }
}
