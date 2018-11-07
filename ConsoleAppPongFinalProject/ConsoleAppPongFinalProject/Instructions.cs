using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Instructions
    {
        public void ClearsTheBoard()
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

        public void Prints1PlayerInstructions(string player)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintsPlayer1Controls(player);
            PrintsScoreInstruction();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Prints2PlayersInstructions(string player1, string player2)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            PrintsPlayer1Controls(player1);
            PrintsPlayer2Controls(player2);
            PrintsScoreInstruction();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string PrintsPlayer1Controls(string player1)
        {
            Console.SetCursorPosition(3, 7);
            Console.WriteLine(player1 + ": ");
            Console.SetCursorPosition(6, 8);
            Console.WriteLine("Controls the paddle on the left hand side of the screen using your keyboard.");
            Console.SetCursorPosition(6, 9);
            Console.WriteLine("Use -UpArrow- to move the paddle up and -DownArrow- to move the paddle down.");
            return player1;
        }

        private void PrintsPlayer2Controls(string player2)
        {
            Console.SetCursorPosition(3, 11);
            Console.WriteLine(player2 + ": ");
            Console.SetCursorPosition(6, 12);
            Console.WriteLine("Controls the paddle on the Right hand side of the screen using your keyboard.");
            Console.SetCursorPosition(6, 13);
            Console.WriteLine("Use -W- to move the paddle up and -S- to move the paddle down.");
        }

        private void PrintsScoreInstruction()
        {
            Console.SetCursorPosition(16,15);
            Console.WriteLine("Points are scored when your opponent misses the ball.");
            Console.SetCursorPosition(17,16);
            Console.WriteLine("First player to reach 5 points wins the game.");
        }

        public string SetsPlayerName(int whichPlayer)
        {
            //Gets an integer to set a string value to player 1/2 with a switch statement and returns it.
            Console.ForegroundColor = ConsoleColor.Cyan;
            string playerName = null;
            Console.SetCursorPosition(3, 7);
            switch (whichPlayer)
            {
                case 1:
                    Console.Write("Enter -player1's- name: ");
                    break;
                case 2:
                    Console.Write("Enter -player2's- name: ");
                    break;
                default:
                    break;
            }
            playerName = Console.ReadLine();
            ClearsTheBoard();
            Console.ForegroundColor = ConsoleColor.White;
            return playerName;
        }
    }
}
