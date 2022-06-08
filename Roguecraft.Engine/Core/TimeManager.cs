using Roguecraft.Engine.Content;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Sound;

namespace Roguecraft.Engine.Core;

public class TimeManager
{
    private readonly ContentRepository _contentRepository;
    private readonly InputManager _inputManager;
    private readonly GameSound _slowDownSound;
    private readonly SoundService _soundService;
    private readonly GameSound _speedUpSound;

    public TimeManager(InputManager inputManager, SoundService soundService, ContentRepository contentRepository)
    {
        _inputManager = inputManager;
        _soundService = soundService;
        _contentRepository = contentRepository;
        _slowDownSound = _contentRepository.SlowDownSound;
        _speedUpSound = _contentRepository.SpeedUpSound;
    }

    public float DeltaTime { get; private set; }
    private bool IsSlowingDown { get; set; }
    private bool IsSpeedingUp { get; set; }
    private bool JustStartedSlowingDown { get; set; }
    private bool JustStartedSpeedingUp { get; set; }

    public float GetDeltaTime(float deltaTime)
    {
        var originalDelta = deltaTime;
        if (_inputManager.State.IsButtonDown(InputAction.SlowMotion))
        {
            deltaTime = Math.Max(0.0005f, DeltaTime * 0.95f);
            JustStartedSlowingDown = !IsSlowingDown;
            IsSlowingDown = true;
        }
        else if (DeltaTime != 0 && DeltaTime < deltaTime)
        {
            JustStartedSpeedingUp = !IsSpeedingUp;
            IsSpeedingUp = true;
            deltaTime = Math.Min(deltaTime, DeltaTime * 1.2f);
        }
        if (originalDelta == deltaTime)
        {
            IsSpeedingUp = false;
            IsSlowingDown = false;
            JustStartedSpeedingUp = false;
            JustStartedSlowingDown = false;
        }
        if (JustStartedSlowingDown)
        {
            _soundService.Play(_slowDownSound);
        }
        if (JustStartedSpeedingUp)
        {
            _soundService.Play(_speedUpSound);
        }
        DeltaTime = deltaTime;
        return deltaTime;
    }
}