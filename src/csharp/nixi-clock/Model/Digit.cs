namespace nixi_clock.Model
{
    public class Digit
    {
        /// <summary>
        /// The on time, have to be a value between 0 and 4096
        /// </summary>
        public short On { get; set; }

        /// <summary>
        /// The off time, have to be a value between 0 and 4096
        /// </summary>
        public short Off { get; set; }

        /// <summary>
        /// Turns the digit fully on and will not use the On/Off values
        /// </summary>
        public bool FullOn { get; set; }

        /// <summary>
        /// Turns the digtig fully off and will not use the On/Off values
        /// </summary>
        public bool FullOff { get; set; }

        public Digit(short On, short Off) : this(On, Off, false, false) { }

        public Digit(short On = 0, short Off = 0, bool FullOn = false, bool FullOff = false)
        {
            this.On = On;
            this.Off = Off;
            this.FullOn = FullOn;
            this.FullOff = FullOff;
        }
    }
}
