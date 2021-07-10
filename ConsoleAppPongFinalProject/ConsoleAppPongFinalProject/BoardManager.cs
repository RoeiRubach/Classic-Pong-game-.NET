using System;

namespace ConsoleAppPongFinalProject
{
    public class BoardManager
    {
        private const int FIELD_HIGHT = 23;
        private const int FIELD_WIDTH = 90;

        //private char[,] _gameField;
        //public char this[int x, int y]
        //{
        //    get
        //    {
        //        return _gameField[x, y];
        //    }
        //}

        public static char[,] GameField { get; private set; }

        public BoardManager()
        {
            //_gameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            GameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            BoardBorder border = new BoardBorder(GameField);
        }

        public void PrintGameField()
        {
            UserInterfaceUtilities.PrintColoredPongTitle();
            for (int i = 0; i < GameField.GetLength(0); i++)
            {
                for (int j = 0; j < GameField.GetLength(1); j++)
                {
                    Console.Write(GameField[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static int GetHalfFieldHight() => FIELD_HIGHT / 2;
        public static int GetHalfFieldWidth() => FIELD_WIDTH / 2;

        public static bool CheckPlayerOutFieldAbove(Coordinate point)
        {
            return GameField[point.Y - 1, point.X] != CharacterUtilities.TOP_AND_BOTTOM_EDGES;
        }

        public static bool CheckPlayerOutFieldBelow(Coordinate point)
        {
            return GameField[point.Y + 5, point.X] != CharacterUtilities.TOP_AND_BOTTOM_EDGES;
        }
    }
}