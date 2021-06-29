namespace ConsoleAppPongFinalProject
{
    public class BoardManager
    {
        public const int FIELD_HIGHT = 23;
        public const int FIELD_WIDTH = 90;
        public char[,] GameField { get; private set; }

        public BoardManager()
        {
            GameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            Border border = new Border(GameField);
        }
    }
}