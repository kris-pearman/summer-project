using Microsoft.Xna.Framework.Input;
using SummerProject.Input;


namespace SummerProject
{
    public class Player
    {
        private readonly IKeyboard _keyboard;

        public int X = 0;
        public int Y = 0;

        public Location Location
        {
            get
            {
                return new Location { X = X, Y = Y };
            }
        }

        public string SpriteLocation = "Test_graphics/dummy_player";

        public Player(IKeyboard keyboard)
        {
            _keyboard = keyboard;
        }

        public void GetPlayerInput()
        {
            if (_keyboard.IsKeyDown(Keys.Down))
            {
                Y++;
            };
            if (_keyboard.IsKeyDown(Keys.Up))
            {
                Y--;
            };
            if (_keyboard.IsKeyDown(Keys.Left))
            {
                X--;
            };
            if (_keyboard.IsKeyDown(Keys.Right))
            {
                X++;
            };
        }

    }
}
