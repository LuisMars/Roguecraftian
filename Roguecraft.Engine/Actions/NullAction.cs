using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public class NullAction : GameAction
{
    public NullAction(Creature actor) : base(actor)
    {
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Direction = Vector2.Zero;
    }
}