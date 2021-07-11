using System;

namespace ConsoleAppPongFinalProject
{
    class FirstPlayer : Player, IMoveable
    {
        public FirstPlayer() : base()
        {
            playerNum = 1;
            PlayerName = null;
            GoalCount = 0;
            scoreDisplayHandler = new ScoreDisplayHandler();
            Point.SetFirstPuddlePosition();
            Ball.PaddleCollisionDetected += OnPaddleCollision;
            Ball.GoalScored += OnGoalScored;
        }

        public void HandleMovement()
        {
            do
            {
                if (GoalCount == GameManager.GOALS_TO_REACH)
                {
                    Console.SetCursorPosition(0, 28);
                    Console.WriteLine("Press any key to continue..");
                    Console.ReadKey(true);
                }

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (BoardManager.CheckPlayerOutFieldAbove(Point))
                            MoveUp();
                        break;

                    case ConsoleKey.DownArrow:
                        if (BoardManager.CheckPlayerOutFieldBelow(Point))
                            MoveDown();
                        break;
                }
                SetPosition();
            } while (!GameManager.IsGameOver);
        }
    }
}