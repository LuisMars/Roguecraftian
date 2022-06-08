﻿using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Timers;

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

    protected override void OnDefaultAction(Creature creature)
    {
        creature.Timers[TimerType.DrawDagger].Reset();
        AttackAction.Creature = creature;
        creature.EquipedItem = this;
    }
}