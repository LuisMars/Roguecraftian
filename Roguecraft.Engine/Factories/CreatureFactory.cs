using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public abstract class CreatureFactory<TActor> : ActorFactoryBase<TActor> where TActor : Creature, new()
{
    public CreatureFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                      base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override TActor Create(Vector2 position, string? name = null)
    {
        var creature = new TActor
        {
            Position = position,
        };

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
        creature.AreaOfInfluence = new Collision
        {
            Actor = creature,
            Bounds = new CircleF
            {
                Radius = Configuration.BaseCreatureAreaOfInfluenceRadius
            },
            IsSensor = true
        };
        CollisionService.Insert(creature.AreaOfInfluence);

        creature.WalkAction = new WalkAction(creature);
        creature.ToggleDoorAction = new ToggleDoorAction(creature);
        return creature;
    }

    protected abstract void OnCreate(TActor creature);
}