using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Render.Particles;

namespace Roguecraft.Engine.Render;

public class ParticleRenderer
{
    private readonly ActorPool _actorPool;

    private readonly ParticleEffect _bloodPaticle;
    private readonly ParticleEffect _deathParticle;
    private readonly ParticleEffect _fireParticle;
    private readonly ParticleEffect _footstepParticle;
    private readonly ParticleEffect _healParticle;
    private readonly int _stepDrawOffset;
    private readonly int _stepFrequencyBaseSquared;

    public ParticleRenderer(Configuration configuration, ContentRepository contentRepository, ActorPool actorPool)
    {
        _actorPool = actorPool;
        _bloodPaticle = new BloodParticle(configuration, contentRepository);
        _deathParticle = new DeathParticle(configuration, contentRepository);
        _fireParticle = new FireParticle(configuration, contentRepository);
        _healParticle = new HealParticle(configuration, contentRepository);
        _footstepParticle = new FootstepsParticle(configuration, contentRepository);
        _stepFrequencyBaseSquared = configuration.StepFrequencyBase * configuration.StepFrequencyBase;
        _stepDrawOffset = configuration.StepFrequencyBase / configuration.StepDrawOffset;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_bloodPaticle);
        spriteBatch.Draw(_footstepParticle);
    }

    public void Update(float deltaTime)
    {
        foreach (var creature in _actorPool.Actors.Where(a => a is Creature).Cast<Creature>())
        {
            if (!creature.Visibility.IsVisible && !creature.Visibility.CanBeDrawn)
            {
                continue;
            }
            TriggerBlood(creature);
            TriggerFootsteps(creature);
        }
        _bloodPaticle.Update(deltaTime);
        _footstepParticle.Update(deltaTime);
    }

    private void TriggerBlood(Creature creature)
    {
        if (!creature.HurtTimer.IsActive)
        {
            return;
        }
        _bloodPaticle.Trigger(creature.Position, 0);
    }

    private void TriggerFootsteps(Creature creature)
    {
        var direction = creature.WalkAction.CurrentPosition - creature.WalkAction.LastPosition;
        var lengthSquared = direction.LengthSquared();
        if (lengthSquared <= _stepFrequencyBaseSquared - (creature.Stats.Speed * creature.Stats.Speed))
        {
            return;
        }
        creature.WalkAction.LastPosition = creature.WalkAction.CurrentPosition;
        var rotation = creature.WalkAction.Direction.ToAngle() + MathF.PI * 0.5f;
        _footstepParticle.Emitters[0].Parameters.Rotation = new(rotation - 0.2f, rotation + 0.2f);

        var stepOffset = direction;
        stepOffset.Normalize();
        stepOffset = stepOffset.Rotate(MathF.PI * 0.25f);
        stepOffset *= _stepDrawOffset;
        _footstepParticle.Trigger(creature.Position + stepOffset, 0);
        _footstepParticle.Trigger(creature.Position - stepOffset, 0);
    }
}