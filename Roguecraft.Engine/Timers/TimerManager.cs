namespace Roguecraft.Engine.Timers;

public class TimerManager
{
    private readonly Dictionary<TimerType, ActionTimer> _timers = new();

    public TimerManager()
    {
        foreach (var type in Enum.GetValues(typeof(TimerType)).Cast<TimerType>())
        {
            _timers.Add(type, new ActionTimer());
        }
    }

    public IEnumerable<TimerType> ActiveTypes => _timers.Where(t => t.Value.IsActive).Select(t => t.Key);
    public IEnumerable<TimerType> JustTriggeredTypes => _timers.Where(t => t.Value.JustTriggered).Select(t => t.Key);
    public ActionTimer this[TimerType timerType] => _timers[timerType];

    internal void Update(float deltaTime)
    {
        foreach (var timer in _timers.Values)
        {
            timer.Update(deltaTime);
        }
    }
}