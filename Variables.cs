using System.Numerics;

public partial class Game
{
    public struct Ship // structs definierar en variabel-struktur (som namnet låter) för att kunna återanvändas flera gånger. till skillnad från class kan värden inte pre-definieras
    {
        public Vector2 startPosition; // vart skeppet börjar på X, Y axeln
        public Vector2 endPosition; // vart skeppet slutar på X, Y axeln
        public ShipDirection direction; // vilket håll skeppet ska vara (horizontellt är enbart en förändring på X axeln, vertikalt är på Y axeln i endPosition)
        public int size; // storleken som sätts under skeppets skapelse, bestämmer vart endPosition ska vara beroende på vart direction är
        public int hit; // hur många gånger skeppet har träffats (ifall hit >= size är hela skeppet borta)
    }

    public enum ShipDirection
    {
        horizontal = 0, // när ShipDirection castas till en int får vi 0, vilket är horizontellt. används inom if satser för att förtydliga samt bools
        vertical = 1
    }

    bool gameOngoing = false; // ifall ett spel är pågående
    int gamePauseTime = 2000; // 2000ms pause för varje spel-event som kräver en paus (t.ex varning meddelanden som berättar för spelaren att koordinaterna är felaktiga, osv)
    static int gameSize = 10; // storleken av spelplanen (10 ger abcdefghij, 0-9)
    static int gridStart = 2; // spelplanen börjar på 2, 2
    Vector2 gridSpacing = new Vector2(gridStart + 2, gridStart); // mellanrummet mellan raderna och karaktärerna i X och Y. i det här fallet blir det 2+2, 2. det ger 4, 2 mellanrum mellan varje karaktär

    const char charFiller = '~'; // oattackerade delar av spelplanen har den här karaktären (const char innebär att char variabeln inte kan förändras efter att programmet har skapats)
    const char charMissed = '*'; // missade attacker blir *
    const char charShip = '#'; // träffade delar av ett skepp blir #

    const int alphabetASCIIStartIndex = 97; // ascii start från 65+x, vilket leder till abcdefghj
    const int alphabetCapsASCIIStartIndex = 65; // ABCDEFGHJ... samma som ovanför
    const int numberASCIIStartIndex = 48; // 0123456789... samma som ovanför

    string roundMessage = string.Empty; // meddelandet som printas för den aktiva rundan (ersätts av ett nytt meddelande varje gång man skjuter ett skott)

    List<Ship> ships = new List<Ship>(); // lista med skepp-objekt som består av skepp-strukturen
    List<Vector2> pointsAttacked = new List<Vector2>(); // lista med alla träffade delar av alla skeppen inom skepp-listan
    List<Vector2> pointsShot = new List<Vector2>(); // lista med alla skott som har skjutits runt spelplanen oavsett ifall de har träffat eller inte
    Vector2 shipsMinimumDistance = new Vector2(1, 1); // minimiavståndet mellan skeppen (skeppen kan inte kollidera med varandra eller utanför spelplanen oavsett)
    bool shipsVisible = false; // ifall vi ska rita skeppen på spelplanen eller inte (sätts av kommandon)
    int pointsToHit = 0; // mängden träff som krävs för att vinna (bestäms efter att skeppen placeras, plussas ihop med Ship.size varje gång en ny tillsätts
}