using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerProject
{
    class player
    {
        public int x = 0;
        public int y = 0;
        public string spriteName = "Test_graphics/dummy_player";
            
        public void GetPlayerInput()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Down))
                    {
                y++;
            };
            if (state.IsKeyDown(Keys.Up))
            {
                y--;
            };
            if (state.IsKeyDown(Keys.Left))
            {
                x--;
            };
            if (state.IsKeyDown(Keys.Right))
            {
                x++;
            };
        }

    }
}
