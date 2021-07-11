namespace ConsoleAppPongFinalProject
{
    class AutoPlayer : Player
    {
        bool isReachTop = false;

        public AutoPlayer() : base()
        {
            playerNum = 2;
            Point.SetSecondPuddlePosition();
            GameManager.UpdateUnits += HandleMovement;
            Ball.PaddleCollisionDetected += OnPaddleCollision;
        }

        private void HandleMovement()
        {
            if (BoardManager.CheckPlayerOutFieldAbove(Point) && (!isReachTop))
                MoveUp();

            else if (BoardManager.CheckPlayerOutFieldBelow(Point))
            {
                isReachTop = true;
                MoveDown();
            }
            else
                isReachTop = false;

            SetPosition();
        }

        public void SetsAIAtMiddle()
        {
            //Sets the auto-player's coordinates at the middle field.
            Point.Y = BoardManager.GetHalfFieldHight() - 2;
            Point.X = BoardManager.GetHalfFieldWidth() - 3;
        }
    }
}