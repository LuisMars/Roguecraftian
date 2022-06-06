namespace Roguecraft.Engine.Components;

public class VisibilityProperties
{
    public bool CanBeDrawn => TimesSeen > 2;
    public bool IsVisibleByHero { get; set; }
    public int TimesSeen { get; set; }
}