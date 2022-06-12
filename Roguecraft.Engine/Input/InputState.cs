using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Cameras;

namespace Roguecraft.Engine.Input;

public class InputState
{
    private readonly CameraService _cameraService;

    private readonly Dictionary<InputAction, Func<GamePadState, GamePadState, bool>> _gamePadMapping = new()
    {
        { InputAction.MoveUp, (g, g2) => g.DPad.Up == ButtonState.Pressed },
        { InputAction.MoveDown, (g, g2) => g.DPad.Down == ButtonState.Pressed },
        { InputAction.MoveLeft, (g, g2) => g.DPad.Left == ButtonState.Pressed },
        { InputAction.MoveRight, (g, g2) => g.DPad.Right == ButtonState.Pressed },
        { InputAction.QuickAction, (g, g2) => g.Buttons.A == ButtonState.Pressed },
        { InputAction.PickUp, (g, g2) => g.Buttons.X == ButtonState.Pressed },
        { InputAction.InventoryNext, (g, g2) => g.Buttons.RightShoulder == ButtonState.Pressed && g2.Buttons.RightShoulder == ButtonState.Released },
        { InputAction.InventoryPrev, (g, g2) => g.Buttons.LeftShoulder == ButtonState.Pressed && g2.Buttons.LeftShoulder == ButtonState.Released },
        { InputAction.InventoryUse, (g, g2) => g.Buttons.B == ButtonState.Pressed && g2.Buttons.B == ButtonState.Released },
    };

    private readonly GamePadState _gamePadState;

    private readonly Vector2 _invertJoystickY = new(1, -1);

    private readonly KeyboardStateExtended _keyboardState;

    private readonly Dictionary<InputAction, (Keys Key, bool JustPressed)> _keysMapping = new()
    {
        { InputAction.MoveUp, (Keys.W, false) },
        { InputAction.MoveDown, (Keys.S, false) },
        { InputAction.MoveLeft, (Keys.A, false) },
        { InputAction.MoveRight, (Keys.D, false) },
        { InputAction.PickUp, (Keys.E, false) },
        { InputAction.QuickAction, (Keys.Space, false) },
        { InputAction.InventoryPrev, (Keys.Up, true) },
        { InputAction.InventoryNext, (Keys.Down, true) },
        { InputAction.InventoryUse, (Keys.Q, true) },
    };

    private readonly Dictionary<InputAction, Func<MouseStateExtended, MouseStateExtended, bool>> _mouseMapping = new()
    {
        { InputAction.InventoryNext, (g, g2) => g.DeltaScrollWheelValue > 0 },
        { InputAction.InventoryPrev, (g, g2) => g.DeltaScrollWheelValue < 0 },
        { InputAction.InventoryUse, (g, g2) => g.MiddleButton == ButtonState.Pressed && g2.MiddleButton == ButtonState.Released },
        { InputAction.PickUp, (g, g2) => g.RightButton == ButtonState.Pressed && g2.RightButton == ButtonState.Released },
        { InputAction.FollowMouse, (g, g2) => g.RightButton == ButtonState.Pressed },
        { InputAction.QuickAction, (g, g2) => g.LeftButton == ButtonState.Pressed && g2.LeftButton == ButtonState.Released },
    };

    private readonly MouseStateExtended _mouseState;
    private readonly GamePadState _previousGamePadState;
    private readonly MouseStateExtended _previousMouseState;

    public InputState(GamePadState gamePadState,
                      GamePadState previousGamePadState,
                      KeyboardStateExtended keyboardState,
                      MouseStateExtended mouseState,
                      MouseStateExtended previousMouseState,
                      CameraService cameraService)
    {
        _gamePadState = gamePadState;
        _previousGamePadState = previousGamePadState;
        _keyboardState = keyboardState;
        _mouseState = mouseState;
        _previousMouseState = previousMouseState;
        _cameraService = cameraService;
    }

    public bool IsMouseEvent
    {
        get
        {
            return _mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;
        }
    }

    public bool IsMouseUsed => _mouseState.DeltaPosition != Point.Zero && _previousMouseState.DeltaPosition != Point.Zero;

    internal Vector2 LeftJostick => _gamePadState.ThumbSticks.Left * _invertJoystickY;
    internal Vector2 MousePosition => _cameraService.ScreenToWorld(_mouseState.Position.ToVector2());

    public bool IsButtonDown(InputAction input)
    {
        if (IsKeyDown(input))
        {
            return true;
        }
        if (IsGamePadButtonDown(input))
        {
            return true;
        }
        if (IsMouseDown(input))
        {
            return true;
        }
        return false;
    }

    private bool IsGamePadButtonDown(InputAction input)
    {
        if (!_gamePadMapping.TryGetValue(input, out var checkButton))
        {
            return false;
        }

        return checkButton(_gamePadState, _previousGamePadState);
    }

    private bool IsKeyDown(InputAction input)
    {
        if (!_keysMapping.TryGetValue(input, out var value))
        {
            return false;
        }

        if (value.JustPressed)
        {
            return _keyboardState.WasKeyJustUp(value.Key);
        }
        return _keyboardState.IsKeyDown(value.Key);
    }

    private bool IsMouseDown(InputAction input)
    {
        if (!_mouseMapping.TryGetValue(input, out var checkButton))
        {
            return false;
        }

        return checkButton(_mouseState, _previousMouseState);
    }
}