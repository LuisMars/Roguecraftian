using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class EnemyFactory : CreatureFactory<Enemy>
{
    public EnemyFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
        base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override void OnCreate(Enemy creature)
    {
        var stats = new Stats
        {
            MaxHealth = 2,
            Speed = Configuration.BaseCreatureSpeed * 0.75f,
            DefaultAttack = new BasicAttackAction(creature)
        };
        creature.Name = "Enemy";
        creature.Texture = ContentRepository.Enemy;
        creature.Stats = stats;
        creature.Color = Configuration.EnemyColor.ToColor();
        creature.Hero = ActorPool.Hero;
    }
}