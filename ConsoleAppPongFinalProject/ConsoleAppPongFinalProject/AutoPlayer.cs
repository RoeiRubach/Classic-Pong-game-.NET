using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class AutoPlayer : Player
    {
        public AutoPlayer(int x, int y, Board board) : base(x, y, board)
        {
        }

        public void HandleAIMovement(ref bool isReachTop)
        {
            if (!_board.IsPaddleReachTopBorder(YAxis, XAxis) && (!isReachTop))
            {
                YAxis--;
                _board.ClearBottomPaddleEdge(YAxis, XAxis);
            }
            else if (!_board.IsPaddleReachBottomBorder(YAxis, XAxis))
            {
                isReachTop = true;
                YAxis++;
                _board.ClearTopPaddleEdge(YAxis, XAxis);
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
