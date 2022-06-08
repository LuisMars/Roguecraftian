using Roguecraft.Engine.Actions.Effects;
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

    protected override void OnCreate(Potion potion)
    {
        potion.HealAction = new HealAction();
        potion.Texture = ContentRepository.Potion;
        potion.Color = Configuration.PotionColor.ToColor();
    }
}