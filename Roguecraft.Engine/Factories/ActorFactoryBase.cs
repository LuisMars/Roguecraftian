using Microsoft.Xna.Framework;
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

    public void Add(float x, float y, string? name = null)
    {
        Add(new Vector2(x, y), name);
    }

    public void Add(Vector2 position, string? name = null)
    {
        var actor = Create(position, name);
        ActorPool.Add(actor);
        if (actor is Hero hero)
        {
            ActorPool.Hero = hero;
        }
    }

    protected abstract TActor Create(Vector2 position, string? name = null);
}