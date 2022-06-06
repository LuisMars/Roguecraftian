using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Combat;

public class BasicAttackAction : AttackAction
{
    public BasicAttackAction(Creature actor) : base(actor)
    {
    }

    protected override void OnAttack(float deltaTime)
    {
        Target.Health -= MaxDamage;
    }
}