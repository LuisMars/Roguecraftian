using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Roguecraft.Engine.Input;

public class InputManager
{
    public InputState State { get; private set; }
    private GamePadState GamePadState { get; set; }
    private GamePadState LastGamePadState { get; set; }

    public void Update()
    {
        LastGamePadState = GamePadState;
        GamePadState = GamePad.GetState(0);
        if (LastGamePadState == default)
        {
            LastGamePadState = GamePadState;
        }

        State = new InputState(GamePadState, LastGamePadState, KeyboardExtended.GetState());
    }
}