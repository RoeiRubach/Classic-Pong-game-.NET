namespace ConsoleAppPongFinalProject
{
    class SecondPlayer
    {
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        private int _yDiraction;

        public SecondPlayer(int x, int y)
        {
            XAxis = x;
            YAxis = y;
        }

        public void SetSecondPlayerPosition(char[,] gameField)
        {
            _yDiraction = YAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[_yDiraction, XAxis] = GameManager.PLAYER_ICON;
                _yDiraction++;
            }
        }

        public void ClearColumn(char[,] gameField)
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