using System;
using System.Runtime.CompilerServices;

namespace ConsoleAppPongFinalProject
{
    struct Point
    {
        static class MethodImplOptionsEx
        {
            public const short AggressiveInlining = 256;
        }

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
            this.x = Board.HalfFieldWidth;
            this.y = Board.HalfFieldHight;
        }

        public void SetFirstPlayerPosition()
        {
            this.x = Board.FirstPlayerXPosition;
            this.y = Board.HalfFieldHight - 2;
        }
        
        public void SetSecondPlayerPosition()
        {
            this.x = Board.SecondPlayerXPosition;
            this.y = Board.HalfFieldHight - 2;
        }

        // used to allow Points to be used as keys in hash tables
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        // also required for being able to use Point as keys in hash tables
        public override bool Equals(object other)
        {
            if (!(other is Point)) return false;

            return Equals((Point)other);
        }

        // Adds two vectors.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator +(Point a, Point b) { return new Point(a.x + b.x, a.y + b.y); }

        // Subtracts one vector from another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator -(Point a, Point b) { return new Point(a.x - b.x, a.y - b.y); }

        // Multiplies one vector by another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator *(Point a, Point b) { return new Point(a.x * b.x, a.y * b.y); }

        // Divides one vector over another.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator /(Point a, Point b) { return new Point(a.x / b.x, a.y / b.y); }

        // Negates a vector.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator -(Point a) { return new Point(-a.x, -a.y); }

        // Multiplies a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator *(Point a, int d) { return new Point(a.x * d, a.y * d); }

        // Multiplies a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator *(int d, Point a) { return new Point(a.x * d, a.y * d); }

        // Divides a vector by a number.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static Point operator /(Point a, int d) { return new Point(a.x / d, a.y / d); }

        // Returns true if the vectors are equal.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator ==(Point lhs, Point rhs)
        {
            // Returns false in the presence of NaN values.
            float diff_x = lhs.x - rhs.x;
            float diff_y = lhs.y - rhs.y;
            return (diff_x * diff_x + diff_y * diff_y) < kEpsilon * kEpsilon;
        }

        // Returns true if vectors are different.
        [MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
        public static bool operator !=(Point lhs, Point rhs)
        {
            // Returns true in the presence of NaN values.
            return !(lhs == rhs);
        }

        // *Undocumented*
        public const float kEpsilon = 0.00001F;
        // *Undocumented*
        public const float kEpsilonNormalSqrt = 1e-15f;
    }
}