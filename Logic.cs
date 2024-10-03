using System.Numerics;
using System.Security.Cryptography;

namespace Functions
{
    internal class Logic
    {
        public static bool IsInsideRect(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2) => 
            !(endPos1.X < startPos2.X || startPos1.X > endPos2.X || endPos1.Y < startPos2.Y || startPos1.Y > endPos2.Y);

        public struct shipStats
        {
            public Vector2 startPosition;
            public Vector2 endPosition;
            public Direction direction;
            public int size;
            public int hit;
        }

        public struct coordinateInput
        {
            public int x, y;
            public bool valid;
        }

        public enum Direction
        {
            horizontal = 0,
            vertical = 1
        }

        //public static int RandomInt(int min, int max) => new Random(Guid.NewGuid().GetHashCode()).Next(min, max);
        public static int RandomInt(int min, int max)
        {
            byte[] randomNumber = new byte[4];
            RandomNumberGenerator.Fill(randomNumber);
            int value = BitConverter.ToInt32(randomNumber, 0);
            return Math.Abs(value % (max - min)) + min;
        }
    }
}
