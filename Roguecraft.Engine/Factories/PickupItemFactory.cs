using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public abstract class PickupItemFactory<TItem> : ActorFactoryBase<TItem> where TItem : Item, new()
{
    public PickupItemFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository)
        : base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override TItem Create(Vector2 position, string? name = null)
    {
        var item = new TItem
        {
            Position = position,
            Name = name ?? "Item"
        };
        OnCreate(item);
        item.Collision = new Collision
        {
            IsTransparent = true,
            IsSensor = true,
            Actor = item,
            Bounds = new CircleF
            {
                Radius = Configuration.BaseCreatureRadius
            }
        };

        CollisionService.Insert(item.Collision);
        return item;
    }

    protected abstract void OnCreate(TItem creature);
}