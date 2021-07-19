using System;
using System.Runtime.CompilerServices;

namespace ConsoleAppPongFinalProject
{
    struct Point
    {
        static class MethodImplOptionsEx { public const short AggressiveInlining = 256; }

        public int X;
        public int Y;

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        public Point(int x, int y) { this.X = x; this.Y = y; }

        public void SetCenter()
        {
            X = Board.HalfFieldWidth;
            Y = Board.HalfFieldHight;
        }

        public void SetFirstPaddlePosition()
        {
            X = Board.FirstPlayerXPosition;
            Y = Board.HalfFieldHight - 2;
        }

        public void SetSecondPaddlePosition()
        {
            X = Board.SecondPlayerXPosition;
            Y = Board.HalfFieldHight - 2;
        }

        public override int GetHashCode() { return X.GetHashCode() ^ (Y.GetHashCode() << 2); }

        public override bool Equals(object other)
        {
            if (!(other is Point)) return false;

            return Equals((Point)other);
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator +(Point a, Point b) { return new Point(a.X + b.X, a.Y + b.Y); }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator ==(Point lhs, Point rhs)
        {
            float diff_x = lhs.X - rhs.X;
            float diff_y = lhs.Y - rhs.Y;
            return (diff_x * diff_x + diff_y * diff_y) < kEpsilon * kEpsilon;
        }

        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator !=(Point lhs, Point rhs) { return !(lhs == rhs); }

        public const float kEpsilon = 0.00001F;
        public const float kEpsilonNormalSqrt = 1e-15f;
    }
}