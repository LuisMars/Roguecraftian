using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Components;

public abstract class Item
{
    public Action? Action { get; init; }
    public Attack? Melee { get; init; }
    public Attack? Ranged { get; init; }
}