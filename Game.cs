using System.Numerics;
using System.Security.Cryptography;

public partial class Game
{
    void InitializeGame() // skapar skeppen, sätter gameOngoing till 'true' vilket skickar en signal till funktionerna att spelet har startats 
    {
        AddShip(2);
        AddShip(3);
        AddShip(3);
        AddShip(4);
        AddShip(5);

        gameOngoing = true;
    }

    void ResetGame() // töm alla listor och sätt gameOngoing till 'false' vilket skickar en signal till funktionerna att det finns inget aktivt spel på gång
    {
        ships.Clear();
        pointsAttacked.Clear();
        pointsShot.Clear();
        pointsToHit = 0;
        roundMessage = string.Empty;
        gameOngoing = false;

        InitializeGame();
    }

    void GameLoop() // körs varje gång spelaren skickar ett meddelande till programmet
    {
        DrawGame(); // ritar allt som ska ritas, exempelvis spelplanen, skepp ifall shipsVisible är 'true', attackerade positioner och meddelanden

        try
        {
            RegisterCommand(Console.ReadLine()); // istället för att manuellt lägga en null-check för varje kommando
                                                 // kan vi sätta den inom en try, och catcha exceptionen vilket gör att programmet inte termineras och vi kan 
                                                 // meddela spelaren om vad erroren handlar om ifall ett felaktigt kommando skickas in
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Command error detected ({ex})");
            PauseGame(); // pausa spelet så att spelaren har tid att se error-meddelandet
        }

        if (pointsToHit - pointsAttacked.Count == 0) // ifall mängden träff som krävs överstiger mängden träff på skeppen har spelaren vunnit, meddela det och vänta tills en ny input skickas innan vi resettar spelet 
        {
            Console.WriteLine("All ships have been hit!");
            Console.WriteLine("Press any key to reset the game and start over again.");
            Console.ReadLine();

            RegisterCommand("reset");
        }

        if (gameOngoing)
            Console.Clear(); // ifall spelet är ongoing ska vi tömma console fönstret så att vi kan rita om allt med den nya informationen baserat på inputsen som spelaren skickar in
    }

    public void Run()
    {
        InitializeGame(); // kör initialize funktionen för att sedan oändligt köra loop funktionen
                          // eftersom att det är bättre att ha en viktig public funktion istället för massa som inte nödvändigtvis måste vara det, kan vi köra den här funktionen utanför klassen

        while (true)
            GameLoop();
    }
}
