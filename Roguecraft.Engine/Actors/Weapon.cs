using Roguecraft.Engine.Actions.Combat;

namespace Roguecraft.Engine.Actors;

public class Weapon : Item
{
    public AttackAction AttackAction { get; set; }

    protected override AttackAction OnGetAttack()
    {
        return AttackAction;
    }

    protected override void OnPickUp(Creature creature)
    {
    }

    protected override void OnQuickAction(Creature creature)
    {
        AttackAction.Creature = creature;
        creature.EquipedItem = this;
    }
}