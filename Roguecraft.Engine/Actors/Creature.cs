using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public abstract class Creature : Actor
{
    public Collision AreaOfInfluence { get; set; }

    public AvailableActions AvailableActions { get; set; }

    public Vector2 Direction { get; set; }

    public float DistanceWalked { get; set; }

    public float Energy { get; set; }

    public Item EquipedItem { get; set; }

    public Vector2 FootstepDistance { get; set; }

    public int Health { get; set; }

    public Vector2 LastPosition { get; set; }

    public Vector2 RealSpeed { get; set; }
    public Vector2 Speed => Position - LastPosition;

    public Stats Stats { get; set; }

    public override void ClearSimulationData()
    {
        base.ClearSimulationData();
        AreaOfInfluence.Clear();
    }

    public void Die()
    {
        IsPickedUp = true;
        Timers[TimerType.Death].Reset();
    }

    public abstract GameAction? OnTakeTurn(float deltaTime);

    public override GameAction? TakeTurn(float deltaTime)
    {
        LastPosition = Position;
        Energy += Stats.Speed * deltaTime;
        Energy = Math.Min(0, Energy);
        return OnTakeTurn(deltaTime);
    }

    public override void UpdateSimulationData()
    {
        base.UpdateSimulationData();
        AreaOfInfluence.Update();
    }
}