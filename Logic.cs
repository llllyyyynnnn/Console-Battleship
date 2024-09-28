using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace project01
{
    internal class Logic
    {
        public static bool IsInsideRect(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2)
        {
            return !(endPos1.X < startPos2.X || startPos1.X > endPos2.X ||
                     endPos1.Y < startPos2.Y || startPos1.Y > endPos2.Y);
        }
        
        public struct shipStats
        {
            public Vector2 startPosition;
            public Vector2 endPosition;
            public direction direction;
            public int size;
        }

        public struct coordinateInput
        {
            public int x, y;
            public bool valid;
        }

        public enum direction
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
