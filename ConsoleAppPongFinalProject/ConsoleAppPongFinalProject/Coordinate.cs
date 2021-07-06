namespace ConsoleAppPongFinalProject
{
    public struct Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void SetFirstPuddlePosition()
        {
            X = 2;
            Y = BoardManager.GetHalfHight() - 2;
        }

        public void SetSecondPuddlePosition()
        {
            X = 20;
            Y = BoardManager.GetHalfHight() - 8;
        }

        public void SetCenterBoardPosition()
        {
            X = BoardManager.GetHalfWidth();
            Y = BoardManager.GetHalfHight();
        }
    }
}