using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Sound;

public class SoundService
{
    private readonly ActorPool _actorPool;
    private readonly ContentRepository _contentRepository;
    private readonly RandomGenerator _random;
    private readonly Dictionary<TimerType, GameSound> _sounds = new();

    public SoundService(RandomGenerator random, ActorPool actorPool, ContentRepository contentRepository)
    {
        _random = random;
        _actorPool = actorPool;
        _contentRepository = contentRepository;
        _sounds = new Dictionary<TimerType, GameSound>
        {
            { TimerType.Death, _contentRepository.DeathSound },
            { TimerType.Hurt, _contentRepository.HitSound },
            { TimerType.Fire, _contentRepository.FireSound },
            { TimerType.Heal, _contentRepository.HealSound },
            { TimerType.Pickup, _contentRepository.InventorySound },
            { TimerType.DrawDagger, _contentRepository.DaggerDraw },
            { TimerType.DoorOpen, _contentRepository.DoorOpenSound },
            { TimerType.DoorClose, _contentRepository.DoorCloseSound }
        };
    }

    public void Play(GameSound sound, Vector2 origin)
    {
        var listener = _actorPool.Hero.Position;
        var distSqrt = (origin - listener).LengthSquared();
        var volume = 1f;
        if (distSqrt != 0)
        {
            volume = MathF.Min(1, 50000 / distSqrt);
        }
        var pan = Math.Clamp((origin.X - listener.X) / 10000f, -1, 1);
        sound.PlayRandom(_random, volume, pan);
    }

    public void Play(GameSound sound) => sound.Play();

    public void Play()
    {
        foreach (var actor in _actorPool.Actors)
        {
            PlayActor(actor);
        }
    }

    private void PlayActor(Actor actor)
    {
        foreach (var type in actor.Timers.JustTriggeredTypes)
        {
            if (!_sounds.TryGetValue(type, out var sound))
            {
                continue;
            }
            Play(sound, actor.Position);
        }
    }

    private void PlayDoor(Actor actor)
    {
        if (actor is not Door door)
        {
            return;
        }
        if (door.JustOpened)
        {
            Play(_contentRepository.DoorOpenSound, actor.Position);
        }
        if (door.JustClosed)
        {
            Play(_contentRepository.DoorCloseSound, actor.Position);
        }
    }
}