namespace ConsoleAppPongFinalProject
{
    class AutoPlayer
    {
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        private int _YDiraction;

        public AutoPlayer(int x, int y)
        {
            XAxis = x;
            YAxis = y;
        }

        public void SetAutoPlayerPosition(char[,] gameField)
        {
            _YDiraction = YAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[_YDiraction, XAxis] = CharacterUtilities.PLAYER_ICON;
                _YDiraction++;
            }
        }

        public void ClearTheColumn(char [,] gameField)
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