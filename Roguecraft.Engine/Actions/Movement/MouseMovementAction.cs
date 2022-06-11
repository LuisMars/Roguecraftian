using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Movement;

public class MouseMovementAction : MoveAction
{
    private readonly InputManager _inputManager;

    public MouseMovementAction(Creature creature, InputManager inputManager) : base(creature)
    {
        _inputManager = inputManager;
    }

    public override Vector2 GetDirection()
    {
        Direction = (_inputManager.State.MousePosition - Creature.Position) / Creature.AreaOfInfluence.Width;
        Direction.ClampMagnitude(1);
        return base.GetDirection();
    }
}