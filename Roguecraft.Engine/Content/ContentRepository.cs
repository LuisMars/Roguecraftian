using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Sound;

namespace Roguecraft.Engine.Content
{
    public class ContentRepository
    {
        private readonly ContentManager _content;

        public ContentRepository(ContentManager content)
        {
            _content = content;
            Load();
        }

        public TextureRegion2D Barrel { get; private set; }
        public TextureRegion2D Bookshelf { get; private set; }
        public TextureRegion2D Chair { get; private set; }
        public TextureRegion2D Chest { get; private set; }
        public TextureRegion2D Creature { get; private set; }
        public TextureRegion2D Dagger { get; private set; }
        public GameSound DaggerDraw { get; private set; }
        public TextureRegion2D Dead { get; private set; }
        public GameSound DeathSound { get; private set; }
        public TextureRegion2D Decoration { get; private set; }
        public TextureRegion2D Door { get; private set; }
        public GameSound DoorCloseSound { get; private set; }
        public GameSound DoorOpenSound { get; private set; }
        public TextureRegion2D Enemy { get; private set; }
        public TextureRegion2D EnergyBack { get; private set; }
        public TextureRegion2D EnergyBar { get; private set; }
        public TextureRegion2D EnergyFront { get; private set; }
        public GameSound FireSound { get; private set; }
        public TextureRegion2D Fist { get; private set; }
        public SpriteFont Font { get; private set; }
        public TextureRegion2D Footstep { get; private set; }
        public GameSound HealSound { get; private set; }
        public GameSound HitSound { get; private set; }
        public TextureRegion2D InventoryFrame { get; private set; }
        public TextureRegion2D InventorySelector { get; private set; }
        public GameSound InventorySound { get; private set; }
        public TextureRegion2D Line { get; private set; }
        public TextureRegion2D Particle { get; private set; }
        public TextureRegion2D Potion { get; private set; }
        public TextureRegion2D ProgressBack { get; private set; }
        public TextureRegion2D ProgressFrame { get; private set; }
        public TextureRegion2D ProgressFront { get; private set; }
        public TextureRegion2D Ritual { get; private set; }
        public GameSound SlowDownSound { get; private set; }
        public GameSound SpeedUpSound { get; private set; }
        public TextureRegion2D Table { get; private set; }
        public TextureRegion2D Wall { get; private set; }

        private void Load()
        {
            Creature = LoadTextureRegion("person");
            Enemy = LoadTextureRegion("enemy");
            Wall = LoadTextureRegion("wall");
            Decoration = LoadTextureRegion("decoration");
            Chair = LoadTextureRegion("chair");
            Ritual = LoadTextureRegion("pentagram");
            Bookshelf = LoadTextureRegion("bookshelf");
            Barrel = LoadTextureRegion("barrel");
            Chest = LoadTextureRegion("chest");
            Table = LoadTextureRegion("table");

            Door = LoadTextureRegion("door");
            Line = LoadTextureRegion("line");
            Potion = LoadTextureRegion("potion");
            Particle = LoadTextureRegion("particle");
            Dead = LoadTextureRegion("dead");
            Footstep = LoadTextureRegion("footstep");

            Dagger = LoadTextureRegion("dagger");
            Fist = LoadTextureRegion("fist");

            ProgressBack = LoadTextureRegion("progress-back");
            ProgressFront = LoadTextureRegion("progress-front");
            ProgressFrame = LoadTextureRegion("progress-frame");

            InventoryFrame = LoadTextureRegion("inventory");
            InventorySelector = LoadTextureRegion("inventory_selector");

            EnergyBack = LoadTextureRegion("energy-back");
            EnergyFront = LoadTextureRegion("energy-front");
            EnergyBar = LoadTextureRegion("energy-bar");

            Font = _content.Load<SpriteFont>("font");
            HitSound = new GameSound(
                    _content.Load<SoundEffect>("hit_01"),
                    _content.Load<SoundEffect>("hit_02"),
                    _content.Load<SoundEffect>("hit_03")
                );

            DeathSound = new GameSound(
                     _content.Load<SoundEffect>("hit_04"),
                     _content.Load<SoundEffect>("hit_05")
                 );
            FireSound = new GameSound(_content.Load<SoundEffect>("fire"));
            HealSound = new GameSound(_content.Load<SoundEffect>("heal"));

            DaggerDraw = new GameSound(_content.Load<SoundEffect>("dagger_draw"));

            SlowDownSound = new GameSound(_content.Load<SoundEffect>("timewarp_01"));
            SpeedUpSound = new GameSound(_content.Load<SoundEffect>("timewarp_02"));

            InventorySound = new GameSound(
                _content.Load<SoundEffect>("inventory_01"),
                _content.Load<SoundEffect>("inventory_02"),
                _content.Load<SoundEffect>("inventory_03"),
                _content.Load<SoundEffect>("inventory_04"),
                _content.Load<SoundEffect>("inventory_05"));

            DoorOpenSound = new GameSound(_content.Load<SoundEffect>("door_01"));
            DoorCloseSound = new GameSound(_content.Load<SoundEffect>("door_02"));
        }

        private TextureRegion2D LoadTextureRegion(string name)
        {
            var creatureTexture = _content.Load<Texture2D>(name);
            var creature = new TextureRegion2D(creatureTexture);
            return creature;
        }
    }
}