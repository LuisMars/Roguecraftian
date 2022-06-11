using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actions.Combat;

public class AttackAction : GameAction
{
    private readonly RandomGenerator _random;

    public AttackAction(RandomGenerator random) : this(null, random, null)
    {
    }

    public AttackAction(Creature actor, RandomGenerator random) : this(actor, random, null)
    {
    }

    public AttackAction(Creature actor, RandomGenerator random, InputManager inputManager) : base(actor, inputManager)
    {
        EngeryCost = 500;
        _random = random;
    }

    public int MaxDamage { get; init; } = 1;
    public int MinDamage { get; init; } = 0;
    public Creature Target { get; private set; }

    public int GetDamage()
    {
        return _random.FromRange(MinDamage, MaxDamage);
    }

    public override bool TryPrepare(bool useMouse)
    {
        if (Creature.Energy < 0)
        {
            return false;
        }
        var creatureEvent = GetSelected<Creature>(useMouse,
            x => !x.Other.IsSensor &&
            x.Other.Actor != Creature &&
            Creature.Alignement.IsHostileTo(((Creature)x.Other.Actor).Alignement));

        if (creatureEvent is null)
        {
            return false;
        }
        var creature = (Creature)creatureEvent.Other.Actor;
        BindTarget(creature);
        return true;
    }

    internal void BindTarget(Creature target)
    {
        Target = target;
    }

    protected virtual int OnAttack(float deltaTime)
    {
        var damageDealt = GetDamage();
        Target.Health -= damageDealt;
        return damageDealt;
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Timers[TimerType.Attack].Reset(0.5f);
        Creature.Angle = (Target.Position - Creature.Position).ToAngle();
        var damageDealt = OnAttack(deltaTime);
        if (damageDealt == 0)
        {
            return;
        }
        Target.Timers[TimerType.Hurt].Reset(0.5f);
    }
}