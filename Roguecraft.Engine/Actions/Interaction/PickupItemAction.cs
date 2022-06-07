using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Interaction;

public class PickupItemAction : GameAction
{
    public PickupItemAction(Creature creature) : base(creature)
    {
        EngeryCost = 200;
    }

    private Item PickupItem { get; set; }

    public override bool TryPrepare()
    {
        if (Creature.Energy < 0)
        {
            return false;
        }

        var door = Creature.AreaOfInfluence.FirstOrDefault<Item>();
        if (door is null)
        {
            return false;
        }
        PickupItem = (Item)door.Other.Actor;
        return true;
    }

    protected override void OnPerform(float deltaTime)
    {
        PickupItem?.PickUp(Creature);
    }
}