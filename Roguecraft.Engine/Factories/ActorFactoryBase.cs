using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public abstract class ActorFactoryBase<TActor> : IActorFactory where TActor : Actor
{
    public ActorFactoryBase(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository)
    {
        ActorPool = actorPool;
        CollisionService = collisionService;
        ContentRepository = contentRepository;
        Configuration = configuration;
    }

    protected ActorPool ActorPool { get; }
    protected CollisionService CollisionService { get; }
    protected Configuration Configuration { get; }
    protected ContentRepository ContentRepository { get; }
    protected Vector2 Size { get; set; }

    public void Add(Vector2 position, Vector2 size, string? name = null)
    {
        Size = size;
        var actor = Create(position, name);
        if (actor.Collision.Bounds is CircleF circle)
        {
            actor.Position += new Vector2(circle.Radius);
        }
        ActorPool.Add(actor);
        if (actor is Hero hero)
        {
            ActorPool.Hero = hero;
        }
    }

    public void Add(Vector2 position, string? name = null)
    {
        Add(position, Vector2.One, name);
    }

    protected abstract TActor Create(Vector2 position, string? name = null);
}