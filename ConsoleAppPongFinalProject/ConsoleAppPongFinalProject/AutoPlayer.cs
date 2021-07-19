namespace ConsoleAppPongFinalProject
{
    class AutoPlayer : Player
    {
        private bool _isReachTop;

        public AutoPlayer(Board board) : base(board)
        {
        }

        public void HandleAIMovement()
        {
            if (!board.IsPaddleReachTopBorder(point) && (!_isReachTop))
                MoveUp();

            else if (!board.IsPaddleReachBottomBorder(point))
            {
                _isReachTop = true;
                MoveDown();
            }
            else
                _isReachTop = false;

            SetPosition();
        }
    }
}