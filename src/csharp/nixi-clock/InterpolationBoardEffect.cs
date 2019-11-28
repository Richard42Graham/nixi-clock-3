using nixi_clock.Model;

namespace nixi_clock
{
    public static class InterpolationBoardEffect
    {
        public static void Interpolate(this Board from, Board to, float time)
        {
            for (int i = 0; i < from.Tubes.Length && i < to.Tubes.Length; i++)
            {
                for (int j = 0; j < from.Tubes[i].Digits.Length && i < to.Tubes[i].Digits.Length; j++)
                {
                    from.Tubes[i].Digits[j] = LinearInterpolation(
                        from.Tubes[i].Digits[j],
                        to.Tubes[i].Digits[j], 
                        time);
                }
            }
        }

        private static byte LinearInterpolation(byte from, byte to, float time)
        {
            return (byte)(from + (to - from) * time);
        }
    }
}
