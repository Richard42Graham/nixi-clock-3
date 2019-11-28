using nixi_clock.Model;

namespace nixi_clock
{
    public class Countdown : IBoardState
    {
        public Board GetBoard()
        {
            return new Board();
        }
    }
}
