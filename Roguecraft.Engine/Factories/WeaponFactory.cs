using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Random.Dice;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class WeaponFactory : PickupItemFactory<Weapon>
{
    private readonly DiceRoller _diceRoller;

    public WeaponFactory(Configuration configuration,
                         ActorPool actorPool,
                         CollisionService collisionService,
                         ContentRepository contentRepository,
                         DiceRoller diceRoller)
            : base(configuration, actorPool, collisionService, contentRepository)
    {
        _diceRoller = diceRoller;
    }

    protected override void OnCreate(Weapon weapon)
    {
        weapon.AttackAction = new AttackAction(_diceRoller) { DiceRoll = new DiceRoll("1d4") };
        weapon.Sprite = new ActorSprite(weapon, ContentRepository.Dagger, Configuration.SteelColor.ToColor());
    }
}