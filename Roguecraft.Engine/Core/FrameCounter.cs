namespace Roguecraft.Engine.Core;

public class FrameCounter
{
    public const int MAXIMUM_SAMPLES = 100;

    private readonly Queue<float> _sampleBuffer = new();

    public FrameCounter()
    {
    }

    public float AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }
    public int SlowFrames => _sampleBuffer.Count(x => x < 55);
    public float Slowness => 1f * SlowFrames / _sampleBuffer.Count;
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    private DateTime LastTime { get; set; } = DateTime.Now;

    public bool IsSlow()
    {
        return SlowFrames > 0;
    }

    public bool Update()
    {
        var deltaTime = (float)(DateTime.Now - LastTime).TotalSeconds;
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
        LastTime = DateTime.Now;

        return true;
    }
}