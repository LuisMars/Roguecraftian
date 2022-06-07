using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace Roguecraft.Engine.Input;

public class InputState
{
    private readonly Dictionary<InputAction, Func<GamePadState, bool>> _gamePadMapping = new()
    {
        { InputAction.MoveUp, g => g.DPad.Up == ButtonState.Pressed },
        { InputAction.MoveDown, g => g.DPad.Down == ButtonState.Pressed },
        { InputAction.MoveLeft, g => g.DPad.Left == ButtonState.Pressed },
        { InputAction.MoveRight, g => g.DPad.Right == ButtonState.Pressed },
        { InputAction.QuickAction, g => g.Buttons.A == ButtonState.Pressed },
        { InputAction.Equip, g => g.Buttons.B == ButtonState.Pressed },
    };

    private readonly GamePadState _gamePadState;

    private readonly Vector2 _invertJoystickY = new(1, -1);
    private readonly KeyboardStateExtended _keyboardState;

    private readonly Dictionary<InputAction, Keys> _keysMapping = new()
    {
        { InputAction.MoveUp, Keys.W },
        { InputAction.MoveDown, Keys.S },
        { InputAction.MoveLeft, Keys.A },
        { InputAction.MoveRight, Keys.D },
        { InputAction.Equip, Keys.E },
        { InputAction.QuickAction, Keys.Space },
    };

    public InputState(GamePadState gamePadState, KeyboardStateExtended keyboardState)
    {
        _gamePadState = gamePadState;
        _keyboardState = keyboardState;
    }

    internal Vector2 LeftJostick => _gamePadState.ThumbSticks.Left * _invertJoystickY;

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
        return false;
    }

    private bool IsGamePadButtonDown(InputAction input)
    {
        if (!_gamePadMapping.TryGetValue(input, out var checkButton))
        {
            return false;
        }

        return checkButton(_gamePadState);
    }

    private bool IsKeyDown(InputAction input)
    {
        if (!_keysMapping.TryGetValue(input, out var key))
        {
            return false;
        }

        return _keyboardState.IsKeyDown(key);
    }
}