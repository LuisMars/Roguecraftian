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
        var wasStill = Creature.Direction == Vector2.Zero;
        Creature.StillFrames++;
        Creature.Direction = Vector2.Zero;
        if (wasStill)
        {
            Creature.IsStill = Creature.StillFrames > 30;
        }
    }
}