
namespace SummerProject
{
    public class MapDetails
    {
        private readonly Location _playerStartPosition;
        private Location _drawOffset;

        public MapDetails(Location playerStartPosition)
        {
            _playerStartPosition = playerStartPosition;
        }

        public void CalculateMapPosition(Location playerPosition)
        {
            _drawOffset = new Location
            {
                X = _playerStartPosition.X - playerPosition.X,
                Y = _playerStartPosition.Y - playerPosition.Y
            };
        }

        public Location GetDrawOffset()
        {
            return _drawOffset;
        }
    }
}
