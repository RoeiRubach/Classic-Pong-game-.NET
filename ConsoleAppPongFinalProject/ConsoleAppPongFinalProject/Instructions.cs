using System;

namespace ConsoleAppPongFinalProject
{
    public class Instructions
    {
        private void ClearBoard()
        {
            for (int i = 0; i < 33; i++)
            {
                for (int j = 4; j < 29; j++)
                {
                    Console.SetCursorPosition(i,j);
                    Console.Write("\t\t\t\t\t\t   ");
                }
            }
        }

        public void PrintSoloPlayerInstructions(string player)
        {
            ClearBoard();
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintPlayerOneControls(player);
            PrintScoreInstruction();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintPVPInstructions(string playerOne, string playerTwo)
        {
            ClearBoard();
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintPlayerOneControls(playerOne);
            PrintPlayerTwoControls(playerTwo);
            PrintScoreInstruction();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintPlayerOneControls(string playerOne)
        {
            Console.SetCursorPosition(3, 7);
            Console.WriteLine(playerOne + ": ");
            Console.SetCursorPosition(6, 8);
            Console.WriteLine("Controls the paddle on the left hand side of the screen using your keyboard.");
            Console.SetCursorPosition(6, 9);
            Console.WriteLine("Use -UpArrow- to move the paddle up and -DownArrow- to move the paddle down.");
        }

        private void PrintPlayerTwoControls(string playerTwo)
        {
            Console.SetCursorPosition(3, 11);
            Console.WriteLine(playerTwo + ": ");
            Console.SetCursorPosition(6, 12);
            Console.WriteLine("Controls the paddle on the Right hand side of the screen using your keyboard.");
            Console.SetCursorPosition(6, 13);
            Console.WriteLine("Use -W- to move the paddle up and -S- to move the paddle down.");
        }

        private void PrintScoreInstruction()
        {
            Console.SetCursorPosition(16,15);
            Console.WriteLine("Points are scored when your opponent misses the ball.");
            Console.SetCursorPosition(17,16);
            Console.WriteLine($"First player to reach {GameManager.GOALS_TO_REACH} points wins the game.");
        }
    }
}