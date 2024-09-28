/*
 using project01;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;

int length = 10;
int startGrid = 2;
int spacing = 2;

const char fillerCharacter = '~';
const char shipCharacter = '#';
bool gameOngoing = false;

Vector2 cursorPosition = Vector2.Zero;
List<Logic.shipStats> ships = new List<Logic.shipStats>();
List<Vector2> pointsAttacked = new List<Vector2>();

int RandomInt(int min, int max) => new Random(Guid.NewGuid().GetHashCode()).Next(min, max);
void Start()
{
    SetupCoordinates();
    SetupFillerCharacters();
    InitializeShips();
    gameOngoing = true;
}

void CenterCursorPosition()
{
    cursorPosition = new Vector2(startGrid, startGrid);
}

void SetupCoordinates()
{
    Console.ForegroundColor = ConsoleColor.Cyan;

    for (int verticalNumbers = 0; verticalNumbers < length; verticalNumbers++)
    {
        Console.SetCursorPosition(0, startGrid + (verticalNumbers * spacing));
        Console.Write((char)(48 + verticalNumbers));
    }

    for (int horziontalLetters = 0; horziontalLetters < length; horziontalLetters++)
    {
        Console.SetCursorPosition(startGrid + (horziontalLetters * spacing), 0);
        Console.Write((char)(65 + horziontalLetters));
    }
}

void SetupFillerCharacters()
{
    CenterCursorPosition();

    Console.ForegroundColor = ConsoleColor.Gray;

    for (int y = 0; y < length; y++)
    {
        cursorPosition = new Vector2(startGrid, startGrid + (spacing * y));

        for (int x = 0; x < length; x++)
        {
            cursorPosition = new Vector2(startGrid + (spacing * x), cursorPosition.Y);
            Console.SetCursorPosition((int)cursorPosition.X, (int)cursorPosition.Y);
            Console.Write(fillerCharacter);
        }
    }
}

void AddShip(int size)
{
    Vector2 randomPosition = new Vector2(RandomInt(0, length), RandomInt(0, length));
    Logic.direction randomDirection = (Logic.direction)(RandomInt(0, 100) % 2);
    bool canContinue = (randomDirection == Logic.direction.horizontal && randomPosition.X + size < length) ||
                       (randomDirection == Logic.direction.vertical && randomPosition.Y + size < length);

    if (canContinue && ships.Count > 0)
    {

        Vector2 startPosition = new Vector2(randomPosition.X, randomPosition.Y);
        Vector2 endPosition = startPosition;

        if (randomDirection == Logic.direction.horizontal)
            endPosition += new Vector2(size, 0);
        else
            endPosition += new Vector2(0, size);

        for (int i = 0; i < ships.Count; i++)
        {
            if (!canContinue)
                continue;

            Logic.shipStats shipReference = ships[i];
            Logic.direction shipDirection = shipReference.direction;
            int shipSize = shipReference.size;

            Vector2 shipStartPosition = shipReference.position;
            Vector2 shipEndPosition = shipReference.position;

            if (shipDirection == Logic.direction.horizontal)
                shipEndPosition += new Vector2(shipSize, 0);
            else
                shipEndPosition += new Vector2(0, shipSize);

            canContinue = !Logic.IsInsideRect(startPosition, endPosition, shipStartPosition, shipEndPosition);
        }
    }

    if (canContinue)
    {
        Logic.shipStats shipStats = new Logic.shipStats();
        shipStats.position = new Vector2(randomPosition.X, randomPosition.Y);
        shipStats.direction = randomDirection;
        shipStats.size = size;

        ships.Add(shipStats);
    }
    else
        AddShip(size);

}

void InitializeShips()
{
    AddShip(2);
    AddShip(3);
    AddShip(3);
    AddShip(4);
    AddShip(5);
}

void DrawShips()
{
    Console.ForegroundColor = ConsoleColor.Cyan;

    for (int i = 0; i < ships.Count(); i++)
    {
        Logic.shipStats shipReference = ships[i];
        Vector2 shipPosition = shipReference.position;
        int shipSize = shipReference.size;
        Logic.direction direction = shipReference.direction;

        for (int dir = 0; dir < shipSize; dir++)
        {
            if (direction == Logic.direction.horizontal)
                cursorPosition = new Vector2(startGrid + (shipPosition.X * spacing) + (spacing * dir), startGrid + (spacing * shipPosition.Y));
            else
                cursorPosition = new Vector2(startGrid + (shipPosition.X * spacing), startGrid + (shipPosition.Y * spacing) + (spacing * dir));

            Console.SetCursorPosition((int)cursorPosition.X, (int)cursorPosition.Y);
            Console.Write(shipCharacter);
        }
    }
}

void Loop()
{
    if (!gameOngoing)
    {
        Console.Clear();
        ships.Clear();
        Start();
        DrawShips();
    }

    Console.ForegroundColor = ConsoleColor.White;
    Console.SetCursorPosition(0, startGrid + length * spacing);
    Console.WriteLine("Specify a coordinate (number + letter)");

    string coordinate = Console.ReadLine();
    int firstChar = (int)coordinate.ToLower()[0] - 97;
    int number = coordinate[1] - 48;

    Vector2 attackPosition = new Vector2(firstChar, number);

    for (int i = 0; i < ships.Count(); i++)
    {
        Logic.direction shipDirection = ships[i].direction;
        Vector2 shipStartPosition = ships[i].position;
        Vector2 shipEndPosition = shipStartPosition;
        int shipSize = ships[i].size;

        if (shipDirection == Logic.direction.horizontal)
            shipEndPosition += new Vector2(shipSize, 0);
        else
            shipEndPosition += new Vector2(0, shipSize);

        if (Logic.IsInsideRect(attackPosition, attackPosition, shipStartPosition, shipEndPosition))
            pointsAttacked.Add(attackPosition);
    }

    for (int y = 0; y < pointsAttacked.Count; y++)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        Vector2 drawPosition = new Vector2(startGrid + (pointsAttacked[y].X * spacing), startGrid + (pointsAttacked[y].Y * spacing));
        Console.SetCursorPosition((int)drawPosition.X, (int)drawPosition.Y);
        Console.Write(shipCharacter);
    }
}

while (true)
{
    Loop();
}
 */

