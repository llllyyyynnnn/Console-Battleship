using System.Numerics;

public partial class Game
{
    void DrawGamePlane() // ritar spelytan
    {
        for (int x = 0; x < gameSize; x++)
        {
            Console.SetCursorPosition(gridStart + (x * (int)gridSpacing.X), 0); // sätter vår aktiva cursor position i console-fönstret (vart nästa karaktär ska skrivas)
            Console.Write((char)(alphabetCapsASCIIStartIndex + x)); // casta decimaltalet från startpunkten av stora alfabetsbokstäver i asciitabellen till en char, vilket ger en bokstav som börjar från A och går mot oändligheten (är spelytan för stor kommer vi få konstiga karaktärer) 

            for (int i = 0; i < gameSize; i++)
            {
                Console.SetCursorPosition(gridStart + (x * (int)gridSpacing.X), gridStart + (i * (int)gridSpacing.Y)); // för varje bokstav ska vi fylla kolumnen med charFiller's, tomma fält i spelytan
                Console.Write(charFiller);
            }
        }

        for (int y = 0; y < gameSize; y++)
        {
            Console.SetCursorPosition(0, gridStart + (y * (int)gridSpacing.Y)); // gridSpacing måsta castas till en int från float då Vector2 alltid är en float (kan fixas genom att göra en egen Vec2 struct, men jag använder det inbyggda)
            Console.Write((char)(numberASCIIStartIndex + y)); // som ovanför, men med nummer och Y axeln
        }
    }

    void DrawShips()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        for (int i = 0; i < ships.Count; i++) // loopa genom alla skepp i listan och få vilket håll den längden ska åka mot och sedan anpassa värdet till gridSpacing (mellarnummen mellan bokstäver / karaktärer)
        {
            Ship shipReference = ships[i];
            Vector2 shipStartPosition = shipReference.startPosition;
            Vector2 shipEndPosition = shipReference.endPosition;
            ShipDirection shipDirection = shipReference.direction;
            int shipSize = shipReference.size;

            for (int j = 0; j < shipSize; j++) // vi skriver punkterna enskilt, mängden punkter som ska ritas är baserat på skeppstorleket och sedan kollar vi åt vilket håll den ska åka
            {
                Console.SetCursorPosition(gridStart + ((shipDirection == ShipDirection.horizontal ? 1 : 0) * (j * (int)gridSpacing.X)) + (int)(shipStartPosition.X * gridSpacing.X),
                                          gridStart + ((shipDirection == ShipDirection.vertical ? 1 : 0) * (j * (int)gridSpacing.Y)) + (int)(shipStartPosition.Y * gridSpacing.Y));
                Console.Write(charShip);
            }
        }
    }
    void DrawHitPoints() // precis som ovanför, med en annan lista som enbart har X, Y punkter och inte en slutposition (startposition och endpositoin är samma i det här fallet)
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

    void DrawMessages() // rita vanliga meddelanden och sedan roundMessage, vilket sätts genom funktionerna som leder hittills
    {
        Console.SetCursorPosition(0, (int)gridSpacing.Y * gameSize + gridStart); // sätt cursorpositionen till slutet av spelytan med mellanrum så att vi kan tydligt se vad våran inputs är
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine($"Points left to hit: {(pointsToHit - pointsAttacked.Count)}");
        Console.WriteLine($"You have shot: {(pointsShot.Count())} times");
        Console.WriteLine($"Write 'attack XY' to attack a point and 'help' for more commands.");
        Console.WriteLine($"\n{roundMessage}");

        roundMessage = string.Empty; // ersätt roundMessage med en tom string när vi har skrivit ut den, så att nästa omgång inte har kvarstående data som fyller upp console fönstret
    }

    void DrawGame()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        DrawGamePlane();
        if (shipsVisible) // rita bara ut skeppens positioner ifall vi har själva satt den att göra det, genom ett kommando
            DrawShips();
        DrawHitPoints();
        DrawMessages(); // alla ritfunktioner kallas här
    }

    void AddRoundMessage(string message) => roundMessage += $"{message}\n"; // plussar meddelandet och en ny linje för nästa meddelande som sedan skrivs ut i DrawMessages()
}