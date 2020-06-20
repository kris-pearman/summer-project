using FluentAssertions;
using SummerProject;
using Xunit;

namespace SummerProjectTests
{
    public class MapDetailsTests
    {
        [Fact]
        public void MapOffsetCaculatedWhenPlayerMovesRight()
        {
            var playerStart = new Location { X = 9, Y = 10 };
            var playerLocation = new Location { X = 10, Y = 10 };
            var mapDetails = new MapDetails(playerStart);

            mapDetails.CalculateMapPosition(playerLocation);

            var drawOffset = mapDetails.GetDrawOffset();
            drawOffset.X.Should().Be(-1);
            drawOffset.Y.Should().Be(0);
        }

        [Fact]
        public void MapOffsetCaculatedWhenPlayerMovesLeft()
        {
            var playerStart = new Location { X = 9, Y = 10 };
            var playerLocation = new Location { X = 8, Y = 10 };
            var mapDetails = new MapDetails(playerStart);

            mapDetails.CalculateMapPosition(playerLocation);

            var drawOffset = mapDetails.GetDrawOffset();
            drawOffset.X.Should().Be(1);
            drawOffset.Y.Should().Be(0);
        }

        [Fact]
        public void MapOffsetCaculatedWhenPlayerMovesDown()
        {
            var playerStart = new Location { X = 9, Y = 10 };
            var playerLocation = new Location { X = 9, Y = 11 };
            var mapDetails = new MapDetails(playerStart);

            mapDetails.CalculateMapPosition(playerLocation);

            var drawOffset = mapDetails.GetDrawOffset();
            drawOffset.X.Should().Be(0);
            drawOffset.Y.Should().Be(-1);
        }

        [Fact]
        public void MapOffsetCaculatedWhenPlayerMovesUp()
        {
            var playerStart = new Location { X = 9, Y = 10 };
            var playerLocation = new Location { X = 9, Y = 9 };
            var mapDetails = new MapDetails(playerStart);

            mapDetails.CalculateMapPosition(playerLocation);

            var drawOffset = mapDetails.GetDrawOffset();
            drawOffset.X.Should().Be(0);
            drawOffset.Y.Should().Be(1);
        }

    }
}
