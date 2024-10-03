using System.Numerics;

public partial class Game
{
    void DrawGamePlane()
    {
        for (int x = 0; x < gameSize; x++)
        {
            Console.SetCursorPosition(gridStart + (x * (int)gridSpacing.X), 0);
            Console.Write((char)(alphabetCapsASCIIStartIndex + x));

            for (int i = 0; i < gameSize; i++)
            {
                Console.SetCursorPosition(gridStart + (x * (int)gridSpacing.X), gridStart + (i * (int)gridSpacing.Y));
                Console.Write(charFiller);
            }
        }

        for (int y = 0; y < gameSize; y++)
        {
            Console.SetCursorPosition(0, gridStart + (y * (int)gridSpacing.Y));
            Console.Write((char)(numberASCIIStartIndex + y));
        }
    }

    void DrawShips()
    {
        for (int i = 0; i < ships.Count; i++)
        {
            Console.ForegroundColor = (ConsoleColor)i + 1;

            Ship shipReference = ships[i];
            Vector2 shipStartPosition = shipReference.startPosition;
            Vector2 shipEndPosition = shipReference.endPosition;
            ShipDirection shipDirection = shipReference.direction;
            int shipSize = shipReference.size;

            for (int j = 0; j < shipSize; j++)
            {
                Console.SetCursorPosition(gridStart + ((shipDirection == ShipDirection.horizontal ? 1 : 0) * (j * (int)gridSpacing.X)) + (int)(shipStartPosition.X * gridSpacing.X),
                                          gridStart + ((shipDirection == ShipDirection.vertical ? 1 : 0) * (j * (int)gridSpacing.Y)) + (int)(shipStartPosition.Y * gridSpacing.Y));
                Console.Write(charShip);
            }
        }
    }
    void DrawHitPoints()
    {
        for (int i = 0; i < pointsShot.Count; i++)
        {
            Console.SetCursorPosition(gridStart + (int)(pointsShot[i].X * gridSpacing.X), gridStart + (int)(pointsShot[i].Y * gridSpacing.Y));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(charMissed);
        }

        for (int i = 0; i < pointsAttacked.Count; i++)
        {
            Console.SetCursorPosition(gridStart + (int)(pointsAttacked[i].X * gridSpacing.X), gridStart + (int)(pointsAttacked[i].Y * gridSpacing.Y));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(charShip);
        }
    }

    void DrawMessages()
    {
        Console.SetCursorPosition(0, (int)gridSpacing.Y * gameSize + gridStart);
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine($"Points left to hit: {(pointsToHit - pointsAttacked.Count)}");
        Console.WriteLine($"You have shot: {(pointsShot.Count())} times");
        Console.WriteLine($"Write 'attack XY' to attack a point and 'help' for more commands.");
        Console.WriteLine($"\n{roundMessage}");

        roundMessage = string.Empty;
    }

    void DrawGame()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        DrawGamePlane();
        if (shipsVisible)
            DrawShips();
        DrawHitPoints();
        DrawMessages();
    }

    void AddRoundMessage(string message) => roundMessage += $"{message}\n";
}