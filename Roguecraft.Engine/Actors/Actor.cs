using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Timers;
using Roguecraft.Engine.Visibility;
using System.Diagnostics;

namespace Roguecraft.Engine.Actors;

[DebuggerDisplay("{Name}: {Position}")]
public abstract class Actor
{
    public float Angle { get; set; }
    public Collision Collision { get; set; }
    public bool IsPickedUp { get; protected set; }
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public ActorSprite Sprite { get; set; }
    public TimerManager Timers { get; } = new TimerManager();
    public VisibilityProperties Visibility { get; set; } = new();

    public void AfterUpdate(float deltaTime) => UpdateTimers(deltaTime);

    public virtual void CalculateVisibility(VisibilityService visibilityService) => Visibility.Calculate(Position, Collision, visibilityService);

    public virtual void ClearSimulationData() => Collision?.Clear();

    public virtual GameAction? TakeTurn(float deltaTime) => null;

    public virtual void UpdateSimulationData() => Collision?.Update();

    private void UpdateTimers(float deltaTime) => Timers.Update(deltaTime);
}