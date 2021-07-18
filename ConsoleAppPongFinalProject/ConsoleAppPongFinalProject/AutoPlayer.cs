namespace ConsoleAppPongFinalProject
{
    class AutoPlayer : Player
    {
        public AutoPlayer(Board board) : base(board)
        {
        }

        public void HandleAIMovement(ref bool isReachTop)
        {
            if (!board.IsPaddleReachTopBorder(point) && (!isReachTop))
                MoveUp();

            else if (!board.IsPaddleReachBottomBorder(point))
            {
                isReachTop = true;
                MoveDown();
            }
            else
                isReachTop = false;

            SetPosition();
        }

        private void EraseAILeftovers(char[,] gameField)
        {
            for (int i = 1; i < 22; i++)
            {
                for (int j = 87; j <= 87; j++)
                    gameField[i, j] = CharacterUtilities.EMPTY_PIXEL;
            }
        }
    }
}
