using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;

namespace Roguecraft.Engine.Sound;

public class SoundService
{
    private readonly ActorPool _actorPool;
    private readonly ContentRepository _contentRepository;

    public SoundService(ActorPool actorPool, ContentRepository contentRepository)
    {
        _actorPool = actorPool;
        _contentRepository = contentRepository;
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
        sound.PlayRandom(volume, pan);
    }

    public void Play()
    {
        foreach (var actor in _actorPool.Actors)
        {
            PlayCreature(actor);
            PlayDoor(actor);
        }
    }

    private void PlayCreature(Actor actor)
    {
        if (actor is not Creature creature)
        {
            return;
        }

        if (!creature.HurtTimer.JustTriggered)
        {
            return;
        }
        Play(_contentRepository.HitSound, actor.Position);
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