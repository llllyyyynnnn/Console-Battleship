using System.Numerics;
using Functions;

bool gameOngoing = false;
int gameSize = 10;
int gamePauseTime = 2000; // miliseconds
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
bool shipsVisible = false;
int pointsToHit = 0;
int shotAmount = 0;

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
    Logic.Direction randomDirection = (Logic.Direction)(Logic.RandomInt(0, 100) % 2);
    bool canContinue = (randomDirection == Logic.Direction.horizontal && randomPosition.X + length <= gameSize) ||
                        randomDirection == Logic.Direction.vertical && randomPosition.Y + length <= gameSize;

    if (canContinue)
    {
        Vector2 startPosition = new Vector2(randomPosition.X, randomPosition.Y);
        Vector2 endPosition = new Vector2(randomPosition.X + ((randomDirection == Logic.Direction.horizontal ? 1 : 0) * length),
                                          randomPosition.Y + ((randomDirection == Logic.Direction.vertical ? 1 : 0) * length));

        if (ships.Count > 0)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                if (!canContinue)
                    continue;

                Logic.shipStats shipReference = ships[i];
                Logic.Direction shipDirection = shipReference.direction;
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
        Logic.Direction shipDirection = shipReference.direction;
        int shipSize = shipReference.size;

        for (int j = 0; j < shipSize; j++)
        {
            Console.SetCursorPosition(gridStart + ((shipDirection == Logic.Direction.horizontal ? 1 : 0) * (j * (int)gridSpacing.X)) + (int)(shipStartPosition.X * gridSpacing.X),
                                      gridStart + ((shipDirection == Logic.Direction.vertical ? 1 : 0) * (j * (int)gridSpacing.Y)) + (int)(shipStartPosition.Y * gridSpacing.Y));
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

void DrawMessages()
{
    Console.SetCursorPosition(0, (int)gridSpacing.Y * gameSize + gridStart);
    Console.ForegroundColor = ConsoleColor.White;

    Console.WriteLine($"Points left to hit: {(pointsToHit - pointsAttacked.Count)}");
    Console.WriteLine($"You have shot: {(shotAmount)} times");
    Console.WriteLine($"Write 'attack XY' to attack a point and 'help' for more commands.");
}
void DrawGame()
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    DrawGamePlane();
    if(shipsVisible)
        DrawShips();
    DrawHitPoints();
    DrawMessages();
}

Logic.coordinateInput ValidateCoordinateInput(string coordinate)
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

    shotAmount += 1;

    if (ships.Count > 0)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            Logic.shipStats shipReference = ships[i];
            Logic.Direction shipDirection = shipReference.direction;
            int shipSize = shipReference.size;

            Vector2 shipStartPosition = shipReference.startPosition;
            Vector2 shipEndPosition = shipReference.endPosition - new Vector2((shipDirection == Logic.Direction.horizontal ? 1 : 0) * 1, (shipDirection == Logic.Direction.vertical ? 1 : 0) * 1); // -1 due to IsInsideRect being too precise and allowing the slightest of overlaps to be registered as a hit

            if (Logic.IsInsideRect(position, position, shipStartPosition, shipEndPosition))
            {
                pointsAttacked.Add(position);

                shipReference.hit += 1;
                ships[i] = shipReference;

                Console.WriteLine("Ship was found in this coordinate.");
                if (shipReference.hit >= shipSize)
                    Console.WriteLine("Ship has been fully destroyed!");

                return;
            }
        }
    }

    Console.WriteLine("No ship was found in this coordinate.");
}

void RegisterCommand(string input)
{
    string[] splitInput = input.ToLower().Split(' ');

    switch (splitInput[0])
    {
        case "show":
            if (splitInput[1] == "ships")
                shipsVisible = true;
            break;
        case "hide":
            if (splitInput[1] == "ships")
                shipsVisible = false;
            break;
        case "attack":
            Logic.coordinateInput userValidatedInput = ValidateCoordinateInput(splitInput[1]);

            if (userValidatedInput.valid)
                AttackPoint(new Vector2(userValidatedInput.x, userValidatedInput.y));
            break;

        case "reset":
            ResetGame();
            break;

        case "set":
            if (splitInput[1] == "pause")
                gamePauseTime = int.Parse(splitInput[2]);
            break;
    }
}

void GameLoop()
{
    DrawGame();
    try
    {
        RegisterCommand(Console.ReadLine());
    }
    catch (Exception ex) {
        Console.WriteLine($"Command error detected ({ex})");
        PauseGame();
    }

    if (pointsToHit - pointsAttacked.Count == 0)
    {
        Console.WriteLine("All ships have been hit!");
        Console.WriteLine("Press any key to reset the game and start over again.");
        Console.ReadLine();

        RegisterCommand("reset");
    }

    if (gameOngoing)
        Console.Clear();
}

InitializeGame();
while (true)
    GameLoop();