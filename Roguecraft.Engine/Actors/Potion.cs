using Roguecraft.Engine.Actions.Effects;

namespace Roguecraft.Engine.Actors;

public class Potion : Item
{
    public HealAction HealAction { get; set; }

    protected override void OnDefaultAction(Creature creature)
    {
        HealAction.Creature = creature;
        HealAction.Perform();
    }
}