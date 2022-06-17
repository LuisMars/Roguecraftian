using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render;

public class HudRenderer
{
    private readonly ActorPool _actorPool;
    private readonly Color _blue;
    private readonly Color _color;
    private readonly Configuration _configuration;
    private readonly ContentRepository _contentRepository;
    private readonly Color _darkBlue;
    private readonly Color _darkRed;
    private readonly TextureRegion2D _energyBack;
    private readonly TextureRegion2D _energyBar;
    private readonly TextureRegion2D _energyFront;
    private readonly SpriteFont _font;
    private readonly FrameCounter _frameCounter;
    private readonly TextureRegion2D _inventory;
    private readonly TextureRegion2D _inventorySelector;
    private readonly Color _lightRed;
    private readonly TextureRegion2D _progressBack;
    private readonly TextureRegion2D _progressFrame;
    private readonly TextureRegion2D _progressFront;

    public HudRenderer(ActorPool actorPool, ContentRepository contentRepository, Configuration configuration, FrameCounter frameCounter)
    {
        _actorPool = actorPool;
        _contentRepository = contentRepository;
        _font = _contentRepository.Font;
        _frameCounter = frameCounter;
        _progressBack = _contentRepository.ProgressBack;
        _progressFront = _contentRepository.ProgressFront;
        _progressFrame = _contentRepository.ProgressFrame;

        _energyBack = _contentRepository.EnergyBack;
        _energyBar = _contentRepository.EnergyBar;
        _energyFront = _contentRepository.EnergyFront;

        _inventory = _contentRepository.InventoryFrame;
        _inventorySelector = _contentRepository.InventorySelector;

        _configuration = configuration;
        _color = _configuration.PlayerColor.ToColor();
        _lightRed = _configuration.DeadColor.ToColor();
        _darkRed = _configuration.BloodColor.ToColor();
        _blue = _configuration.PotionColor.ToColor();
        _darkBlue = Color.Lerp(_blue, Color.Black, 0.5f);
    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        var hero = _actorPool.Hero;
        var health = hero.Health;
        var energy = hero.Energy;
        var maxHealth = hero.Stats.MaxHealth;
        var minimumEnergy = hero.Stats.Speed * 2;
        var progressCenter = new Vector2((spriteBatch.GraphicsDevice.Viewport.Width - _progressBack.Width) / 2, 8);

        var clip = new Rectangle((int)progressCenter.X + 8, 0, _progressFront.Width, 1000);
        var healthPercent = 1 - (1f * health / maxHealth);
        var energyCenter = progressCenter + new Vector2(0, 64 + 8);
        var energyPercent = -Math.Max(-minimumEnergy, energy) / minimumEnergy;
        var colorProgress = energyPercent * 5;
        var barColor = Color.Lerp(_blue, _lightRed, colorProgress);
        var frameColor = Color.Lerp(_darkBlue, _darkRed, colorProgress);

        spriteBatch.Draw(_progressBack, progressCenter, _lightRed);
        spriteBatch.Draw(_progressFront, progressCenter - new Vector2(_progressFront.Width * healthPercent, 0), _lightRed, clip);
        spriteBatch.Draw(_progressFrame, progressCenter, _lightRed);

        spriteBatch.Draw(_energyBack, energyCenter, frameColor);
        spriteBatch.Draw(_energyBar, energyCenter - new Vector2(_energyBar.Width * energyPercent, 0), barColor, clip);
        spriteBatch.Draw(_energyFront, energyCenter, frameColor);

        var inventoryStart = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width - _inventory.Width - 8, 8);

        var equipedStart = new Vector2(inventoryStart.X - _inventorySelector.Width, inventoryStart.Y);
        spriteBatch.Draw(_inventorySelector, equipedStart, _color);
        if (hero.EquipedItem is not null)
        {
            var scale = new Vector2(64f / hero.EquipedItem.Sprite.Width);
            equipedStart += new Vector2(8);

            spriteBatch.Draw(hero.EquipedItem.Sprite.Texture,
                             equipedStart,
                             hero.EquipedItem.Sprite.Color,
                             0f,
                             Vector2.Zero,
                             scale,
                             SpriteEffects.None,
                             0f);
        }
        spriteBatch.Draw(_inventory, inventoryStart, _color);
        spriteBatch.Draw(_inventorySelector, inventoryStart + new Vector2(0, 64 * hero.Inventory.CurrentIndex), _color);

        inventoryStart += new Vector2(8);
        for (var i = 0; i < hero.Inventory.Items.Length; i++)
        {
            var item = hero.Inventory.Items[i];
            if (item is null)
            {
                inventoryStart += new Vector2(0, 64);
                continue;
            }
            var itemScale = new Vector2(64f / item.Sprite.Width);
            var color = item.Sprite.Color;
            if (i != hero.Inventory.CurrentIndex)
            {
                color = Color.Lerp(color, Color.Black, 0.5f);
            }
            spriteBatch.Draw(item.Sprite.Texture,
                             inventoryStart,
                             color,
                             0f,
                             Vector2.Zero,
                             itemScale,
                             SpriteEffects.None,
                             0f);

            inventoryStart += new Vector2(0, 64);
        }

        spriteBatch.DrawString(_font, $"HP: {health}", new(8, 8), _color);
        spriteBatch.DrawString(_font, $"FPS: {(int)_frameCounter.AverageFramesPerSecond}", new(8, 32), _color);
        spriteBatch.DrawString(_font, $"Slowness: {_frameCounter.Slowness:F2}", new(8, 64), _color);

        spriteBatch.End();
    }
}