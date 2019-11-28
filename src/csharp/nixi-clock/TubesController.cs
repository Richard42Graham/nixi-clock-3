using nixi_clock.Model;
using nixi_clock.Native;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace nixi_clock
{
    public class TubesController
    {
        private enum States
        {
            Stopped,
            Running,
            Stopping
        }

        public int FramesPerSecond = 144;

        private Thread tubeThread = null;
        private States State = States.Running;
        private readonly object lockObject = new object();
        private readonly int fd;

        private IBoardState clock = new Clock();
        private IBoardState countdown = new Countdown();
        private IBoardState current;

        public TubesController(int fd)
        {
            current = clock;
            this.fd = fd;
        }

        public void Start()
        {
            lock (lockObject)
            {
                if (State == States.Stopped)
                {
                    tubeThread = new Thread(Run);
                    tubeThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (State == States.Running)
                {
                    State = States.Stopping;
                }
            }
            tubeThread.Join();
        }

        public void Run()
        {
            var currentBoard = new Board();
            while (State == States.Running)
            {
                var board = current.GetBoard();
                //currentBoard.Interpolate(board, 0.1f);
                UpdateBoard(board);
                Thread.Sleep(1000 / FramesPerSecond);
            }
        }

        private void UpdateBoard(Board board)
        {
            for (int i = 0; i < board.Tubes.Length; i++)
            {
                for (int j = 0; j < board.Tubes[i].Digits.Length; j++)
                {
                    Tubes.set_tube(fd, i, j, board.Tubes[i].Digits[j]);
                }
            }
        }
    }
}
