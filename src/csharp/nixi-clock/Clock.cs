using nixi_clock.Model;
using System;

namespace nixi_clock
{
    public class Clock : IBoardState
    {
        private Board currentState = new Board();
        public Board GetBoard()
        {
            DateTime now = DateTime.Now;
            Board b = new Board();
            b.Tubes[0].Digits[now.Hour / 10].DutyCycle = 1;
            b.Tubes[1].Digits[now.Hour % 10].DutyCycle = 1;
            b.Tubes[2].Digits[now.Minute / 10].DutyCycle = 1;
            b.Tubes[3].Digits[now.Minute % 10].DutyCycle = 1;
            b.Tubes[4].Digits[now.Second / 10].DutyCycle = 1;
            b.Tubes[5].Digits[now.Second % 10].DutyCycle = 1;
            //b.Tubes[6].Digits[now.Millisecond / 100].DutyCycle = 1;

            //b.Tubes[0].Digits[now.Second / 10].DutyCycle = 1;
            //b.Tubes[1].Digits[now.Second % 10].DutyCycle = 1;
            currentState.Interpolate(b, 0.2f);
            
            return currentState;
        }
    }
}
