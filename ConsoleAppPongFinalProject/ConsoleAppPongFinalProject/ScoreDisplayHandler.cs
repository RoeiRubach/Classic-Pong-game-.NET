using System;

namespace ConsoleAppPongFinalProject
{
    class ScoreDisplayHandler
    {
        private const int RIGHT_SIDE_SCORE_LOCATION = 83;

        public ScoreDisplayHandler()
        {
            ResetBothScores();
            Player.PlayerScored += PrintCurrentScore;
        }

        private void ResetBothScores()
        {
            PrintCurrentScore(0, 0);
            PrintCurrentScore(0, RIGHT_SIDE_SCORE_LOCATION);
        }

        public void PrintCurrentScore(int scoreToDisplay, int playerNum)
        {
            int location;
            if (playerNum == 1)
                location = 0;
            else
                location = RIGHT_SIDE_SCORE_LOCATION;

            ClearScore(location);
            switch (scoreToDisplay)
            {
                case 0:
                    PrintZero(location);
                    break;
                case 1:
                    PrintOne(location);
                    break;
                case 2:
                    PrintTwo(location);
                    break;
                case 3:
                    PrintThree(location);
                    break;
                case 4:
                    PrintFour(location);
                    break;
                case 5:
                    PrintFive(location);
                    break;
            }
        }

        private void PrintZero(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("   __  ");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine("  /  \\ ");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine(" | () |");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine("  \\__/ ");
        }

        private void PrintOne(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("  _ ");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine(" / |");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine(" | |");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine(" |_|");
        }

        private void PrintTwo(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("  ___ ");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine(" |_  )");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine("  / / ");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine(" /___|");
        }

        private void PrintThree(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("  ____");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine(" |__ /");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine("  |_ \\");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine(" |___/");
        }

        private void PrintFour(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("  _ _  ");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine(" | | | ");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine(" |_  _|");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine("   |_| ");
        }

        private void PrintFive(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("  ___ ");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine(" | __|");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine(" |__ \\");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine(" |___/");
        }

        private void ClearScore(int location)
        {
            Console.SetCursorPosition(location, 0);
            Console.WriteLine("\t\t");
            Console.SetCursorPosition(location, 1);
            Console.WriteLine("\t\t");
            Console.SetCursorPosition(location, 2);
            Console.WriteLine("\t\t");
            Console.SetCursorPosition(location, 3);
            Console.WriteLine("\t\t");
        }
    }
}