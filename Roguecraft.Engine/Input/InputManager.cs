using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Roguecraft.Engine.Input;

public class InputManager
{
    public InputState State { get; private set; }

    public void Update()
    {
        State = new InputState(GamePad.GetState(0), KeyboardExtended.GetState());
    }
}