namespace Roguecraft.Engine.Timers;

public class ActionTimer
{
    public float Elapsed { get; private set; }
    public bool IsActive => Elapsed < TotalTime;
    public bool JustTriggered { get; private set; }
    public float TotalTime { get; private set; }

    public void Update(float deltaTime)
    {
        JustTriggered = false;
        Elapsed += deltaTime;
    }

    internal void Reset(float totalTime = 0.125f)
    {
        JustTriggered = true;
        TotalTime = totalTime;
        Elapsed = 0;
    }
}