using System;

namespace ConsoleAppPongFinalProject
{
    class Player
    {
        public static event Action GameOver;
        public static event Action<int, int> PlayerScored;

        public string PlayerName { get; protected set; }
        public int GoalCount { get; protected set; }
        public Coordinate Point;

        protected ScoreDisplayHandler scoreDisplayHandler;
        protected int playerNum;

        public Player()
        {
            Point = new Coordinate();
            SetPosition();
        }

        protected void SetPosition()
        {
            int playerLength = Point.Y + 5;

            for (int y = Point.Y; y < playerLength; y++)
                BoardManager.GameField[y, Point.X] = CharacterUtilities.PLAYER_ICON;
        }

        protected void OnPaddleCollision()
        {
            int currentY = Point.Y;

            for (int paddleEdge = 0; paddleEdge < 5; paddleEdge++)
            {
                if(BoardManager.GameField[currentY, Point.X] == CharacterUtilities.BALL_ICON)
                {
                    if ((paddleEdge == 0) || (paddleEdge == 1))
                        Ball.Instance.ChangeBallDirection(PaddleEdge.UpperEdge);
                    else if (paddleEdge == 2)
                        Ball.Instance.ChangeBallDirection(PaddleEdge.MiddleEdge);
                    else
                        Ball.Instance.ChangeBallDirection(PaddleEdge.BottomEdge);
                }
                else
                    currentY++;
            }
        }

        protected void OnGoalScored(int whichPlayer)
        {
            if (whichPlayer != playerNum) return;

            GoalCount++;
            PlayerScored?.Invoke(GoalCount, whichPlayer);

            if (GoalCount == GameManager.GOALS_TO_REACH)
                GameOver?.Invoke();
        }

        protected void SetPlayerName()
        {

        }

        protected void MoveUp()
        {
            Point.Y--;
            BoardManager.GameField[Point.Y + 5, Point.X] = CharacterUtilities.EMPTY_PIXEL;
        }

        protected void MoveDown()
        {
            Point.Y++;
            BoardManager.GameField[Point.Y - 1, Point.X] = CharacterUtilities.EMPTY_PIXEL;
        }
    }
}