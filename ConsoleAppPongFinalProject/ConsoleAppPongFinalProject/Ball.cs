using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Ball
    {
        public int xAxis { get; set; }
        public int yAxis { get; set; }

        public Ball(int x, int y)
        {
            xAxis = x;
            yAxis = y;
        }

        public void SetsTheBallPosition(char[,] gameField)
        {
            gameField[yAxis, xAxis] = GameManager.ballIcon;
        }
    }
}
