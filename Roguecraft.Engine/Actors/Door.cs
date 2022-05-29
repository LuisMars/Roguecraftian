using MonoGame.Extended.TextureAtlases;

namespace Roguecraft.Engine.Actors
{
    public class Door : Actor
    {
        public bool IsOpen { get; private set; }
        public TextureRegion2D ToggledTexture { get; set; }

        internal void Toggle()
        {
            IsOpen = !IsOpen;
            Collision.IsSensor = IsOpen;
            var texture = Texture;
            Texture = ToggledTexture;
            ToggledTexture = texture;
        }
    }
}