using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Enemy : Creature
{
    public override GameAction? OnTakeTurn(float deltaTime)
    {
        Energy += Stats.Speed * deltaTime;
        Energy = Math.Min(0, Energy);
        if (TryAttack(out var attack))
        {
            return attack;
        }

        return NullAction;
    }

    private bool TryAttack(out AttackAction attack)
    {
        attack = null;
        if (Energy < 0)
        {
            return false;
        }
        var creatureEvent = AreaOfInfluence.LastEvents.FirstOrDefault(x => x.Other.Actor is Creature);
        if (creatureEvent?.Other?.Actor is not Creature creature || creature == this)
        {
            return false;
        }
        Stats.DefaultAttack.BindTarget(creature);
        attack = Stats.DefaultAttack;
        return true;
    }
}