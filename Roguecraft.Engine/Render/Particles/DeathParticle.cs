using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render.Particles;

internal class DeathParticle : ParticleEffect
{
    public DeathParticle(Configuration configuration, ContentRepository contentRepository)
    {
        var color = configuration.DeadColor.ToColor();

        var bodyTextureRegion = contentRepository.Dead;
        Emitters = new List<ParticleEmitter>
            {
                new ParticleEmitter(bodyTextureRegion, 15000, TimeSpan.FromSeconds(10),
                                    Profile.Circle(100, Profile.CircleRadiation.Out))
                {
                    AutoTrigger = false,
                    Parameters = new ParticleReleaseParameters
                    {
                        Color = color.ToHsl(),
                        Speed = new Range<float>(50, 200),
                        Quantity = 4,
                        Rotation = new Range<float>(-1, 1),
                        Scale = new Range<float>(2, 3)
                    },
                    Modifiers =
                    {
                        new RotationModifier { RotationRate = .1f },
                        new DragModifier
                        {
                            Density = 2f, DragCoefficient = 2f
                        },
                        new OpacityFastFadeModifier()
                    }
                }
            };
    }
}