namespace ConsoleAppPongFinalProject
{
    class Board
    {
        public const int FIELD_HIGHT = 24;
        public const int FIELD_WIDTH = 90;
        public const int HalfFieldHight = FIELD_HIGHT / 2;
        public const int HalfFieldWidth = FIELD_WIDTH / 2;

        public char[,] GameField;

        public Board()
        {
            GameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            GameBorder gameBorder = new GameBorder(GameField);
        }

        public void ClearTopPaddleEdge(int y, int x) => GameField[y - 1, x] = CharacterUtilities.EMPTY_PIXEL;

        public void ClearBottomPaddleEdge(int y, int x) => GameField[y + 5, x] = CharacterUtilities.EMPTY_PIXEL;

        public bool IsPaddleReachBottomBorder(int y, int x) => GameField[y + 5, x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;

        public bool IsPaddleReachTopBorder(int y, int x) => GameField[y - 1, x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;
    }
}