using System;
using System.Runtime.CompilerServices;

namespace ConsoleAppPongFinalProject
{
    struct Point
    {
        static class MethodImplOptionsEx { public const short AggressiveInlining = 256; }

        public int x;
        public int y;

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        public Point(int x, int y) { this.x = x; this.y = y; }

        public void SetCenter()
        {
            x = BoardManager.HalfFieldWidth;
            y = BoardManager.HalfFieldHight;
        }

        public void SetFirstPaddlePosition()
        {
            x = BoardManager.FirstPlayerXPosition;
            y = BoardManager.HalfFieldHight - 2;
        }

        public void SetSecondPaddlePosition()
        {
            x = BoardManager.SecondPlayerXPosition;
            y = BoardManager.HalfFieldHight - 2;
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator +(Point a, Point b) { return new Point(a.x + b.x, a.y + b.y); }
    }
}