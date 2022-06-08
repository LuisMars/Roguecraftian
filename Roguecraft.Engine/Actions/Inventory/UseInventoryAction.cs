using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Inventory;

public class UseInventoryAction : GameAction
{
    public UseInventoryAction(Creature creature) : base(creature)
    {
    }

    private Item? Item { get; set; }

    public override bool TryPrepare()
    {
        Item = Creature.Inventory.UseCurrentItem();
        return Item is not null;
    }

    protected override void OnPerform(float deltaTime)
    {
        Item?.DefaultAction(Creature);
    }
}