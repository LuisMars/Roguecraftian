namespace Roguecraft.Engine.Components;

public class VisibilityProperties
{
    public bool CanBeDrawn => TimesSeen > 2;
    public bool IsVisibleByPlayer { get; set; }
    public int TimesSeen { get; set; }
}