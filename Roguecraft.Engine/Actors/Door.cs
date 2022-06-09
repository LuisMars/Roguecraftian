using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors
{
    public class Door : Actor
    {
        public bool IsOpen { get; private set; }
        public bool JustClosed => !IsOpen && WasOpen;
        public bool JustOpened => IsOpen && !WasOpen;
        public TextureRegion2D ToggledTexture { get; set; }
        private bool WasOpen { get; set; }

        public override GameAction? TakeTurn(float deltaTime)
        {
            WasOpen = IsOpen;
            return null;
        }

        internal void Toggle()
        {
            IsOpen = !IsOpen;
            Collision.IsSensor = IsOpen;
            var texture = Texture;
            Texture = ToggledTexture;
            ToggledTexture = texture;
            if (IsOpen)
            {
                Timers[TimerType.DoorOpen].Reset();
            }
            else
            {
                Timers[TimerType.DoorClose].Reset();
            }
        }
    }
}