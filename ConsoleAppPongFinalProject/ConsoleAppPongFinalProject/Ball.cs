using System;

namespace ConsoleAppPongFinalProject
{
    class BoardManager
    {
        public const int FIELD_HIGHT = 24;
        public const int FIELD_WIDTH = 90;
        public const int HalfFieldHight = FIELD_HIGHT / 2;
        public const int HalfFieldWidth = FIELD_WIDTH / 2;
        public const int FirstPlayerXPosition = 2;
        public const int SecondPlayerXPosition = FIELD_WIDTH - 3;

        public char[,] GameField;

        public BoardManager()
        {
            GameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            GameBorder gameBorder = new GameBorder(GameField);
        }

        public void PrintGameField()
        {
            UIUtilities.PrintPongTitle();
            for (int i = 0; i < GameField.GetLength(0); i++)
            {
                for (int j = 0; j < GameField.GetLength(1); j++)
                {
                    Console.Write(GameField[i, j]);
                }
                Console.WriteLine();
            }
        }

        public bool IsPointsAreEqual(int y, int x, Point ballPoint) => GameField[y, x] == GameField[ballPoint.y, ballPoint.x];

        public void SetEmptyPixelAtPoint(Point point) => GameField[point.y, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public void ClearTopPaddleAfterStep(Point point) => GameField[point.y - 1, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public void ClearBottomPaddleAfterStep(Point point) => GameField[point.y + 5, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public bool IsPaddleReachBottomBorder(Point point) => GameField[point.y + 5, point.x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;

        public bool IsPaddleReachTopBorder(Point point) => GameField[point.y - 1, point.x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;
    }
}