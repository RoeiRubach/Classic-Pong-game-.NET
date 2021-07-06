namespace ConsoleAppPongFinalProject
{
    public enum UserChoice
    {
        None,
        SinglePlayer,
        PlayerVSPlayer
    }

    public enum CollidedWithBall
    {
        None,
        UpperEdge,
        MiddleEdge,
        BottomEdge
    }

    public enum GameStatus
    {
        None,
        Restart,
        End
    }

    public enum UserOptions
    {
        SoloPlayer = 9,
        PvP = 15,
        Highscore = 21
    }
}
