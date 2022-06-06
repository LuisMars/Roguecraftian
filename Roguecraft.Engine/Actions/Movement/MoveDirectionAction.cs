using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Movement;

public class MoveDirectionAction : MoveAction
{
    public MoveDirectionAction(Creature actor, Vector2 direction) : base(actor)
    {
        Direction = direction;
    }
}