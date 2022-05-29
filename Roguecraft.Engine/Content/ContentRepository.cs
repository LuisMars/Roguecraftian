using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

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
        public TextureRegion2D Wall { get; private set; }

        private void Load()
        {
            Creature = LoadTextureRegion("person");
            Wall = LoadTextureRegion("wall");
        }

        private TextureRegion2D LoadTextureRegion(string name)
        {
            var creatureTexture = _content.Load<Texture2D>(name);
            var creature = new TextureRegion2D(creatureTexture);
            return creature;
        }
    }
}