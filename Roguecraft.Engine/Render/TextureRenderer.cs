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
        foreach (var actor in _actorPool.Actors.Where(a => a.Sprite.Texture is not null))
        {
            var color = actor.Sprite.Color;
            if (!actor.Visibility.IsVisibleByHero && !actor.Visibility.CanBeDrawn)
            {
                continue;
            }

            spriteBatch.Draw(actor.Sprite.Texture,
                             actor.Sprite.Position,
                             color,
                             actor.Sprite.Angle,
                             actor.Sprite.Origin,
                             actor.Sprite.Scale,
                             actor.Sprite.Effects,
                             GetLayer(actor.Position));
            DrawCreatureArms(spriteBatch, actor);
            DrawUnderPlayer(spriteBatch, actor);
        }
    }

    private static float GetLayer(Vector2 position, float offset = 0)
    {
        return 0.5f + (position.Y + offset * 100) / 100000f + position.X / 10000000000f;
    }

    private void DrawCreatureArms(SpriteBatch spriteBatch, Actor actor)
    {
        if (actor is not Creature creature)
        {
            return;
        }

        var color = creature.Sprite.Color;
        var texture = _fist;
        var armColor = color;
        if (creature.EquipedItem is not null)
        {
            armColor = creature.EquipedItem.Sprite.Color;
            texture = creature.EquipedItem.Sprite.Texture;
        }
        var rightArmAngle = MathF.PI * 0.25f;
        var leftArmAngle = MathF.PI * -0.25f;
        var attackTimer = creature.Timers[TimerType.Attack];
        var hurtTimer = creature.Timers[TimerType.Hurt];
        if (attackTimer.IsActive)
        {
            rightArmAngle = MathF.PI;
            if (attackTimer.Percentage < 0.25f)
            {
                rightArmAngle *= 2 * attackTimer.Percentage + 0.25f;
            }
            else if (attackTimer.Percentage < 0.75f)
            {
                rightArmAngle *= 0.75f - 1.5f * (attackTimer.Percentage - 0.25f);
            }
            else
            {
                rightArmAngle *= attackTimer.Percentage - 0.75f;
            }

            leftArmAngle = MathF.PI * -attackTimer.Percentage * 0.25f;
        }
        if (hurtTimer.IsActive)
        {
            var value = hurtTimer.Percentage * 0.25f;
            rightArmAngle = MathF.PI * value;
            leftArmAngle = MathF.PI * -value;
        }
        spriteBatch.Draw(texture,
                         creature.Sprite.Position,
                         armColor,
                         creature.Sprite.Angle - rightArmAngle,
                         creature.Sprite.Origin + new Vector2(-texture.Width, 0),
                         creature.Sprite.Scale * 0.75f,
                         SpriteEffects.FlipHorizontally,
                         GetLayer(actor.Position, 10f));
        spriteBatch.Draw(_fist,
                         creature.Sprite.Position,
                         color,
                         creature.Angle - leftArmAngle,
                         creature.Sprite.Origin + new Vector2(texture.Width, 0),
                         creature.Sprite.Scale * 0.75f,
                         SpriteEffects.None,
                         GetLayer(actor.Position, 10f));
    }

    private void DrawUnderPlayer(SpriteBatch spriteBatch, Actor actor)
    {
        if (actor is not Hero hero)
        {
            return;
        }
        spriteBatch.Draw(hero.UnderTexture,
                         hero.Sprite.Position + new Vector2(hero.AreaOfInfluence.Width * -(0.28125f)),
                         hero.UnderColor,
                         0,
                         hero.Sprite.Origin,
                         new Vector2(hero.AreaOfInfluence.Width / hero.UnderTexture.Width) * 1.125f,
                         hero.Sprite.Effects,
                         0);
    }
}