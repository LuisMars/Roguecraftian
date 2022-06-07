using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Actions.Interaction;
using Roguecraft.Engine.Actions.Movement;
using Roguecraft.Engine.Actions.Triggers;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class HeroFactory : CreatureFactory<Hero>
{
    public HeroFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository, RandomGenerator randomGenerator) :
        base(configuration, actorPool, collisionService, contentRepository, randomGenerator)
    {
    }

    protected override void OnCreate(Hero hero)
    {
        var stats = new Stats
        {
            MaxHealth = 20,
            Speed = Configuration.BaseCreatureSpeed,
            UnarmedAttack = new AttackAction(hero, RandomGenerator) { MinDamage = 1 }
        };
        hero.Name = "Hero";
        hero.Texture = ContentRepository.Creature;
        hero.Stats = stats;
        hero.Color = Configuration.PlayerColor.ToColor();

        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.E } }, new PickupItemAction(hero));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.Space } }, new ConsumeItemAction(hero));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.Space } }, new ToggleDoorAction(hero));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.Space } }, new AttackSelectionAction(hero));

        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.W, Keys.A }, Except = new() { Keys.S, Keys.D } }, new MoveDirectionAction(hero, new Vector2(-1, -1)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.W, Keys.D }, Except = new() { Keys.S, Keys.A } }, new MoveDirectionAction(hero, new Vector2(1, -1)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.S, Keys.A }, Except = new() { Keys.W, Keys.D } }, new MoveDirectionAction(hero, new Vector2(-1, 1)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.S, Keys.D }, Except = new() { Keys.W, Keys.A } }, new MoveDirectionAction(hero, new Vector2(1, 1)));

        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.W }, Except = new() { Keys.A, Keys.S, Keys.D } }, new MoveDirectionAction(hero, new Vector2(0, -1)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.S }, Except = new() { Keys.W, Keys.A, Keys.D } }, new MoveDirectionAction(hero, new Vector2(0, 1)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.A }, Except = new() { Keys.W, Keys.S, Keys.D } }, new MoveDirectionAction(hero, new Vector2(-1, 0)));
        hero.AvailableActions.Add(new KeyPressedActionTrigger { Keys = new() { Keys.D }, Except = new() { Keys.W, Keys.A, Keys.S } }, new MoveDirectionAction(hero, new Vector2(1, 0)));
    }
}