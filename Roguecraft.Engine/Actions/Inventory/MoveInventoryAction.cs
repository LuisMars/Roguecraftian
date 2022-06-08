using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Inventory;

public class MoveInventoryAction : GameAction
{
    private readonly bool _moveNext;

    public MoveInventoryAction(Creature creature, bool moveNext) : base(creature)
    {
        _moveNext = moveNext;
    }

    protected override void OnPerform(float deltaTime)
    {
        if (_moveNext)
        {
            Creature.Inventory.SelectNext();
            return;
        }

        Creature.Inventory.SelectPrev();
    }
}