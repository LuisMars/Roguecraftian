using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Core;

namespace Roguecraft.Engine.Render;

public class TextureRenderer
{
    private readonly ActorPool _actorPool;

    public TextureRenderer(ActorPool actorPool)
    {
        _actorPool = actorPool;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var actor in _actorPool.Actors.Where(a => a.Texture is not null))
        {
            var color = actor.Color;
            if (!actor.Visibility.IsVisibleByHero && !actor.Visibility.CanBeDrawn)
            {
                continue;
            }
            spriteBatch.Draw(actor.Texture,
                             actor.Position,
                             color,
                             actor.Angle,
                             actor.Origin,
                             actor.Scale,
                             SpriteEffects.None,
                             GetLayer(actor.Position));
        }
    }

    private float GetLayer(Vector2 position)
    {
        return 0.5f + position.Y / 100000f + position.X / 10000000000f;
    }
}