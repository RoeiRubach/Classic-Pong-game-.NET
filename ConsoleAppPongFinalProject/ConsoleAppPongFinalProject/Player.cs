using System;

namespace ConsoleAppPongFinalProject
{
    public class Player
    {
        protected static event Action GoalScored;

        public int GoalCount { get; private set; }
        public Coordinate Point;

        public Player()
        {
            GoalCount = 0;
            Point = new Coordinate();
            SetPosition();
        }

        protected void SetPosition()
        {
            int playerLength = Point.Y + 5;

            for (int y = Point.Y; y < playerLength; y++)
                BoardManager.GameField[y, Point.X] = CharacterUtilities.PLAYER_ICON;
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