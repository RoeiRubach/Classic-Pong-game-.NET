using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class ManualPlayer
    {
        public int xAxis { get; set; }
        public int yAxis { get; set; }
        private int yDiraction;

        public ManualPlayer(int x, int y)
        {
            xAxis = x;
            yAxis = y;
        }

        public void SetsTheManualPlayerPosition(char[,] gameField)
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
