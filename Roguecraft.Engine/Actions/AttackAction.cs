﻿using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public abstract class AttackAction : GameAction
{
    protected AttackAction(Creature actor) : base(actor)
    {
    }

    public int MaxDamage { get; init; } = 1;
    public int MinDamage { get; init; } = 0;
    public Creature Target { get; private set; }

    internal void BindTarget(Creature target)
    {
        Target = target;
    }
}