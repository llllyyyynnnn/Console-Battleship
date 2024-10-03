using System.Numerics;
using System.Security.Cryptography;

public partial class Game
{
    public static bool IsInsideRect(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2) => !(endPos1.X < startPos2.X || startPos1.X > endPos2.X || endPos1.Y < startPos2.Y || startPos1.Y > endPos2.Y);
    void PauseGame() => System.Threading.Thread.Sleep(gamePauseTime);

    public static int RandomInt(int min, int max)
    {
        byte[] randomNumber = new byte[4];
        RandomNumberGenerator.Fill(randomNumber);
        int value = BitConverter.ToInt32(randomNumber, 0);
        return Math.Abs(value % (max - min)) + min;
    }

    public struct coordinateInput
    {
        public int x, y;
        public bool valid;
    }

    coordinateInput ValidateCoordinateInput(string coordinate)
    {
        coordinateInput input;
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
        {
            Console.WriteLine("Input was invalid or out of range.");
            PauseGame();
        }

        return input;
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
                coordinateInput userValidatedInput = ValidateCoordinateInput(splitInput[1]);

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
}