using Microsoft.Xna.Framework.Input;

namespace SummerProject.Input
{
    public class KeyboardKeys : IKeyboard
    {
        public bool IsKeyDown(Keys key)
        {
            KeyboardState state = Keyboard.GetState();

            return state.IsKeyDown(key);
        }
    }
}
