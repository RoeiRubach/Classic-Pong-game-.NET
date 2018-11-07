using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Board
    {

        public void SetsTheBoard(char[,] gameField)
        {
            for (int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    if (i == 0)
                    {
                        gameField[i, j] = GameManager.topBottomEdge;
                        if (j == 0)
                        {
                            gameField[i, j] = '╔';
                        }
                        else if (j == gameField.GetLength(1) - 1)
                        {
                            gameField[i, j] = '╗';
                        }
                    }
                    else if (i == gameField.GetLength(0) - 1)
                    {
                        gameField[i, j] = GameManager.topBottomEdge;
                        if (j == 0)
                        {
                            gameField[i, j] = '╚';
                        }
                        else if (j == gameField.GetLength(1) - 1)
                        {
                            gameField[i, j] = '╝';
                        }
                    }
                    else
                    {
                        gameField[i, j] = ' ';
                        if ((j == 0) || j == gameField.GetLength(1) - 1)
                        {
                            gameField[i, j] = GameManager.leftRightEdge;
                        }
                    }
                }
            }
        }
    }
}
