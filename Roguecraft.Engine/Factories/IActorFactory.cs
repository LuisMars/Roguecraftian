using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Factories
{
    public interface IActorFactory
    {
        void Add(Vector2 position, string? name = null);

        void Add(Vector2 position, Vector2 size, string? name = null);
    }
}