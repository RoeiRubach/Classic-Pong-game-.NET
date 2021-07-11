namespace ConsoleAppPongFinalProject
{
    class FirstPlayer
    {
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        private int _yDiraction;

        public FirstPlayer(int x, int y)
        {
            XAxis = x;
            YAxis = y;
        }

        public void SetFirstPlayerPosition(char[,] gameField)
        {
            _yDiraction = YAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[_yDiraction, XAxis] = GameManager.PLAYER_ICON;
                _yDiraction++;
            }
        }
    }
}