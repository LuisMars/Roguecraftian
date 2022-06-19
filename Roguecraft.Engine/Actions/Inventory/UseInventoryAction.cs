using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Inventory;

public class UseInventoryAction : GameAction
{
    public UseInventoryAction(Creature creature) : base(creature)
    {
    }

    public UseInventoryAction(Creature creature, InputManager inputManager) : base(creature, inputManager)
    {
    }

    private Item? Item { get; set; }

    public override bool TryPrepare(bool useMouse)
    {
        Item = Creature.Inventory.CurrenItem;
        return Item is not null && Item.TryPrepare(Creature);
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Inventory.UseCurrentItem();
        Item?.DefaultAction(Creature);
    }
}