using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Interaction;

public class PickupItemAction : GameAction
{
    public PickupItemAction(Creature creature) : this(creature, null)
    {
    }

    public PickupItemAction(Creature creature, InputManager inputManager) : base(creature, inputManager)
    {
        EngeryCost = 200;
    }

    private Item PickupItem { get; set; }

    public override bool TryPrepare(bool useMouse)
    {
        if (Creature.Energy < 0)
        {
            return false;
        }

        var item = GetSelected<Item>(useMouse);
        if (item is null)
        {
            return false;
        }
        PickupItem = (Item)item.Other.Actor;
        return true;
    }

    protected override void OnPerform(float deltaTime)
    {
        PickupItem?.PickUp(Creature);
    }
}