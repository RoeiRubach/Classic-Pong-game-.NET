using System;

namespace ConsoleAppPongFinalProject
{
    public class Player
    {
        public int GoalCount;
        public Coordinate Point;

        public Player(Coordinate point)
        {
            GoalCount = 0;
            Point = point;
            SetPlayerPosition();
        }

        private void SetPlayerPosition()
        {
            int playerLength = Point.Y + 5;

            for (int i = Point.Y; i < playerLength; i++)
                BoardManager.GameField[Point.Y, Point.X] = CharacterUtilities.PLAYER_ICON;
        }

        public void StartPlayerLoop()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (BoardManager.CheckPlayerOutFieldAbove(Point))
                        {
                            Point.Y--;
                            BoardManager.GameField[Point.Y + 5, Point.X] = CharacterUtilities.EMPTY_PIXEL;
                        }
                        SetPlayerPosition();
                        break;
                    case ConsoleKey.DownArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (BoardManager.CheckPlayerOutFieldBelow(Point))
                        {
                            Point.Y++;
                            BoardManager.GameField[Point.Y - 1, Point.X] = CharacterUtilities.EMPTY_PIXEL;
                        }
                        SetPlayerPosition();
                        break;

                    default:
                        //Checks if the game has ended.
                        if ((GoalCount == GameManager.GOALS_TO_REACH) || (GameManager.AIGoalCount == GameManager.GOALS_TO_REACH) || (GameManager.SecondPlayerGoalCount == GameManager.GOALS_TO_REACH))
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey(true);
                        }
                        break;
                }
            } while (!GameManager.IsGameOver);
        }
    }
}