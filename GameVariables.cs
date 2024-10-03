using System.Numerics;

public partial class Game
{
    bool gameOngoing = false;
    int gamePauseTime = 2000; // miliseconds
    static int gameSize = 10;
    static int gridStart = 2;
    Vector2 gridSpacing = new Vector2(gridStart + 2, gridStart);

    const char charFiller = '~';
    const char charMissed = '*';
    const char charShip = '#';

    const int alphabetASCIIStartIndex = 97; // abcdefghj...
    const int alphabetCapsASCIIStartIndex = 65; // ABCDEFGHJ...
    const int numberASCIIStartIndex = 48; // 0123456789...

    string roundMessage = string.Empty;

    List<Ship> ships = new List<Ship>();
    List<Vector2> pointsAttacked = new List<Vector2>();
    List<Vector2> pointsShot = new List<Vector2>();
    Vector2 shipsMinimumDistance = new Vector2(1, 1);
    bool shipsVisible = false;
    int pointsToHit = 0;
}