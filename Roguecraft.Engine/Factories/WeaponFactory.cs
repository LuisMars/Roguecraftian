using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

internal class WeaponFactory : PickupItemFactory<Weapon>
{
    private readonly RandomGenerator _randomGenerator;

    public WeaponFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository, RandomGenerator randomGenerator)
            : base(configuration, actorPool, collisionService, contentRepository)
    {
        _randomGenerator = randomGenerator;
    }

    protected override void OnCreate(Weapon weapon)
    {
        weapon.Texture = ContentRepository.Dagger;
        weapon.Color = Configuration.SteelColor.ToColor();
        weapon.AttackAction = new AttackAction(_randomGenerator) { MinDamage = 1, MaxDamage = 3 };
    }
}