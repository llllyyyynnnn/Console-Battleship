using System.Numerics;

public partial class Game
{
    void AddShip(int length)
    {
        Vector2 randomPosition = new Vector2(RandomInt(0, gameSize), RandomInt(0, gameSize)); // generera en slumpmässig koordinat (X, Y) i en Vector2 variabel
        ShipDirection randomDirection = (ShipDirection)(RandomInt(0, 100) % 2); // bestäm åt vilket håll skeppet ska vara genom att generera ett slumpmässigt tal från 0 till 100 som sedan omvandlas till resterna av divisionen av 2, vilket ger 0 - 1
                                                                                // värdet castas sedan från int 0 - 1 till ShipDirection, vilket resulterar i antingen ShipDirection.horizontal eller .vertical
        bool canContinue = (randomDirection == ShipDirection.horizontal && randomPosition.X + length <= gameSize) ||
                            randomDirection == ShipDirection.vertical && randomPosition.Y + length <= gameSize; // är den slumpmässiga koordinaten större än spelplanen både från början eller efter längden har tillsatts? forsätt med att omgenerera skeppets koordinater ifall det inte funkar.

        if (canContinue)
        {
            Vector2 startPosition = new Vector2(randomPosition.X, randomPosition.Y);
            Vector2 endPosition = new Vector2(randomPosition.X + ((randomDirection == ShipDirection.horizontal ? 1 : 0) * length),
                                              randomPosition.Y + ((randomDirection == ShipDirection.vertical ? 1 : 0) * length)); // slutpositionen bestäms genom att sätta ett värde baserat på ifall randomDirection == ShipDirection, vilket ger antingen 0 eller 1 vilket multipliceras med längden.
                                                                                                                                  // ifall ShipDirection är horizontal kommer den returnera 1 * längd, medans ShipDirection.vertical kommer returnera 0 * längd, vilket gör att skeppet enbart åker ner eller åt sidan

            if (ships.Count > 0) // ifall det redan finns skepp, kolla genom hela listan och jämför alla skepp-objekt för att säkerställa att vi inte krockar med dem
            {
                for (int i = 0; i < ships.Count; i++) // for loop från 0 till skepp-listans mängd av objekt i sig
                {
                    if (!canContinue) // ifall vi är i insidan av en annan skepps rektangel baserat på start och slutpositionen av den jämfört med det nya skeppet som ska genereras kommer vi inte fortsätta då canContinue sätts till 'false' och vi istället kallar AddShip funktionen igen med samma argument för att starta om processen
                        continue;

                    Ship shipReference = ships[i]; // skapa ett referensobjekt för skeppet som vi loopar genom
                    ShipDirection shipDirection = shipReference.direction; // ta reda på vilket håll skeppet åker
                    int shipSize = shipReference.size; // ta reda på storleken av skeppet

                    Vector2 shipStartPosition = shipReference.startPosition;
                    Vector2 shipEndPosition = shipReference.endPosition;

                    canContinue = !IsInsideRect(startPosition, endPosition, shipStartPosition - shipsMinimumDistance, shipEndPosition + shipsMinimumDistance); // ifall skeppet inte är inuti objektreferensen ska vi fortsätta, vi har inte krockat med något och har inget att oroa oss för
                }
            }

            if (canContinue) // ifall funktionen ovanför passerar och canContinue fortfarande är 'true' innebär det att inget har krockat alls, då kan vi skapa skeppet.
            {
                Ship ship = new Ship(); // skapa en ny skepp-objekt
                ship.startPosition = startPosition; // tillsätt alla variabler
                ship.endPosition = endPosition;
                ship.direction = randomDirection;
                ship.size = length;

                ships.Add(ship); // lägg till skepp-objektet till lista
                pointsToHit += length; // mängden träff som krävs adderas med storleken av skeppet
                return; // stoppa funktionen, vi är klara och ska returna till vart vi kallades från
            }
        }

        AddShip(length); // har vi inte returnat ovanför är canContinue satt till 'false' och ett krock hade skett mellan våra slumpmässiga koordinater och ett annat skepp eller hamnat utanför spelplanen pga längd. kör om hela funktionen
    }

    void AttackPoint(Vector2 position)
    {
        if (pointsAttacked.Contains(position) || pointsShot.Contains(position)) // låt inte spelaren träffa samma område mer än en gång, leder till infinita poängökningar
        {
            Console.WriteLine("Point already hit!");
            return;
        }

        pointsShot.Add(position); // har vi inte träffat den här punkten ska vi registrera det och sedan kolla genom alla skepp-objekt i skepp-listan för att se ifall vi har träffat en tom punkt eller en med ett skepp i

        if (ships.Count > 0) // finns det skepp, så ska vi loopa genom allt igen
        {
            for (int i = 0; i < ships.Count; i++) // for loop igen genom skepp-listan
            {
                Ship shipReference = ships[i];
                ShipDirection shipDirection = shipReference.direction;
                int shipSize = shipReference.size;

                Vector2 shipStartPosition = shipReference.startPosition;
                Vector2 shipEndPosition = shipReference.endPosition - new Vector2((shipDirection == ShipDirection.horizontal ? 1 : 0) * 1, (shipDirection == ShipDirection.vertical ? 1 : 0) * 1); // -1 eftersom att min InsideRect funktion är för precis och registerar ett träff då den yttersta kanten av skeppet överlappas av den innersta kanten av den tomma ytan

                if (IsInsideRect(position, position, shipStartPosition, shipEndPosition)) // är vår punkt inom ett skepp?
                {
                    pointsAttacked.Add(position); // registrera det som ett framgångsrikt träff i den andra listan

                    shipReference.hit += 1; // objektreferensen för skeppet ska öka sin hit-counter vilket vi använder för att få reda på ifall skeppet är förstört eller inte
                    ships[i] = shipReference; // ersätt den nuvarande referensen i listan med objektreferensen för att ersätta med de nya värden

                    AddRoundMessage($"Ship was found in {(char)(position.X + alphabetCapsASCIIStartIndex)}{position.Y}"); // sätt ett meddelande att ett skepp har träffats för rundan
                    if (shipReference.hit >= shipSize)
                        AddRoundMessage("Ship has been fully destroyed!"); // är mängden träff på skeppet lika med eller mer än skeppstorleken har vi förstört skeppet

                    return;
                }
            }
        }

        AddRoundMessage($"No ship was found in {(char)(position.X + alphabetCapsASCIIStartIndex)}{position.Y}."); // ifall vi inte har returnat ovanför har vi inte träffat något skepp
    }
}
