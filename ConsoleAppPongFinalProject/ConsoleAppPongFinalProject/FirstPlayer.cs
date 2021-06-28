namespace ConsoleAppPongFinalProject
{
    class FirstPlayer
    {
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        private int _YDiraction;

        public FirstPlayer(int x, int y)
        {
            XAxis = x;
            YAxis = y;
        }

        public void SetFirstPlayerPosition(char[,] gameField)
        {
            _YDiraction = YAxis;
            for (int i = 0; i < 5; i++)
            {
                gameField[_YDiraction, XAxis] = CharacterUtilities.PLAYER_ICON;
                _YDiraction++;
            }
        }
    }
}
