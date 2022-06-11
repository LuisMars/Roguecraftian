using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Render.Particles;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Render;

public class ParticleRenderer
{
    private readonly ActorPool _actorPool;

    private readonly ParticleEffect _bloodPaticle;
    private readonly ParticleEffect _deathParticle;
    private readonly ParticleEffect _fireParticle;
    private readonly ParticleEffect _footstepParticle;
    private readonly ParticleEffect _healParticle;
    private readonly Dictionary<TimerType, ParticleEffect> _particles = new();
    private readonly float _stepDrawOffset;
    private readonly float _stepFrequencyBaseSquared;

    public ParticleRenderer(Configuration configuration, ContentRepository contentRepository, ActorPool actorPool)
    {
        _actorPool = actorPool;
        _particles = new Dictionary<TimerType, ParticleEffect>
        {
            { TimerType.Death, new DeathParticle(configuration, contentRepository) },
            { TimerType.Hurt, new BloodParticle(configuration, contentRepository) },
            { TimerType.Fire, new FireParticle(configuration, contentRepository) },
            { TimerType.Heal, new HealParticle(configuration, contentRepository) }
        };

        _footstepParticle = new FootstepsParticle(configuration, contentRepository);

        var diameter = configuration.BaseCreatureRadius * 2;
        _stepFrequencyBaseSquared = diameter * diameter;
        _stepDrawOffset = configuration.BaseCreatureRadius * 0.75f;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var particle in _particles.Values)
        {
            spriteBatch.Draw(particle);
        }
        spriteBatch.Draw(_footstepParticle);
    }

    public void Update(float deltaTime)
    {
        foreach (var creature in _actorPool.Actors.Where(a => a is Creature).Cast<Creature>())
        {
            if (!creature.Visibility.IsVisibleByHero && !creature.Visibility.CanBeDrawn)
            {
                continue;
            }
            foreach (var type in creature.Timers.ActiveTypes)
            {
                if (!_particles.TryGetValue(type, out var particle))
                {
                    continue;
                }
                particle.Trigger(creature.Position, 0.01f);
            }
            TriggerFootsteps(creature);
        }

        foreach (var particle in _particles.Values)
        {
            particle.Update(deltaTime);
        }
        _footstepParticle.Update(deltaTime);
    }

    private void TriggerFootsteps(Creature creature)
    {
        creature.FootstepDistance += creature.Position - creature.LastPosition;

        var lengthSquared = creature.FootstepDistance.LengthSquared();
        if (lengthSquared <= _stepFrequencyBaseSquared)
        {
            return;
        }
        var direction = creature.Direction;
        creature.FootstepDistance = new Vector2();
        var rotation = direction.ToAngle() + MathF.PI * 0.5f;
        _footstepParticle.Emitters[0].Parameters.Rotation = new(rotation - 0.2f, rotation + 0.2f);

        var stepOffset = direction;
        stepOffset.Normalize();
        stepOffset = stepOffset.Rotate(MathF.PI * 0.25f);
        stepOffset *= _stepDrawOffset;
        _footstepParticle.Trigger(creature.Position + stepOffset, 0.01f);
        _footstepParticle.Trigger(creature.Position - stepOffset, 0.01f);
    }
}