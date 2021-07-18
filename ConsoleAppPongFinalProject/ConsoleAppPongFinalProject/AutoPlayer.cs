using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class AutoPlayer : Player
    {
        public AutoPlayer(Board board) : base(board)
        {
        }

        public void HandleAIMovement(ref bool isReachTop)
        {
            if (!_board.IsPaddleReachTopBorder(point) && (!isReachTop))
                MoveUp();

            else if (!_board.IsPaddleReachBottomBorder(point))
            {
                isReachTop = true;
                MoveDown();
            }
            else
                isReachTop = false;

            SetPlayerPosition();
        }

        //private void SetAIAtMid()
        //{
        //    //Sets the auto-player's coordinates at the middle field.
        //    EraseAILeftovers(_board.GameField);
        //    YAxis = Board.HalfFieldHight - 2;
        //    XAxis = Board.FIELD_WIDTH - 3;
        //    SetPlayerPosition();
        //}

        private void EraseAILeftovers(char[,] gameField)
        {
            for (int i = 1; i < 22; i++)
            {
                for (int j = 87; j <= 87; j++)
                    gameField[i, j] = CharacterUtilities.EMPTY_PIXEL;
            }
        }
    }
}
