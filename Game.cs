using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using project01;

bool gameOngoing = false;
int gameSize = 10;
int gamePauseTime = 1; // miliseconds
int gridStart = 2;
Vector2 gridSpacing = new Vector2(gridStart + 2, gridStart);

const char charFiller = '~';
const char charShip = '#';
const int alphabetASCIIStartIndex = 97; // abcdefghj...
const int alphabetCapsASCIIStartIndex = 65; // ABCDEFGHJ...
const int numberASCIIStartIndex = 48; // 0123456789...

List<Logic.shipStats> ships = new List<Logic.shipStats>();
List<Vector2> pointsAttacked = new List<Vector2>();
Vector2 shipsMinimumDistance = new Vector2(1, 1);
int pointsToHit = 0;

void InitializeGame()
{
    AddShip(2);
    AddShip(3);
    AddShip(3);
    AddShip(4);
    AddShip(5);
    gameOngoing = true;
}

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

void AddShip(int length)
{
    Vector2 randomPosition = new Vector2(Logic.RandomInt(0, gameSize), Logic.RandomInt(0, gameSize));
    Logic.direction randomDirection = (Logic.direction)(Logic.RandomInt(0, 100) % 2);
    bool canContinue = (randomDirection == Logic.direction.horizontal && randomPosition.X + length <= gameSize) ||
                        randomDirection == Logic.direction.vertical && randomPosition.Y + length <= gameSize;

    if (canContinue)
    {
        Vector2 startPosition = new Vector2(randomPosition.X, randomPosition.Y);
        Vector2 endPosition = new Vector2(randomPosition.X + ((randomDirection == Logic.direction.horizontal ? 1 : 0) * length),
                                          randomPosition.Y + ((randomDirection == Logic.direction.vertical ? 1 : 0) * length));

        if (ships.Count > 0)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                if (!canContinue)
                    continue;

                Logic.shipStats shipReference = ships[i];
                Logic.direction shipDirection = shipReference.direction;
                int shipSize = shipReference.size;

                Vector2 shipStartPosition = shipReference.startPosition;
                Vector2 shipEndPosition = shipReference.endPosition;

                canContinue = !Logic.IsInsideRect(startPosition, endPosition, shipStartPosition - shipsMinimumDistance, shipEndPosition + shipsMinimumDistance);
            }
        }

        if (canContinue)
        {
            Logic.shipStats shipStats = new Logic.shipStats();
            shipStats.startPosition = startPosition;
            shipStats.endPosition = endPosition;
            shipStats.direction = randomDirection;
            shipStats.size = length;

            ships.Add(shipStats);
            pointsToHit += length;
            return;
        }
    }
    
    AddShip(length);
}

void DrawShips()
{
    for (int i = 0; i < ships.Count; i++)
    {
        Console.ForegroundColor = (ConsoleColor)i + 1;


        Logic.shipStats shipReference = ships[i];
        Vector2 shipStartPosition = shipReference.startPosition;
        Vector2 shipEndPosition = shipReference.endPosition;
        Logic.direction shipDirection = shipReference.direction;
        int shipSize = shipReference.size;


        for (int j = 0; j < shipSize; j++)
        {
            Console.SetCursorPosition(gridStart + ((shipDirection == Logic.direction.horizontal ? 1 : 0) * (j * (int)gridSpacing.X)) + (int)(shipStartPosition.X * gridSpacing.X),
                                      gridStart + ((shipDirection == Logic.direction.vertical ? 1 : 0) * (j * (int)gridSpacing.Y)) + (int)(shipStartPosition.Y * gridSpacing.Y));

            Console.Write(charShip);
        }
    }
}

void ResetGame()
{
    ships.Clear();
    pointsAttacked.Clear();
    pointsToHit = 0;
    gameOngoing = false;
    
    InitializeGame();
}

void PauseGame() => System.Threading.Thread.Sleep(gamePauseTime);
void DrawHitPoints()
{
    for(int i = 0; i < pointsAttacked.Count; i++)
    {
        Console.SetCursorPosition(gridStart + (int)(pointsAttacked[i].X * gridSpacing.X), gridStart + (int)(pointsAttacked[i].Y * gridSpacing.Y));
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(charShip);
    }
}

void DrawGame()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    DrawGamePlane();
    DrawShips();
    DrawHitPoints();
}

Logic.coordinateInput ValidateInput(string coordinate)
{
    Logic.coordinateInput input;
    input.valid = false;
    input.x = 0;
    input.y = 0;

    bool validCoordinate = (coordinate != null && coordinate.Length == 2);
    bool inRange = false;

    if (validCoordinate)
    {
        int letterCoordinate = (int)coordinate.ToLower()[0];
        int numberCoordinate = coordinate[1];

        inRange = (letterCoordinate >= alphabetASCIIStartIndex && letterCoordinate <= alphabetASCIIStartIndex + gameSize) && numberCoordinate >= numberASCIIStartIndex && numberCoordinate <= numberASCIIStartIndex + gameSize;
        if (inRange)
        {
            input.x = letterCoordinate - alphabetASCIIStartIndex;
            input.y = numberCoordinate - numberASCIIStartIndex;
            input.valid = true;
        }
    }

    if (input.valid == false)
        Console.WriteLine("Input was invalid or out of range.");

    return input;
}

void AttackPoint(Vector2 position)
{
    if (pointsAttacked.Contains(position))
    {
        Console.WriteLine("Point already hit!");
        return;
    }

    bool shouldContinue = true;

    if (ships.Count > 0)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            if (!shouldContinue)
                continue;

            Logic.shipStats shipReference = ships[i];
            Logic.direction shipDirection = shipReference.direction;
            int shipSize = shipReference.size;

            Vector2 shipStartPosition = shipReference.startPosition;
            Vector2 shipEndPosition = shipReference.endPosition - new Vector2((shipDirection == Logic.direction.horizontal ? 1 : 0) * 1, (shipDirection == Logic.direction.vertical ? 1 : 0) * 1); // -1 due to IsInsideRect being too precise and allowing the slightest of overlaps to be registered as a hit

            shouldContinue = !Logic.IsInsideRect(position, position, shipStartPosition, shipEndPosition);
        }
    }

    if (!shouldContinue)
        pointsAttacked.Add(position);
}

void GameLoop()
{
    DrawGame();

    Console.SetCursorPosition(0, (int)gridSpacing.Y * gameSize + gridStart);

    string coordinate = Console.ReadLine();
    Logic.coordinateInput userValidatedInput = ValidateInput(coordinate);

    if (userValidatedInput.valid)
        AttackPoint(new Vector2(userValidatedInput.x, userValidatedInput.y));

    Console.WriteLine("Points left to hit: " + (pointsToHit - pointsAttacked.Count));

    if (pointsToHit - pointsAttacked.Count == 0)
    {
        gameOngoing = false;
        Console.WriteLine("All ships have been hit!");
        Console.WriteLine("Press any key to reset the game and start over again.");
        Console.ReadLine();
        ResetGame();
    }

    if (gameOngoing)
    {
        PauseGame();
        Console.Clear();
    }
}

InitializeGame();
while (true)
    GameLoop();