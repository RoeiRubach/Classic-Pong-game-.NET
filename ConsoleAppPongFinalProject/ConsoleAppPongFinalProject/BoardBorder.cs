namespace ConsoleAppPongFinalProject
{
    public class BoardBorder
    {
        private char[,] _gameBoard;

        public BoardBorder(char[,] gameBoard)
        {
            _gameBoard = gameBoard;
            SetBorder();
        }

        private void SetBorder()
        {
            for (int i = 0; i < _gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < _gameBoard.GetLength(1); j++)
                {
                    if (i == 0)
                        HandleUpperBorder(i, j);

                    else if (i == _gameBoard.GetLength(0) - 1)
                        HandleBottomBorder(i, j);

                    else
                    {
                        _gameBoard[i, j] = CharacterUtilities.EMPTY_PIXEL;
                        HandleLeftRightEdges(i, j);
                    }
                }
            }
        }

        private void HandleUpperBorder(int i, int j)
        {
            _gameBoard[i, j] = CharacterUtilities.TOP_AND_BOTTOM_EDGES;

            if (j == 0)
                _gameBoard[i, j] = CharacterUtilities.LEFT_UPPER_CORNER;

            else if (j == _gameBoard.GetLength(1) - 1)
                _gameBoard[i, j] = CharacterUtilities.RIGHT_UPPER_CORNER;
        }

        private void HandleBottomBorder(int i, int j)
        {
            _gameBoard[i, j] = CharacterUtilities.TOP_AND_BOTTOM_EDGES;
            if (j == 0)
                _gameBoard[i, j] = CharacterUtilities.LEFT_BOTTOM_CORNER;

            else if (j == _gameBoard.GetLength(1) - 1)
                _gameBoard[i, j] = CharacterUtilities.RIGHT_BOTOOM_CORNER;
        }

        private void HandleLeftRightEdges(int i, int j)
        {
            if ((j == 0) || j == _gameBoard.GetLength(1) - 1)
                _gameBoard[i, j] = CharacterUtilities.LEFT_AND_RIGHT_EDGES;
        }
    }
}
