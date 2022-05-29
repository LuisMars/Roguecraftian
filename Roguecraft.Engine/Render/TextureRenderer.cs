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
            spriteBatch.Draw(actor.Texture,
                             actor.Position,
                             actor.Color,
                             actor.Angle,
                             actor.Origin,
                             actor.Scale,
                             SpriteEffects.None,
                             0);
        }
    }
}