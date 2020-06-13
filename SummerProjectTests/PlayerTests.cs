using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using SummerProject;
using SummerProject.Input;
using Xunit;

namespace SummerProjectTests
{
    public class PlayerTests
    {
        private Player _player;
        private Mock<IKeyboard> _mockKeyboard;
        private const int OriginalPoint = 10;

        public PlayerTests()
        {
            _mockKeyboard = new Mock<IKeyboard>();
            _player = new Player(_mockKeyboard.Object);
            _player.X = OriginalPoint;
            _player.Y = OriginalPoint;
        }

        [Fact]
        public void PressingDownIncreasesY()
        {
            _mockKeyboard.Setup(kb => kb.IsKeyDown(Keys.Down)).Returns(true);

            _player.GetPlayerInput();

            _player.Y.Should().Be(OriginalPoint + 1);
        }

        [Fact]
        public void PressingUpDecreasesY()
        {
            _mockKeyboard.Setup(kb => kb.IsKeyDown(Keys.Up)).Returns(true);

            _player.GetPlayerInput();

            _player.Y.Should().Be(OriginalPoint - 1);
        }

        [Fact]
        public void PressingLeftDecreasesX()
        {
            _mockKeyboard.Setup(kb => kb.IsKeyDown(Keys.Left)).Returns(true);

            _player.GetPlayerInput();

            _player.X.Should().Be(OriginalPoint - 1);
        }

        [Fact]
        public void PressingRightIncreasesX()
        {
            _mockKeyboard.Setup(kb => kb.IsKeyDown(Keys.Right)).Returns(true);

            _player.GetPlayerInput();

            _player.X.Should().Be(OriginalPoint + 1);
        }
    }
}
