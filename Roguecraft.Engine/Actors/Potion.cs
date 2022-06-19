using Roguecraft.Engine.Actions.Effects;

namespace Roguecraft.Engine.Actors;

public class Potion : Item
{
    public HealAction HealAction { get; set; }

    public override bool TryPrepare(Creature creature)
    {
        HealAction.Creature = creature;
        return HealAction.TryPrepare(false);
    }

    protected override void OnDefaultAction(Creature creature)
    {
        HealAction.Creature = creature;
        HealAction.Perform();
    }
}