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