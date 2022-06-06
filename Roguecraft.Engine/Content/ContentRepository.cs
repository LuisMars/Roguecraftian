﻿using Microsoft.Xna.Framework.Audio;
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

        public TextureRegion2D Creature { get; private set; }
        public TextureRegion2D Dead { get; private set; }
        public TextureRegion2D Door { get; private set; }
        public GameSound DoorCloseSound { get; private set; }
        public GameSound DoorOpenSound { get; private set; }
        public TextureRegion2D Enemy { get; private set; }
        public SpriteFont Font { get; private set; }
        public TextureRegion2D Footstep { get; private set; }
        public GameSound HitSound { get; private set; }
        public TextureRegion2D Line { get; private set; }
        public TextureRegion2D Particle { get; private set; }
        public TextureRegion2D Wall { get; private set; }

        private void Load()
        {
            Creature = LoadTextureRegion("person");
            Enemy = LoadTextureRegion("enemy");
            Wall = LoadTextureRegion("wall");
            Door = LoadTextureRegion("door");
            Line = LoadTextureRegion("line");
            Particle = LoadTextureRegion("particle");
            Dead = LoadTextureRegion("particle");
            Footstep = LoadTextureRegion("footstep");
            Font = _content.Load<SpriteFont>("font");
            HitSound = new GameSound(
                    _content.Load<SoundEffect>("hit_01"),
                    _content.Load<SoundEffect>("hit_02"),
                    _content.Load<SoundEffect>("hit_03")
                );
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