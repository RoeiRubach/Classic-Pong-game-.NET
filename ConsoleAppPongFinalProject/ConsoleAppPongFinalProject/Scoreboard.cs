using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Scoreboard
    {
        private void PrintsZero(int location)
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

        private void PrintsOne(int location)
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

        private void PrintsTwo(int location)
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

        private void PrintsThree(int location)
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

        private void PrintsFour(int location)
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

        private void PrintsFive(int location)
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

        private void ClearTheScore(int location)
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

        public void GetsTheScore(int currentScore, int location)
        {
            ClearTheScore(location);
            switch (currentScore)
            {
                case 0:
                    PrintsZero(location);
                    break;
                case 1:
                    PrintsOne(location);
                    break;
                case 2:
                    PrintsTwo(location);
                    break;
                case 3:
                    PrintsThree(location);
                    break;
                case 4:
                    PrintsFour(location);
                    break;
                case 5:
                    PrintsFive(location);
                    break;
            }
        }
    }
}
