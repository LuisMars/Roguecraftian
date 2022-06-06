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

    protected override void OnCreate(Enemy enemy)
    {
        var stats = new Stats
        {
            MaxHealth = 2,
            Speed = Configuration.BaseCreatureSpeed * 0.75f
        };
        enemy.Name = "Enemy";
        enemy.Texture = ContentRepository.Enemy;
        enemy.Stats = stats;
        enemy.Color = Configuration.EnemyColor.ToColor();
        enemy.Hero = ActorPool.Hero;

        //enemy.AvailableActions.Add(new HeroVisibleTrigger(), new BasicAttackAction(enemy));
        //enemy.AvailableActions.Add(new HeroNotVisibleTrigger(), new ToggleDoorAction(enemy));
        //enemy.AvailableActions.Add(new AutoTrigger(), new ChaseHeroAction(enemy, ActorPool.Hero));
    }
}