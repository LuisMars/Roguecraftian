using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Actions.EnemyAI;
using Roguecraft.Engine.Actions.Interaction;
using Roguecraft.Engine.Actions.Triggers;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Random.Dice;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class EnemyFactory : CreatureFactory<Enemy>
{
    private readonly PotionFactory _potionFactory;
    private readonly RandomGenerator _randomGenerator;

    public EnemyFactory(Configuration configuration,
                        ActorPool actorPool,
                        CollisionService collisionService,
                        ContentRepository contentRepository,
                        RandomGenerator randomGenerator,
                        PotionFactory potionFactory,
                        DiceRoller diceRoller) :
        base(configuration, actorPool, collisionService, contentRepository, diceRoller)
    {
        _randomGenerator = randomGenerator;
        _potionFactory = potionFactory;
    }

    protected override void OnCreate(Enemy enemy)
    {
        var stats = new Stats
        {
            MaxHealth = 3,
            Speed = Configuration.BaseCreatureSpeed * 0.95f,
            UnarmedAttack = new AttackAction(enemy, DiceRoller) { DiceRoll = new DiceRoll("1d3-1") }
        };
        var potion = _potionFactory.Add(Vector2.Zero, "Enemy Potion");
        potion.IsPickedUp = true;
        enemy.Inventory.Add(potion);

        enemy.Alignement = new Alignement
        {
            Group = 0b010,
            Hostile = 0b001,
            Friendly = 0b010
        };

        var bounds = new CircleF
        {
            Radius = Configuration.BaseCreatureAreaOfInfluenceRadius * 0.95f
        };
        enemy.AreaOfInfluence = new Collision(enemy, bounds)
        {
            IsSensor = true
        };
        CollisionService.Insert(enemy.AreaOfInfluence);

        enemy.Name = "Enemy";
        enemy.Stats = stats;
        enemy.Sprite = new ActorSprite(enemy, ContentRepository.Enemy, Configuration.EnemyColor.ToColor());
        enemy.Hero = ActorPool.Hero;
        enemy.AvailableActions.Add(new DeathTrigger(), new DieAction(enemy, ActorPool, CollisionService, _randomGenerator));
        enemy.AvailableActions.Add(new AutoTrigger(), new UsePotionAction(enemy));
        enemy.AvailableActions.Add(new HeroVisibleTrigger(), new AttackSelectionAction(enemy));
        enemy.AvailableActions.Add(new HeroNotVisibleTrigger(), new ToggleDoorAction(enemy));
        enemy.AvailableActions.Add(new AutoTrigger(), new ChaseHeroAction(enemy, ActorPool.Hero));
    }
}