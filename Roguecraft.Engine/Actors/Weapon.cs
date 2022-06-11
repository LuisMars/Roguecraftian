using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public class Weapon : Item
{
    public AttackAction AttackAction { get; set; }

    protected override void OnDefaultAction(Creature creature)
    {
        creature.Timers[TimerType.DrawDagger].Reset();
        AttackAction.Creature = creature;
        if (creature.EquipedItem != null)
        {
            creature.Inventory.Add(creature.EquipedItem);
        }
        creature.EquipedItem = this;
    }

    protected override AttackAction OnGetAttack()
    {
        return AttackAction;
    }

    protected override void OnPickUp(Creature creature)
    {
    }
}