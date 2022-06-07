using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Combat;

public class AttackSelectionAction : GameAction
{
    public AttackSelectionAction(Creature creature) : base(creature)
    {
    }

    private AttackAction AttackAction { get; set; }

    public override bool TryPrepare()
    {
        if (!TryFindAttack(out var attack))
        {
            return false;
        }
        AttackAction = attack;
        return attack?.TryPrepare() ?? false;
    }

    protected override void OnPerform(float deltaTime)
    {
        AttackAction?.Perform(deltaTime);
    }

    private bool TryFindAttack(out AttackAction attack)
    {
        attack = null;
        if (Creature.EquipedItem is not null)
        {
            attack = Creature.EquipedItem.GetAttack();
            return true;
        }
        if (Creature.Stats.UnarmedAttack is not null)
        {
            attack = Creature.Stats.UnarmedAttack;
            return true;
        }
        return false;
    }
}