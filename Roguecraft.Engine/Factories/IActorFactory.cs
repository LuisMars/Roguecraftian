using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Factories
{
    public interface IActorFactory
    {
        void Add(float x, float y, string? name = null);
        void Add(Vector2 position, string? name = null);
    }
}