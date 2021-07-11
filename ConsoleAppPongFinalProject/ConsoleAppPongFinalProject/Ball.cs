using System;

namespace ConsoleAppPongFinalProject
{
    class Ball
    {
        public static event Action PaddleCollisionDetected;
        public static event Action<int> GoalScored;

        public static Ball Instance { get; private set; }

        private Coordinate _velocity;
        public Coordinate Position;

        public Ball()
        {
            Instance = this;
            Position = new Coordinate();
            Position.SetCenterBoardPosition();
            _velocity = new Coordinate();
            _velocity.X = 1;
            _velocity.Y = 0;
            SetBallPosition();
            GameManager.CollisionCheck += CheckBallCollision;
            GameManager.UpdateUnits += HandleMovement;
        }

        private void CheckBallCollision()
        {
            char currentPixel = BoardManager.GameField[Position.X, Position.Y];
            if (currentPixel == CharacterUtilities.EMPTY_PIXEL) return;

            if (IsPaddleCollision(currentPixel))
                PaddleCollisionDetected?.Invoke();

            else if (IsTopBottomCollision(currentPixel))
                _velocity.Y *= -1;

            else if (IsGoal(currentPixel))
            {
                if (Position.X >= 89)
                    GoalScored?.Invoke(1);
                else
                    GoalScored?.Invoke(2);
            }
        }

        private void HandleMovement()
        {
            char lastChar = BoardManager.GameField[Position.Y, Position.X];
            if(lastChar == CharacterUtilities.PLAYER_ICON || lastChar == CharacterUtilities.TOP_AND_BOTTOM_EDGES)
            {

            }

            ///Changes the ball -X- and -Y- as needed.
            //Coordinate point = new Coordinate();
            //point.X = _velocity.X + Position.X;
            //point.Y = _velocity.Y + Position.Y;
            //Position = point;
            Position.X += _velocity.X;
            Position.Y += _velocity.Y;

            SetBallPosition();

            //temp = BoardManager.GameField[Position.Y, Position.X];
        }

        private void SetBallPosition()
        {
            BoardManager.GameField[Position.Y, Position.X] = CharacterUtilities.BALL_ICON;
        }

        public void ChangeBallDirection(PaddleEdge paddleEdge)
        {
            switch (paddleEdge)
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

        private bool IsPaddleCollision(char collision) => collision == CharacterUtilities.PLAYER_ICON;

        private bool IsTopBottomCollision(char collision) => collision == CharacterUtilities.TOP_AND_BOTTOM_EDGES;
        private bool IsGoal(char collision) => collision == CharacterUtilities.LEFT_AND_RIGHT_EDGES;

        public void BallMovementLogic(ref char temp, GameManager gameManager, int xDirection, int yDirection)
        {
            //Saves the last icon (so it won't be deleted).
            gameManager.SaveLastIcon(temp);

            //Changes the ball -X- and -Y- as needed.
            Coordinate point = new Coordinate();
            point.X = xDirection + Position.X;
            point.Y = yDirection + Position.Y;
            Position = point;

            //Sets the last icon the his previous location.
            temp = BoardManager.GameField[Position.Y, Position.X];

            //Sets the ball to his new location.
            SetBallPosition();
        }

        public void SetBackToOrigin()
        {
            BoardManager.GameField[Position.Y, Position.X] = CharacterUtilities.EMPTY_PIXEL;
            Position.SetCenterBoardPosition();
            SetBallPosition();
        }

        public void CreateBallInconsistently(ref int ballYDiraction, ref int ballXDiraction, bool isTwoPlayers)
        {
            //Spawns the ball at Inconsistently coordinates.
            Coordinate point = new Coordinate();
            ballYDiraction = GameManager.RandomNumer();

            if (isTwoPlayers)
                ballXDiraction *= -1;
            else
                ballXDiraction = -1;

            SetBallPosition();
        }
    }
}