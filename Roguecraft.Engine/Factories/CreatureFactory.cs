using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class CreatureFactory<TActor> : ActorFactoryBase<TActor> where TActor : Creature, new()
{
    public CreatureFactory(ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) : base(actorPool, collisionService, contentRepository)
    {
    }

    protected override TActor Create(Vector2 position, string? name = null)
    {
        var creature = new TActor
        {
            Image = ContentRepository.Creature,
            Name = name ?? "Creature",
            Position = position,
            Speed = 100
        };
        var stats = new Stats
        {
            MaxHealth = 2,
            Speed = 1,
            DefaultAttack = new BasicAttack(creature)
        };
        creature.Stats = stats;
        creature.Health = creature.Stats.MaxHealth;

        creature.Collision = new Collision
        {
            Actor = creature,
            Bounds = new CircleF
            {
                Radius = 45f
            }
        };
        CollisionService.Insert(creature.Collision);
        creature.AreaOfInfluence = new Collision
        {
            Actor = creature,
            Bounds = new CircleF
            {
                Radius = 55f
            },
            IsSensor = true
        };
        CollisionService.Insert(creature.AreaOfInfluence);

        creature.WalkAction = new WalkAction(creature);
        return creature;
    }
}