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
                    from.Tubes[i].Digits[j].FullOn = to.Tubes[i].Digits[j].FullOn;
                    from.Tubes[i].Digits[j].FullOff = to.Tubes[i].Digits[j].FullOff;
                    if (from.Tubes[i].Digits[j].On != to.Tubes[i].Digits[j].On)
                    {
                        from.Tubes[i].Digits[j].On = LinearInterpolation(
                            from.Tubes[i].Digits[j].On,
                            to.Tubes[i].Digits[j].On,
                            time);
                    }
                    if (from.Tubes[i].Digits[j].Off != to.Tubes[i].Digits[j].Off)
                    {
                        from.Tubes[i].Digits[j].Off = LinearInterpolation(
                            from.Tubes[i].Digits[j].Off,
                            to.Tubes[i].Digits[j].Off,
                            time);
                    }
                }
            }
        }

        private static short LinearInterpolation(short from, short to, float time)
        {
            return (short)(from + (to - from) * time);
        }
    }
}
