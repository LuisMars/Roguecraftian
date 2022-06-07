using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class PotionFactory : PickupItemFactory<Potion>
{
    public PotionFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository)
        : base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override void OnCreate(Potion creature)
    {
        creature.Texture = ContentRepository.Potion;
        creature.Color = Configuration.PotionColor.ToColor();
    }
}