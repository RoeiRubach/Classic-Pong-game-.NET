using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    enum UserChoice
    {
        None,
        SinglePlayer,
        MultiPlayer
    }

    enum CollidedWithBall
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
