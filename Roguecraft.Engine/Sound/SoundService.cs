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

    public void Update()
    {
        foreach (var actor in _actorPool.Actors)
        {
            if (actor is Creature creature)
            {
                if (creature.HurtTimer.JustTriggered)
                {
                    Play(_contentRepository.HitSound, actor.Position);
                }
            }

            if (actor is Door door)
            {
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
    }
}