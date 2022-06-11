using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Actions.Interaction;
using Roguecraft.Engine.Actions.Inventory;
using Roguecraft.Engine.Actions.Movement;
using Roguecraft.Engine.Actions.Triggers;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class HeroFactory : CreatureFactory<Hero>
{
    private readonly InputManager _inputManager;

    public HeroFactory(Configuration configuration,
                       ActorPool actorPool,
                       CollisionService collisionService,
                       ContentRepository contentRepository,
                       RandomGenerator randomGenerator,
                       InputManager inputManager) :
        base(configuration, actorPool, collisionService, contentRepository, randomGenerator)
    {
        _inputManager = inputManager;
    }

    protected override void OnCreate(Hero hero)
    {
        var stats = new Stats
        {
            MaxHealth = 20,
            Speed = Configuration.BaseCreatureSpeed,
            UnarmedAttack = new AttackAction(hero, RandomGenerator) { MinDamage = 1 }
        };
        hero.UnderTexture = ContentRepository.UnderPlayer;
        hero.UnderColor = Configuration.UnderPlayerColor.ToColor();
        hero.AreaOfInfluence = new Collision
        {
            Actor = hero,
            Bounds = new CircleF
            {
                Radius = Configuration.BaseCreatureAreaOfInfluenceRadius
            },
            IsSensor = true
        };
        CollisionService.Insert(hero.AreaOfInfluence);

        hero.Name = "Hero";
        hero.Texture = ContentRepository.Creature;
        hero.Stats = stats;
        hero.Color = Configuration.PlayerColor.ToColor();
        hero.InputManager = _inputManager;

        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.InventoryNext } }, new MoveInventoryAction(hero, true));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.InventoryPrev } }, new MoveInventoryAction(hero, false));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.InventoryUse } }, new UseInventoryAction(hero));

        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.QuickAction } }, new AttackSelectionAction(hero));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.PickUp } }, new PickupItemAction(hero));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.QuickAction } }, new QuickAction(hero));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.QuickAction } }, new ToggleDoorAction(hero));

        hero.AvailableActions.Add(new InputActionTrigger { LeftStick = true }, new JoystickMovementAction(hero, _inputManager));

        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveUp, InputAction.MoveLeft }, Except = new() { InputAction.MoveDown, InputAction.MoveRight } }, new MoveDirectionAction(hero, new Vector2(-1, -1)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveUp, InputAction.MoveRight }, Except = new() { InputAction.MoveDown, InputAction.MoveLeft } }, new MoveDirectionAction(hero, new Vector2(1, -1)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveDown, InputAction.MoveLeft }, Except = new() { InputAction.MoveUp, InputAction.MoveRight } }, new MoveDirectionAction(hero, new Vector2(-1, 1)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveDown, InputAction.MoveRight }, Except = new() { InputAction.MoveUp, InputAction.MoveLeft } }, new MoveDirectionAction(hero, new Vector2(1, 1)));

        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveUp }, Except = new() { InputAction.MoveLeft, InputAction.MoveDown, InputAction.MoveRight } }, new MoveDirectionAction(hero, new Vector2(0, -1)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveDown }, Except = new() { InputAction.MoveUp, InputAction.MoveLeft, InputAction.MoveRight } }, new MoveDirectionAction(hero, new Vector2(0, 1)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveLeft }, Except = new() { InputAction.MoveUp, InputAction.MoveDown, InputAction.MoveRight } }, new MoveDirectionAction(hero, new Vector2(-1, 0)));
        hero.AvailableActions.Add(new InputActionTrigger { Keys = new() { InputAction.MoveRight }, Except = new() { InputAction.MoveUp, InputAction.MoveLeft, InputAction.MoveDown } }, new MoveDirectionAction(hero, new Vector2(1, 0)));
    }
}