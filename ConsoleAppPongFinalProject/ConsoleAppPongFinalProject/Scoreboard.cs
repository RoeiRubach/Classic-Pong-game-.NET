using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Scoreboard
    {
        public void PrintsZero(int location)
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

        public void PrintsOne(int location)
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

        public void PrintsTwo(int location)
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

        public void PrintsThree(int location)
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

        public void PrintsFour(int location)
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

        public void PrintsFive(int location)
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

        public void ClearTheScore(int location)
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
