using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Random.Dice;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public abstract class CreatureFactory<TActor> : ActorFactoryBase<TActor> where TActor : Creature, new()
{
    public CreatureFactory(Configuration configuration,
                           ActorPool actorPool,
                           CollisionService collisionService,
                           ContentRepository contentRepository,
                           DiceRoller diceRoller) :
                      base(configuration, actorPool, collisionService, contentRepository)
    {
        DiceRoller = diceRoller;
    }

    protected DiceRoller DiceRoller { get; }

    protected override TActor Create(Vector2 position, string? name = null)
    {
        var creature = new TActor
        {
            Position = position,
            LastPosition = position
        };
        creature.AvailableActions = new AvailableActions(creature);
        OnCreate(creature);
        creature.Health = creature.Stats.MaxHealth;

        creature.Collision = new Collision
        {
            Actor = creature,
            Bounds = new CircleF
            {
                Radius = Configuration.BaseCreatureRadius
            }
        };
        CollisionService.Insert(creature.Collision);

        return creature;
    }

    protected abstract void OnCreate(TActor creature);
}