namespace squares_api.Domain.Entities
{
    public class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point? lhs, Point? rhs)
        {
            if(lhs is null && rhs is null) return true;
            if (lhs is null || rhs is null) return false;
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }
        public static bool operator !=(Point? lhs, Point? rhs)
        {
            if (lhs is null && rhs is null) return false;
            if (lhs is null || rhs is null) return true;
            return lhs.X != rhs.X || lhs.Y != rhs.Y;
        }

        public static Point operator -(Point lhs, Point rhs)
        {
            return new Point(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }
        public static Point operator +(Point lhs, Point rhs)
        {
            return new Point(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
        public static Point Rotate(Point A, Point B)
        {
            Point BA = B - A;
            return new Point(B.X + BA.Y, B.Y - BA.X);
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Point p) return false;
            return X == p.X && Y == p.Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
