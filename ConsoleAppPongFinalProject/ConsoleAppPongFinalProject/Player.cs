namespace ConsoleAppPongFinalProject
{
    class Player
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }

        public Player(int x, int y, char[,] gameField)
        {
            Score = 0;
            XAxis = x;
            YAxis = y;
            SetPlayerPosition(gameField);
        }

        public void IncreaseScoreByOne()
        {
            Score++;

            if (Score == GameManager.GOALS_TO_REACH)
            {
                //end game
            }
        }

        public void SetPlayerPosition(char[,] gameField)
        {
            for (int i = YAxis; i < YAxis + 5; i++)
                gameField[i, XAxis] = CharacterUtilities.PLAYER_ICON;
        }
    }
}