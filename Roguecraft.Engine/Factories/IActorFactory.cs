using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Factories
{
    public interface IActorFactory<TActor> where TActor : Actor
    {
        TActor Add(Vector2 position, string? name = null);

        TActor Add(Vector2 position, Vector2 size, string? name = null);
    }
}