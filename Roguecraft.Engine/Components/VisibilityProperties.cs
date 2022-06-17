using Microsoft.Xna.Framework;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Components;

public class VisibilityProperties
{
    public bool CanBeDrawn => TimesSeen > 2;
    public bool IsDectectedAsVisible { get; internal set; }
    public bool IsVisibleByHero { get; set; }
    public int TimesSeen { get; set; }

    public void Calculate(Vector2 position, Collision collision, VisibilityService visibilityService)
    {
        var isVisible = IsDectectedAsVisible ||
               (collision.IsTransparent && visibilityService.IsVisible(position, collision.Bounds));
        IsDectectedAsVisible = false;
        IsVisibleByHero = isVisible;
        if (collision.IsFixed && IsVisibleByHero)
        {
            TimesSeen++;
        }
    }
}