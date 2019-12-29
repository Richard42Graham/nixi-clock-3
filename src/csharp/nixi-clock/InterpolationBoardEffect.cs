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
                    if (from.Tubes[i].Digits[j].DutyCycle != to.Tubes[i].Digits[j].DutyCycle)
                    {
                        from.Tubes[i].Digits[j].DutyCycle = LinearInterpolation(
                            from.Tubes[i].Digits[j].DutyCycle,
                            to.Tubes[i].Digits[j].DutyCycle,
                            time);
                    }
                }
            }
        }

        private static double LinearInterpolation(double from, double to, float time)
        {
            return from + (to - from) * time;
        }
    }
}
