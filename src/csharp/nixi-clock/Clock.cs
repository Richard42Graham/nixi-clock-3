using nixi_clock.Model;
using System;

namespace nixi_clock
{
    public class Clock : IBoardState
    {
        public byte Brightness { get; set; } = 255;

        public Board GetBoard()
        {
            DateTime now = DateTime.Now;
            Board b = new Board();
            b.Tubes[0].Digits[now.Hour / 10] = Brightness;
            b.Tubes[1].Digits[now.Hour % 10] = Brightness;
            b.Tubes[2].Digits[now.Minute / 10] = Brightness;
            b.Tubes[3].Digits[now.Minute % 10] = Brightness;
            b.Tubes[4].Digits[now.Second / 10] = Brightness;
            b.Tubes[5].Digits[now.Second % 10] = Brightness;
            b.Tubes[6].Digits[now.Millisecond / 100] = Brightness;
            return b;
        }
    }
}
