using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Movement;

public class JoystickMovementAction : MoveAction
{
    private readonly InputManager _inputManager;

    public JoystickMovementAction(Creature actor, InputManager inputManager) : base(actor)
    {
        _inputManager = inputManager;
    }

    public override Vector2 GetDirection()
    {
        Direction = _inputManager.State.LeftJostick;
        return base.GetDirection();
    }
}