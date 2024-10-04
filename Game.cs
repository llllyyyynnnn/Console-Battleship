using System.Numerics;
using System.Security.Cryptography;

public partial class Game
{
    void InitializeGame() // skapar skeppen, s�tter gameOngoing till 'true' vilket skickar en signal till funktionerna att spelet har startats 
    {
        AddShip(2);
        AddShip(3);
        AddShip(3);
        AddShip(4);
        AddShip(5);

        gameOngoing = true;
    }

    void ResetGame() // t�m alla listor och s�tt gameOngoing till 'false' vilket skickar en signal till funktionerna att det finns inget aktivt spel p� g�ng
    {
        ships.Clear();
        pointsAttacked.Clear();
        pointsShot.Clear();
        pointsToHit = 0;
        roundMessage = string.Empty;
        gameOngoing = false;

        InitializeGame();
    }

    void GameLoop() // k�rs varje g�ng spelaren skickar ett meddelande till programmet
    {
        DrawGame(); // ritar allt som ska ritas, exempelvis spelplanen, skepp ifall shipsVisible �r 'true', attackerade positioner och meddelanden

        try
        {
            RegisterCommand(Console.ReadLine()); // ist�llet f�r att manuellt l�gga en null-check f�r varje kommando
                                                 // kan vi s�tta den inom en try, och catcha exceptionen vilket g�r att programmet inte termineras och vi kan 
                                                 // meddela spelaren om vad erroren handlar om ifall ett felaktigt kommando skickas in
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Command error detected ({ex})");
            PauseGame(); // pausa spelet s� att spelaren har tid att se error-meddelandet
        }

        if (pointsToHit - pointsAttacked.Count == 0) // ifall m�ngden tr�ff som kr�vs �verstiger m�ngden tr�ff p� skeppen har spelaren vunnit, meddela det och v�nta tills en ny input skickas innan vi resettar spelet 
        {
            Console.WriteLine("All ships have been hit!");
            Console.WriteLine("Press any key to reset the game and start over again.");
            Console.ReadLine();

            RegisterCommand("reset");
        }

        if (gameOngoing)
            Console.Clear(); // ifall spelet �r ongoing ska vi t�mma console f�nstret s� att vi kan rita om allt med den nya informationen baserat p� inputsen som spelaren skickar in
    }

    public void Run()
    {
        InitializeGame(); // k�r initialize funktionen f�r att sedan o�ndligt k�ra loop funktionen
                          // eftersom att det �r b�ttre att ha en viktig public funktion ist�llet f�r massa som inte n�dv�ndigtvis m�ste vara det, kan vi k�ra den h�r funktionen utanf�r klassen

        while (true)
            GameLoop();
    }
}
