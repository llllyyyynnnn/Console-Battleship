using System.Numerics;
using System.Security.Cryptography;

public partial class Game
{
    void InitializeGame()
    {
        AddShip(2);
        AddShip(3);
        AddShip(3);
        AddShip(4);
        AddShip(5);
        gameOngoing = true;
    }

    void ResetGame()
    {
        ships.Clear();
        pointsAttacked.Clear();
        pointsShot.Clear();
        pointsToHit = 0;
        gameOngoing = false;

        InitializeGame();
    }

    void GameLoop()
    {
        DrawGame();

        try
        {
            RegisterCommand(Console.ReadLine());
        }
        catch (Exception ex)
        {
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

    public void Run()
    {
        InitializeGame();

        while (true)
            GameLoop();
    }
}
