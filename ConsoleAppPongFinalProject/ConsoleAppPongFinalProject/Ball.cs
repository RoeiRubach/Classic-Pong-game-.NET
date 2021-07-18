using System;

namespace ConsoleAppPongFinalProject
{
    class BoardManager
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

        public void SetBallPosition()
        {
            _board.GameField[_point.y, _point.x] = CharacterUtilities.BALL_ICON;
        }

        public void IncrementBallMovement() => _point += _velocity;

        public void SetBallInconsistently()
        {
            //Spawns the ball at Inconsistently coordinates.
            _point.y = RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + Board.HalfFieldHight;
            _point.x = 2 + RandomNumer() + Board.HalfFieldWidth;
            _velocity.y = RandomNumer();

            if (GameManager.GameMode == GameMode.PVP)
                _velocity.x *= -1;
            else
                _velocity.x = -1;

            SetBallPosition();
        }

        public void ResetBallValue()
        {
            _point.SetCenter();
            SetBallPosition();
        }

        //A random value for the ball starting position.
        private int RandomNumer()
        {
            Random rnd = new Random();
            int horizontalOrVertical = rnd.Next(0, 3);

            if (horizontalOrVertical == 0)
                horizontalOrVertical = -1;

            else if (horizontalOrVertical == 1)
                horizontalOrVertical = 0;

            else
                horizontalOrVertical = 1;

            return horizontalOrVertical;
        }

        public void IsCollidedWithAnObject(char currentPixel, ref bool isFirstPlayerScored, ref bool isGoal, int firstPaddleY, int secondPaddleY)
        {
            if (currentPixel == CharacterUtilities.PLAYER_ICON)
            {
                PaddleEdge collidedWithBall = PaddleEdge.None;
                WhichPaddleEdgeCollidedWithBall(ref collidedWithBall, firstPaddleY, secondPaddleY);

                switch (collidedWithBall)
                {
                    case PaddleEdge.UpperEdge:
                        _velocity.y = -1;
                        break;
                    case PaddleEdge.MiddleEdge:
                        _velocity.y = 0;
                        break;
                    case PaddleEdge.BottomEdge:
                        _velocity.y = 1;
                        break;
                }
                _velocity.x *= -1;
            }

            else if (currentPixel == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                _velocity.y *= (-1);

            else if (currentPixel == CharacterUtilities.LEFT_RIGHT_BORDER_ICON)
            {
                if (_point.x >= 89)
                    isFirstPlayerScored = true;
                else
                    isFirstPlayerScored = false;

                isGoal = true;
            }
        }

        private void WhichPaddleEdgeCollidedWithBall(ref PaddleEdge collidedWithBall, int firstPlayerY, int secondPlayerY)
        {
            int firstPlayerPaddlePart = firstPlayerY;
            int secondPlayerPaddlePart = secondPlayerY;

            for (int i = 0; i < 5; i++)
            {
                if (FoundHittedPart(firstPlayerPaddlePart, secondPlayerPaddlePart))
                {
                    GetCollidedPaddleEdge(ref collidedWithBall, i);
                    break;
                }
                secondPlayerPaddlePart++;
                firstPlayerPaddlePart++;
            }
        }

        private bool FoundHittedPart(int firstPlayerPaddlePart, int secondPlayerPaddlePart)
        {
            return _board.IsPointsAreEqual(firstPlayerPaddlePart, Board.FirstPlayerXPosition, _point) ||
                                _board.IsPointsAreEqual(secondPlayerPaddlePart, Board.SecondPlayerXPosition, _point);
        }

        private void GetCollidedPaddleEdge(ref PaddleEdge collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = PaddleEdge.UpperEdge;
            else if (i == 2)
                collidedWithBall = PaddleEdge.MiddleEdge;
            else
                collidedWithBall = PaddleEdge.BottomEdge;
        }

        public bool IsPointsAreEqual(int y, int x, Point ballPoint) => GameField[y, x] == GameField[ballPoint.y, ballPoint.x];

        public void SetEmptyPixelAtPoint(Point point) => GameField[point.y, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public void ClearTopPaddleAfterStep(Point point) => GameField[point.y - 1, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public void ClearBottomPaddleAfterStep(Point point) => GameField[point.y + 5, point.x] = CharacterUtilities.EMPTY_PIXEL;

        public bool IsPaddleReachBottomBorder(Point point) => GameField[point.y + 5, point.x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;

        public bool IsPaddleReachTopBorder(Point point) => GameField[point.y - 1, point.x] == CharacterUtilities.TOP_BOTTOM_BORDER_ICON;
    }
}