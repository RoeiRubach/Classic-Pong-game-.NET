using System;

namespace ConsoleAppPongFinalProject
{
    class Ball
    {
        public Point PointRef => _point;

        private Point _point;
        private Point _velocity;
        private Board _board;

        public Ball(Board board)
        {
            _point = new Point(Board.HalfFieldWidth, Board.HalfFieldHight);
            _velocity = new Point(-1, 0);
            _board = board;
            SetBallPosition();
        }

        public void SetBallPosition() => _board.GameField[_point.Y, _point.X] = CharacterUtilities.BALL_ICON;

        public void IncrementBallMovement() => _point += _velocity;

        public void SpawnBallRandomPosition()
        {
            _point.Y = GetRandomPosition() + Board.HalfFieldHight;
            _point.X = GetRandomDirection() + Board.HalfFieldWidth;
            _velocity.Y = GetRandomDirection();

            if (GameManager.GameMode == GameMode.PVP)
                _velocity.X *= -1;
            else
                _velocity.X = -1;

            SetBallPosition();
        }

        private int GetRandomPosition()
        {
            Random rnd = new Random();
            return rnd.Next(0, 5);
        }

        private int GetRandomDirection()
        {
            Random rnd = new Random();
            return rnd.Next(-1, 2);
        }

        public void CheckCollision(char currentPixel, ref bool isFirstPlayerScored, ref bool isGoal, Point firstPlayer, Point secondPlayer)
        {
            if (currentPixel == CharacterUtilities.PLAYER_ICON)
                HandlePaddleCollision(firstPlayer, secondPlayer);

            else if (currentPixel == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                _velocity.Y *= (-1);

            else if (currentPixel == CharacterUtilities.LEFT_RIGHT_BORDER_ICON)
                HandleGoalScored(out isFirstPlayerScored, out isGoal);
        }

        private void HandlePaddleCollision(Point firstPlayer, Point secondPlayer)
        {
            PaddleEdge collidedWithBall = PaddleEdge.None;
            WhichPaddleEdgeCollidedWithBall(ref collidedWithBall, firstPlayer, secondPlayer);

            switch (collidedWithBall)
            {
                case PaddleEdge.UpperEdge:
                    _velocity.Y = -1;
                    break;
                case PaddleEdge.MiddleEdge:
                    _velocity.Y = 0;
                    break;
                case PaddleEdge.BottomEdge:
                    _velocity.Y = 1;
                    break;
            }
            _velocity.X *= -1;
        }

        private void WhichPaddleEdgeCollidedWithBall(ref PaddleEdge collidedWithBall, Point firstPlayer, Point secondPlayer)
        {
            for (int i = 0; i < 5; i++)
            {
                Point first = new Point(firstPlayer.X, firstPlayer.Y + i);
                Point second = new Point(secondPlayer.X + i, secondPlayer.Y);
                if (FoundHittedPart(first, second))
                {
                    GetCollidedPaddleEdge(ref collidedWithBall, i);
                    break;
                }
            }
        }

        private bool FoundHittedPart(Point a, Point b) => a == _point || b == _point;

        private void GetCollidedPaddleEdge(ref PaddleEdge collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = PaddleEdge.UpperEdge;
            else if (i == 2)
                collidedWithBall = PaddleEdge.MiddleEdge;
            else
                collidedWithBall = PaddleEdge.BottomEdge;
        }

        private void HandleGoalScored(out bool isFirstPlayerScored, out bool isGoal)
        {
            if (_point.X >= 89)
                isFirstPlayerScored = true;
            else
                isFirstPlayerScored = false;

            isGoal = true;
        }
    }
}