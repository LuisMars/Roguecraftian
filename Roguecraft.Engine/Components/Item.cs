using Roguecraft.Engine.Actions.Combat;

namespace Roguecraft.Engine.Components;

public abstract class Item
{
    public Action? Action { get; init; }
    public AttackAction? Melee { get; init; }
    public AttackAction? Ranged { get; init; }
}