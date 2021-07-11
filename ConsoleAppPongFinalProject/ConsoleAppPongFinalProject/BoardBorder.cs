namespace ConsoleAppPongFinalProject
{
    public class BoardBorder
    {
        public BoardBorder(char[,] gameBoard) => SetBorder(gameBoard);

        private void SetBorder(char[,] gameBoard)
        {
            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    if (i == 0)
                        HandleUpperBorder(gameBoard, i, j);

                    else if (i == gameBoard.GetLength(0) - 1)
                        HandleBottomBorder(gameBoard, i, j);

                    else
                    {
                        gameBoard[i, j] = CharacterUtilities.EMPTY_PIXEL;
                        HandleLeftRightEdges(gameBoard, i, j);
                    }
                }
            }
        }

        private void HandleUpperBorder(char[,] gameBoard, int i, int j)
        {
            gameBoard[i, j] = CharacterUtilities.TOP_AND_BOTTOM_EDGES;

            if (j == 0)
                gameBoard[i, j] = CharacterUtilities.LEFT_UPPER_CORNER;

            else if (j == gameBoard.GetLength(1) - 1)
                gameBoard[i, j] = CharacterUtilities.RIGHT_UPPER_CORNER;
        }

        private void HandleBottomBorder(char[,] gameBoard, int i, int j)
        {
            gameBoard[i, j] = CharacterUtilities.TOP_AND_BOTTOM_EDGES;
            if (j == 0)
                gameBoard[i, j] = CharacterUtilities.LEFT_BOTTOM_CORNER;

            else if (j == gameBoard.GetLength(1) - 1)
                gameBoard[i, j] = CharacterUtilities.RIGHT_BOTOOM_CORNER;
        }

        private void HandleLeftRightEdges(char[,] gameBoard, int i, int j)
        {
            if ((j == 0) || j == gameBoard.GetLength(1) - 1)
                gameBoard[i, j] = CharacterUtilities.LEFT_AND_RIGHT_EDGES;
        }
    }
}