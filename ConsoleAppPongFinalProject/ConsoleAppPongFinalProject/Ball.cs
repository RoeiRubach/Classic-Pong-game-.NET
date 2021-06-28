namespace ConsoleAppPongFinalProject
{
    class Ball
    {
        public int XAxis { get; set; }
        public int YAxis { get; set; }

        public Ball(int x, int y)
        {
            XAxis = x;
            YAxis = y;
        }

        public void SetBallPosition(char[,] gameField)
        {
            gameField[YAxis, XAxis] = CharacterUtilities.BALL_ICON;
        }
    }
}
