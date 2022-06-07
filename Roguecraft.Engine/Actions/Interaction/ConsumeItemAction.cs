﻿using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Interaction;

public class ConsumeItemAction : GameAction
{
    public ConsumeItemAction(Creature creature) : base(creature)
    {
        EngeryCost = 400;
    }

    private Item Item { get; set; }

    public override bool TryPrepare()
    {
        if (Creature.Energy < 0)
        {
            return false;
        }

        var door = Creature.AreaOfInfluence.FirstOrDefault<Item>();
        if (door is null)
        {
            return false;
        }
        Item = (Item)door.Other.Actor;
        return true;
    }

    protected override void OnPerform(float deltaTime)
    {
        Item?.QuickAction(Creature);
    }
}