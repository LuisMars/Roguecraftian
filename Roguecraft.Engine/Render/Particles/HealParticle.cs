using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render.Particles;

internal class HealParticle : ParticleEffect
{
    public HealParticle(Configuration configuration, ContentRepository contentRepository)
    {
        var color = configuration.SplashColor.ToColor();
        var color2 = configuration.PotionColor.ToColor();
        var healTextureRegion = contentRepository.Particle;

        Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(healTextureRegion, 15000, TimeSpan.FromSeconds(1.5f),
                                        Profile.Circle(75, Profile.CircleRadiation.Out))
                    {
                        AutoTrigger = false,
                        Parameters = new ParticleReleaseParameters
                        {
                            Color = new Range<HslColor>(color.ToHsl(), color2.ToHsl()),
                            Speed = new Range<float>(50, 150),
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