/*
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

int gridStartPosition = 2;
Vector2 gridSpacing = new Vector2(2, 2);

const char charFiller = '~';
const char charShip = '#';

Vector2 cursorPosition = Vector2.Zero;
List<Logic.shipStats> ships = new List<Logic.shipStats>();
List<Vector2> pointsAttacked = new List<Vector2>();

void Start()
{

}

void CenterCursorPosition() => cursorPosition = new Vector2(gridStartPosition, gridStartPosition);

void SetupCoordinates()
{
    Console.ForegroundColor = ConsoleColor.Cyan;

    for (int verticalNumbers = 0; verticalNumbers < gameSize; verticalNumbers++)
    {
        Console.SetCursorPosition(0, gridStartPosition + (verticalNumbers * (int)gridSpacing.Y));
        Console.Write((char)(48 + verticalNumbers));
    }

    for (int horziontalLetters = 0; horziontalLetters < gameSize; horziontalLetters++)
    {
        Console.SetCursorPosition(gridStartPosition + (horziontalLetters * (int)gridSpacing.X), 0);
        Console.Write((char)(65 + horziontalLetters));
    }
}

void SetupFillerCharacters()
{
    CenterCursorPosition();

    Console.ForegroundColor = ConsoleColor.Gray;

    for (int y = 0; y < gameSize; y++)
    {
        cursorPosition = new Vector2(gridStartPosition, gridStartPosition + (gridSpacing.Y * y));

        for (int x = 0; x < gameSize; x++)
        {
            cursorPosition = new Vector2(gridStartPosition + (gridSpacing.X * x), cursorPosition.Y);
            Console.SetCursorPosition((int)cursorPosition.X, (int)cursorPosition.Y);
            Console.Write(charFiller);
        }
    }
}

void AddShip(int size)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
}
void InitializeShips()
{
    AddShip(2);
    AddShip(3);
    AddShip(3);
    AddShip(4);
    AddShip(5);
}
 */