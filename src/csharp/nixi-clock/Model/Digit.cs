namespace nixi_clock.Model
{
    public class Digit
    {
        public int Number { get; set; }
        public byte Brightness { get; set; }

        public Digit(int Number, byte Brightness)
        {
            this.Number = Number;
            this.Brightness = Brightness;
        }
    }
}
