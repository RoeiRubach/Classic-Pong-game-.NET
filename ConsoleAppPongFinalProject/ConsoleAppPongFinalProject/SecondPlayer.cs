using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class SecondPlayer
    {
        public int xAxis { get; set; }
        public int yAxis { get; set; }
        private int yDiraction;

        public SecondPlayer(int x, int y)
        {
            xAxis = x;
            yAxis = y;
        }

        public void SetsTheSecondPlayerPosition(char[,] gameField)
        {
            yDiraction = yAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[yDiraction, xAxis] = GameManager.playerIcon;
                yDiraction++;
            }
        }

        public void ClearTheColumn(char[,] gameField)
        {
            for (int i = 1; i < 22; i++)
            {
                for (int j = 87; j <= 87; j++)
                {
                    gameField[i, j] = ' ';
                }
            }
        }
    }
}
