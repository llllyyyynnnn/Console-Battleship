using System.Numerics;

public partial class Game
{
    public struct Ship
    {
        public Vector2 startPosition;
        public Vector2 endPosition;
        public ShipDirection direction;
        public int size;
        public int hit;
    }

    public enum ShipDirection
    {
        horizontal = 0,
        vertical = 1
    }

    void AddShip(int length)
    {
        Vector2 randomPosition = new Vector2(RandomInt(0, gameSize), RandomInt(0, gameSize));
        ShipDirection randomDirection = (ShipDirection)(RandomInt(0, 100) % 2);
        bool canContinue = (randomDirection == ShipDirection.horizontal && randomPosition.X + length <= gameSize) ||
                            randomDirection == ShipDirection.vertical && randomPosition.Y + length <= gameSize;

        if (canContinue)
        {
            Vector2 startPosition = new Vector2(randomPosition.X, randomPosition.Y);
            Vector2 endPosition = new Vector2(randomPosition.X + ((randomDirection == ShipDirection.horizontal ? 1 : 0) * length),
                                              randomPosition.Y + ((randomDirection == ShipDirection.vertical ? 1 : 0) * length));

            if (ships.Count > 0)
            {
                for (int i = 0; i < ships.Count; i++)
                {
                    if (!canContinue)
                        continue;

                    Ship shipReference = ships[i];
                    ShipDirection shipDirection = shipReference.direction;
                    int shipSize = shipReference.size;

                    Vector2 shipStartPosition = shipReference.startPosition;
                    Vector2 shipEndPosition = shipReference.endPosition;

                    canContinue = !IsInsideRect(startPosition, endPosition, shipStartPosition - shipsMinimumDistance, shipEndPosition + shipsMinimumDistance);
                }
            }

            if (canContinue)
            {
                Ship ship = new Ship();
                ship.startPosition = startPosition;
                ship.endPosition = endPosition;
                ship.direction = randomDirection;
                ship.size = length;

                ships.Add(ship);
                pointsToHit += length;
                return;
            }
        }

        AddShip(length);
    }

    void AttackPoint(Vector2 position)
    {
        if (pointsAttacked.Contains(position) || pointsShot.Contains(position))
        {
            Console.WriteLine("Point already hit!");
            return;
        }

        pointsShot.Add(position);

        if (ships.Count > 0)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                Ship shipReference = ships[i];
                ShipDirection shipDirection = shipReference.direction;
                int shipSize = shipReference.size;

                Vector2 shipStartPosition = shipReference.startPosition;
                Vector2 shipEndPosition = shipReference.endPosition - new Vector2((shipDirection == ShipDirection.horizontal ? 1 : 0) * 1, (shipDirection == ShipDirection.vertical ? 1 : 0) * 1); // -1 due to IsInsideRect being too precise and allowing the slightest of overlaps to be registered as a hit

                if (IsInsideRect(position, position, shipStartPosition, shipEndPosition))
                {
                    pointsAttacked.Add(position);

                    shipReference.hit += 1;
                    ships[i] = shipReference;

                    AddRoundMessage($"Ship was found in {(char)(position.X + alphabetCapsASCIIStartIndex)}{position.Y}");
                    if (shipReference.hit >= shipSize)
                        AddRoundMessage("Ship has been fully destroyed!");

                    return;
                }
            }
        }

        AddRoundMessage($"No ship was found in {(char)(position.X + alphabetCapsASCIIStartIndex)}{position.Y}.");
    }
}
