using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class HeroFactory : CreatureFactory<Hero>
{
    public HeroFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
        base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override void OnCreate(Hero creature)
    {
        var stats = new Stats
        {
            MaxHealth = 2,
            Speed = Configuration.BaseCreatureSpeed,
            DefaultAttack = new BasicAttackAction(creature)
        };
        creature.Name = "Hero";
        creature.Texture = ContentRepository.Creature;
        creature.Stats = stats;
        creature.Color = Configuration.PlayerColor.ToColor();
    }
}