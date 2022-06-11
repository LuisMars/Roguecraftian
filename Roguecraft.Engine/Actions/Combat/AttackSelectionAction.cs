using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Combat;

public class AttackSelectionAction : GameAction
{
    public AttackSelectionAction(Creature creature) : this(creature, null)
    {
    }

    public AttackSelectionAction(Creature creature, InputManager inputManager) : base(creature, inputManager)
    {
    }

    private AttackAction AttackAction { get; set; }

    public override bool TryPrepare(bool useMouse)
    {
        if (!TryFindAttack(out var attack))
        {
            return false;
        }
        AttackAction = attack;
        return attack?.TryPrepare(useMouse) ?? false;
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
            attack.InputManager = InputManager;
            return true;
        }
        if (Creature.Stats.UnarmedAttack is not null)
        {
            attack = Creature.Stats.UnarmedAttack;
            attack.InputManager = InputManager;
            return true;
        }
        return false;
    }
}