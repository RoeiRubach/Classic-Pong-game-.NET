using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    public class AutoPlayer : Player, IMoveable
    {
        bool isReachTop = false;

        public AutoPlayer() : base()
        {
            Point.SetSecondPuddlePosition();
        }

        public void HandleMovement()
        {
            if (BoardManager.CheckPlayerOutFieldAbove(Point) && (!isReachTop))
                MoveUp();

            else if (BoardManager.CheckPlayerOutFieldAbove(Point))
            {
                isReachTop = true;
                MoveDown();
            }
            else
                isReachTop = false;

            SetPosition();
        }

        public void SetsAIAtMiddle()
        {
            //Sets the auto-player's coordinates at the middle field.
            Point.Y = BoardManager.GetHalfFieldHight() - 2;
            Point.X = BoardManager.GetHalfFieldWidth() - 3;
        }
    }
}