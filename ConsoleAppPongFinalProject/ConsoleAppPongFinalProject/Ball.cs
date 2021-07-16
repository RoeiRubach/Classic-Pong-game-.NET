using System;

namespace ConsoleAppPongFinalProject
{
    class Ball
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public int XDirection { get; set; }
        public int YDirection { get; set; }

        private Board _board;

        public Ball(int x, int y, Board board)
        {
            _board = board;
            XPosition = x;
            YPosition = y;
            XDirection = -1;
            YDirection = 0;
            SetBallPosition();
        }

        public void SetBallPosition()
        {
            _board.GameField[YPosition, XPosition] = CharacterUtilities.BALL_ICON;
        }

        public void IncrementBallMovement()
        {
            XPosition += XDirection;
            YPosition += YDirection;
        }

        public void SetBallInconsistently()
        {
            //Spawns the ball at Inconsistently coordinates.
            YPosition = RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + Board.HalfFieldHight;
            XPosition = 2 + RandomNumer() + Board.HalfFieldWidth;
            YDirection = RandomNumer();

            if (GameManager.UserChoice == UserChoice.PVP)
                XDirection *= (-1);
            else
                XDirection = -1;

            SetBallPosition();
        }

        public void ResetBallValue()
        {
            //The next 3 lines resets the ball's coordinates.
            YPosition = Board.HalfFieldHight;
            XPosition = Board.HalfFieldWidth;
            SetBallPosition();
        }

        //A random value for the ball starting position.
        private int RandomNumer()
        {
            Random rnd = new Random();
            int horizontalOrVertical = rnd.Next(0, 3);

            if (horizontalOrVertical == 0)
                horizontalOrVertical = -1;

            else if (horizontalOrVertical == 1)
                horizontalOrVertical = 0;

            else
                horizontalOrVertical = 1;

            return horizontalOrVertical;
        }

        public void IsCollidedWithAnObject(char currentPixel, ref bool isFirstPlayerScored, ref bool isGoal, int firstPlayerY, int secondPlayerY = 0, int AIY = 0)
        {
            if (currentPixel == CharacterUtilities.PLAYER_ICON)
            {
                PaddleEdge collidedWithBall = PaddleEdge.None;
                WhichPaddleEdgeCollidedWithBall(ref collidedWithBall, firstPlayerY, secondPlayerY, AIY);

                switch (collidedWithBall)
                {
                    case PaddleEdge.UpperEdge:
                        YDirection = -1;
                        break;
                    case PaddleEdge.MiddleEdge:
                        YDirection = 0;
                        break;
                    case PaddleEdge.BottomEdge:
                        YDirection = 1;
                        break;
                }
                XDirection *= -1;
            }

            else if (currentPixel == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                YDirection *= (-1);

            else if (currentPixel == CharacterUtilities.LEFT_RIGHT_BORDER_ICON)
            {
                if (XPosition >= 89)
                    isFirstPlayerScored = true;
                else
                    isFirstPlayerScored = false;

                isGoal = true;
            }
        }

        private void WhichPaddleEdgeCollidedWithBall(ref PaddleEdge collidedWithBall, int firstPlayerY, int secondPlayerY, int AIY)
        {
            int currentAIYValue = 0, currentFirstPlayerYValue = firstPlayerY, currentSecondPlayerYValue = 0;

            if (GameManager.UserChoice == UserChoice.PVP)
                currentSecondPlayerYValue = secondPlayerY;
            else
                currentAIYValue = AIY;

            for (int i = 0; i < 5; i++)
            {
                if (GameManager.UserChoice == UserChoice.PVP)
                {
                    if (_board.GameField[currentFirstPlayerYValue, 2] == _board.GameField[YPosition, XPosition] ||
                        (_board.GameField[currentSecondPlayerYValue, Board.FIELD_WIDTH - 3] == _board.GameField[YPosition, XPosition]))
                    {
                        GetCollidedPaddleEdge(ref collidedWithBall, i);
                        break;
                    }
                    currentSecondPlayerYValue++;
                }
                else
                {
                    if (_board.GameField[currentFirstPlayerYValue, 2] == _board.GameField[YPosition, XPosition] ||
                        (_board.GameField[currentAIYValue, Board.FIELD_WIDTH - 3] == _board.GameField[YPosition, XPosition]))
                    {
                        GetCollidedPaddleEdge(ref collidedWithBall, i);
                        break;
                    }
                    currentAIYValue++;
                }
                currentFirstPlayerYValue++;
            }
        }

        private void GetCollidedPaddleEdge(ref PaddleEdge collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = PaddleEdge.UpperEdge;
            else if (i == 2)
                collidedWithBall = PaddleEdge.MiddleEdge;
            else
                collidedWithBall = PaddleEdge.BottomEdge;
        }
    }
}