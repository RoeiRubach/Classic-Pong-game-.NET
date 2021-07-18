namespace ConsoleAppPongFinalProject
{
    enum MainMenuOptions
    {
        None,
        SinglePlayer = 9,
        PVP = 15,
        Highscore = 21
    }

    enum GameMode
    {
        None,
        SinglePlayer,
        PVP
    }

    enum PaddleEdge
    {
        None,
        UpperEdge,
        MiddleEdge,
        BottomEdge
    }

    enum GameStatus
    {
        None,
        Restart,
        End
    }
}