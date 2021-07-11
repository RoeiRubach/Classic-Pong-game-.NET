using System;

namespace ConsoleAppPongFinalProject
{
    class SecondPlayer : Player, IMoveable
    {
        public SecondPlayer() : base()
        {
            Point.SetSecondPuddlePosition();
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
                    case ConsoleKey.W:
                        if (BoardManager.CheckPlayerOutFieldAbove(Point))
                            MoveUp();
                        break;

                    case ConsoleKey.S:
                        if (BoardManager.CheckPlayerOutFieldBelow(Point))
                            MoveDown();
                        break;
                }
                SetPosition();
            } while (!GameManager.IsGameOver);
        }
    }
}