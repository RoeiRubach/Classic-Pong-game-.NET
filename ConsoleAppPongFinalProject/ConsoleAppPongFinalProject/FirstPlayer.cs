using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    public class FirstPlayer : Player, IMoveable
    {
        public FirstPlayer() : base()
        {
            Point.SetFirstPuddlePosition();
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