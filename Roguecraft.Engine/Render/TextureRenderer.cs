using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Render;

public class TextureRenderer
{
    private readonly ActorPool _actorPool;
    private readonly TextureRegion2D _fist;

    public TextureRenderer(ActorPool actorPool, ContentRepository contentRepository)
    {
        _actorPool = actorPool;
        _fist = contentRepository.Fist;
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
            var texture = _fist;
            var itemColor = actor.Color;
            if (actor is not Creature creature)
            {
                continue;
            }
            if (creature.EquipedItem is not null)
            {
                color = creature.EquipedItem.Color;
                texture = creature.EquipedItem.Texture;
            }
            var angle = MathF.PI * 0.25f;
            var timer = creature.Timers[TimerType.Attack];
            if (timer.IsActive)
            {
                angle = MathF.PI;
                if (timer.Percentage < 0.25f)
                {
                    angle *= 2 * timer.Percentage + 0.25f;
                }
                else if (timer.Percentage < 0.75f)
                {
                    angle *= 0.75f - 1.5f * (timer.Percentage - 0.25f);
                }
                else
                {
                    angle *= timer.Percentage - 0.75f;
                }
            }
            spriteBatch.Draw(texture,
                             actor.Position,
                             color,
                             actor.Angle - angle,
                             actor.Origin + new Vector2(-texture.Width, 0),
                             creature.Scale * 0.75f,
                             SpriteEffects.FlipHorizontally,
                             GetLayer(actor.Position) * 1.001f);
        }
    }

    private float GetLayer(Vector2 position)
    {
        return 0.5f + position.Y / 100000f + position.X / 10000000000f;
    }
}