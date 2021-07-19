namespace ConsoleAppPongFinalProject
{
    class GameBorder
    {
        public GameBorder(char[,] gameField) => SetBorder(gameField);

        private void SetBorder(char[,] gameField)
        {
            for (int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    if (i == 0)
                        HandleUpperBorder(i, j, gameField);

                    else if (i == gameField.GetLength(0) - 1)
                        HandleLowerBorder(i, j, gameField);

                    else
                    {
                        gameField[i, j] = CharacterUtilities.EMPTY_PIXEL;
                        HandleLeftRightBorders(i, j, gameField);
                    }
                }
            }
        }

        private void HandleLeftRightBorders(int i, int j, char[,] gameField)
        {
            if ((j == 0) || j == gameField.GetLength(1) - 1)
                gameField[i, j] = CharacterUtilities.LEFT_RIGHT_BORDER_ICON;
        }

        private void HandleLowerBorder(int i, int j, char[,] gameField)
        {
            gameField[i, j] = CharacterUtilities.TOP_BOTTOM_BORDER_ICON;

            if (j == 0)
                gameField[i, j] = CharacterUtilities.LEFT_BOTTOM_BORDER_ICON;

            else if (j == gameField.GetLength(1) - 1)
                gameField[i, j] = CharacterUtilities.RIGHT_BOTTOM_BORDER_ICON;
        }

        private void HandleUpperBorder(int i, int j, char[,] gameField)
        {
            gameField[i, j] = CharacterUtilities.TOP_BOTTOM_BORDER_ICON;

            if (j == 0)
                gameField[i, j] = CharacterUtilities.LEFT_UPPER_BORDER_ICON;

            else if (j == gameField.GetLength(1) - 1)
                gameField[i, j] = CharacterUtilities.RIGHT_UPPER_BORDER_ICON;
        }
    }
}