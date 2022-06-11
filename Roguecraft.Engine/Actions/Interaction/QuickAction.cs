using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Interaction;

public class QuickAction : GameAction
{
    public QuickAction(Creature creature) : this(creature, null)
    {
    }

    public QuickAction(Creature creature, InputManager inputManager) : base(creature, inputManager)
    {
        EngeryCost = 400;
    }

    private Item Item { get; set; }

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
        Item = (Item)item.Other.Actor;
        return true;
    }

    protected override void OnPerform(float deltaTime)
    {
        Item?.DefaultAction(Creature);
    }
}