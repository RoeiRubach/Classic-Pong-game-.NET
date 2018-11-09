using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class FirstPlayer
    {
        public int xAxis { get; set; }
        public int yAxis { get; set; }
        private int yDiraction;

        public FirstPlayer(int x, int y)
        {
            xAxis = x;
            yAxis = y;
        }

        public void SetsTheFirstPlayerPosition(char[,] gameField)
        {
            yDiraction = yAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[yDiraction, xAxis] = GameManager.playerIcon;
                yDiraction++;
            }
        }
    }
}
