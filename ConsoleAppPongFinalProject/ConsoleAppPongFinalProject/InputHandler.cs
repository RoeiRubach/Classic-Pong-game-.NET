using System;

namespace ConsoleAppPongFinalProject
{
    class InputHandler
    {
        private BoardManager _board;

        public InputHandler(BoardManager board) => _board = board;

        public void HandlePlayersInput(Player first, Player second)
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (!_board.IsPaddleReachTopBorder(first.PointRef))
                            first.MoveUp();
                        break;

                    case ConsoleKey.DownArrow:
                        if (!_board.IsPaddleReachBottomBorder(first.PointRef))
                            first.MoveDown();
                        break;

                    case ConsoleKey.W:
                        if (second != null && !_board.IsPaddleReachTopBorder(second.PointRef))
                            second.MoveUp();

                        break;
                    case ConsoleKey.S:
                        if (second != null && !_board.IsPaddleReachBottomBorder(second.PointRef))
                            second.MoveDown();
                        break;
                }
            } while (!GameManager.IsGameOver);
        }
    }
}