namespace ConsoleAppPongFinalProject
{
    public class Ball
    {
        public Coordinate Point { get; private set; }

        public Ball()
        {
            Point = new Coordinate();
            Point.SetCenterBoardPosition();
            SetBallPosition();
        }

        private void SetBallPosition()
        {
            BoardManager.GameField[Point.Y, Point.X] = CharacterUtilities.BALL_ICON;
        }

        public void BallMovementLogic(ref char temp, GameManager gameManager, int xDirection, int yDirection)
        {
            //Saves the last icon (so it won't be deleted).
            gameManager.SaveLastIcon(temp);

            //Changes the ball -X- and -Y- as needed.
            Coordinate point = new Coordinate();
            point.X = xDirection + Point.X;
            point.Y = yDirection + Point.Y;
            Point = point;

            //Sets the last icon the his previous location.
            temp = BoardManager.GameField[Point.Y, Point.X];

            //Sets the ball to his new location.
            SetBallPosition();
        }

        public void SetBackToOrigin()
        {
            BoardManager.GameField[Point.Y, Point.X] = CharacterUtilities.EMPTY_PIXEL;
            Point.SetCenterBoardPosition();
            SetBallPosition();
        }

        public void CreateBallInconsistently(ref int ballYDiraction, ref int ballXDiraction, bool isTwoPlayers)
        {
            //Spawns the ball at Inconsistently coordinates.
            Coordinate point = new Coordinate();
            point.Y = (GameManager.RandomNumer() + GameManager.RandomNumer() + GameManager.RandomNumer() + GameManager.RandomNumer() + BoardManager.GetHalfHight());
            point.X = 2 + GameManager.RandomNumer() + BoardManager.GetHalfWidth();
            Point = point;
            ballYDiraction = GameManager.RandomNumer();

            if (isTwoPlayers)
                ballXDiraction *= (-1);
            else
                ballXDiraction = -1;

            SetBallPosition();
        }
    }
}