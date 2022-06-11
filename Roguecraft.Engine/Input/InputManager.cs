using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Cameras;

namespace Roguecraft.Engine.Input;

public class InputManager
{
    private readonly CameraService _cameraService;

    public InputManager(CameraService cameraService)
    {
        _cameraService = cameraService;
    }

    public InputState State { get; private set; }
    private GamePadState GamePadState { get; set; }
    private GamePadState LastGamePadState { get; set; }
    private MouseStateExtended? LastMouseState { get; set; }
    private MouseStateExtended MouseState { get; set; }

    public void Update()
    {
        LastGamePadState = GamePadState;
        GamePadState = GamePad.GetState(0);
        if (LastGamePadState == default)
        {
            LastGamePadState = GamePadState;
        }

        LastMouseState = MouseState;
        MouseState = MouseExtended.GetState();
        if (LastMouseState is null)
        {
            LastMouseState = MouseState;
        }
        State = new InputState(GamePadState,
                               LastGamePadState,
                               KeyboardExtended.GetState(),
                               MouseState,
                               LastMouseState.Value,
                               _cameraService);
    }
}