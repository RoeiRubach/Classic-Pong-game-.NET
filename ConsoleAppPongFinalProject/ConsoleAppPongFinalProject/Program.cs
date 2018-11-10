using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GameManager game;
            do
            {
                game = new GameManager();
                game.Start();
            } while (!game.IsGameRestart());
        }
    }
}
