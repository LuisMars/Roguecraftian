using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;

namespace Roguecraft.Engine.Render;

public class HudRenderer
{
    private readonly ActorPool _actorPool;
    private readonly Color _color = Color.White;
    private readonly ContentRepository _contentRepository;
    private readonly SpriteFont _font;
    private readonly FrameCounter _frameCounter;

    public HudRenderer(ActorPool actorPool, ContentRepository contentRepository, FrameCounter frameCounter)
    {
        _actorPool = actorPool;
        _contentRepository = contentRepository;
        _font = _contentRepository.Font;
        _frameCounter = frameCounter;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        var hero = _actorPool.Hero;
        var health = hero.Health;
        spriteBatch.DrawString(_font, $"HP: {health}", new(8, 8), _color);
        spriteBatch.DrawString(_font, $"FPS: {(int)_frameCounter.AverageFramesPerSecond}", new(8, 32), _color);
        spriteBatch.DrawString(_font, $"Slowness: {_frameCounter.Slowness:F2}", new(8, 64), _color);

        spriteBatch.End();
    }
}