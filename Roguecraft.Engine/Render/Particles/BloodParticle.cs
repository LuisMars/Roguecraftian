using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render.Particles;

public class BloodParticle : ParticleEffect
{
    public BloodParticle(Configuration configuration, ContentRepository contentRepository)
    {
        var color = configuration.BloodColor.ToColor(0.5f);
        var bloodTextureRegion = contentRepository.Particle;

        Emitters = new List<ParticleEmitter>
            {
                new ParticleEmitter(bloodTextureRegion, 15000, TimeSpan.FromSeconds(2),
                                    Profile.Circle(50, Profile.CircleRadiation.Out))
                {
                    AutoTrigger = false,
                    Parameters = new ParticleReleaseParameters
                    {
                        Color = color.ToHsl(),
                        Speed = new Range<float>(100, 200),
                        Quantity = 2,
                        Rotation = new Range<float>(-1, 1),
                        Scale = new Range<float>(0.5f, 1)
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