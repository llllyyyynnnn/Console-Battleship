using System.Numerics;
using System.Security.Cryptography;

public partial class Game
{
    public static bool IsInsideRect(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2) => !(endPos1.X < startPos2.X || startPos1.X > endPos2.X || endPos1.Y < startPos2.Y || startPos1.Y > endPos2.Y); // returnera true ifall vi har rektangel1 är inuti eller överlappar med rektangel2 (genom att kolla vec2 start och slut, ger x, y, w, h)
    void PauseGame() => System.Threading.Thread.Sleep(gamePauseTime); // thread.sleep funktionen agerar genom att stoppa programmets thread i x milisekunder

    public static int RandomInt(int min, int max)
    {
        byte[] randomNumber = new byte[4];
        RandomNumberGenerator.Fill(randomNumber);
        int value = BitConverter.ToInt32(randomNumber, 0);
        return Math.Abs(value % (max - min)) + min;
    } // riktig random generation som inte behöver pauser och kan köras flera gånger i samma frame utan att få samma nummer flera gånger

    public struct coordinateInput
    {
        public int x, y; // kan göras om till en public Vector2 position, men då krävs det också att vi castar om till int i console.setcursorpos då den enbart vill ha int och inga float värden
        public bool valid;
    } // används för att validera koordinaten (ifall input som spelaren har gett är aktuella och ligger inom spelplanen)

    coordinateInput ValidateCoordinateInput(string coordinate)
    {
        coordinateInput input; // skapa en objektreferens baserat på coordinateInput-strukturen
        input.valid = false;
        input.x = 0;
        input.y = 0; // sätt startvärden för att säkerställa att vi kan returnera något och att vi inte har en null-referens

        bool validCoordinate = (coordinate != null && coordinate.Length == 2); // ifall input som vi har fått inte är null och är exakt 2 karaktärer stort
        bool inRange = false; // ifall vi är inom spelplanen sätts inRange till true, annars stannar den false och input.valid blir också false

        if (validCoordinate)
        {
            int letterCoordinate = (int)coordinate.ToLower()[0]; // från char till ascii-decimalvärdet i int (subtraheras i under för att få värde från 0 till spelplanens maxstorlek9
            int numberCoordinate = coordinate[1]; // andra karaktären inom string inputten som vi har fått in bör vara ett nummer, vi kollar det genom att säkerställa att positionen är från ascii startvärdet i decimalform för nummer och bokstäver

            inRange = (letterCoordinate >= alphabetASCIIStartIndex && letterCoordinate <= alphabetASCIIStartIndex + gameSize) && numberCoordinate >= numberASCIIStartIndex && numberCoordinate <= numberASCIIStartIndex + gameSize; // förklarat ovanför
            if (inRange) // är vi inom spelytan? sätt input.x och .y till positionen från 0 till spelplanens maxstorlek, görs genom att subtrahera ascii värdets startpunkt med decimalvärdet vi fick in tidigare
            {
                input.x = letterCoordinate - alphabetASCIIStartIndex;
                input.y = numberCoordinate - numberASCIIStartIndex;
                input.valid = true; // valid = true innebär att funktionen som ursprunglingen kallade ValidateCoordinateInput får signalen att vi har fått riktiga koordinater inom spelytan, då fortsätter den
            }
        }

        if (input.valid == false) // annars, varna spelaren och pausa spelet
        {
            Console.WriteLine("Input was invalid or out of range.");
            PauseGame();
        }

        return input; // nu kan vi returnera coordinateInput objektet som vi har skapat
    }

    void RegisterCommand(string input) // här hanteras alla kommandon, eftersom att den körs i en try funktion med catch behöver vi inte nullchecka alla kommandon då det inte kommer terminera programmet ifall något går fel
    {
        string[] splitInput = input.ToLower().Split(' '); // splittra string till en array med flera strings baserat på mellanrum

        switch (splitInput[0]) // case med det första argumenten, själva kommandot
        {
            case "show":
                if (splitInput[1] == "ships") // kolla vad det är vi vill köra "show" på
                    shipsVisible = true;
                break;
            case "hide":
                if (splitInput[1] == "ships")
                    shipsVisible = false;
                break;
            case "attack": // ifall vi vill attackera, skickar vi nästa string array till ValidateCoordinateInput för att säkerästlla att koordinaterna är aktuella
                coordinateInput userValidatedInput = ValidateCoordinateInput(splitInput[1]);

                if (userValidatedInput.valid) // ifall de är valid ska vi fortsätta med att attackera punkten, annars händer ingenting
                    AttackPoint(new Vector2(userValidatedInput.x, userValidatedInput.y));
                break;

            case "reset":
                ResetGame();
                break;

            case "set":
                if (splitInput[1] == "pause")
                    gamePauseTime = int.Parse(splitInput[2]); // ändrar pausetiden i milisekunder
                break;
        }
    }
}