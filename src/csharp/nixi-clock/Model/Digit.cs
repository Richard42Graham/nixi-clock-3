namespace nixi_clock.Model
{
    public class Digit
    {
        /// <summary>
        /// How much of the time the digit is on
        /// Value between 0 and 1
        /// </summary>
        public double DutyCycle { get; set; }

        /// <summary>
        /// Turns the digit fully on and will not use the On/Off values
        /// </summary>
        public bool FullOn { get; set; }

        /// <summary>
        /// Turns the digtig fully off and will not use the On/Off values
        /// </summary>
        public bool FullOff { get; set; }

        public Digit(double DutyCycle) : this(DutyCycle, false, false) { }

        public Digit(double DutyCycle = 0, bool FullOn = false, bool FullOff = false)
        {
            this.DutyCycle = DutyCycle;
            this.FullOn = FullOn;
            this.FullOff = FullOff;
        }
    }
}
