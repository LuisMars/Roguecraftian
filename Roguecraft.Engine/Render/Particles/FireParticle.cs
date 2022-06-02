using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render.Particles;

public class FireParticle : ParticleEffect
{
    public FireParticle(Configuration configuration, ContentRepository contentRepository)
    {
        var color = configuration.FireColor.ToColor();
        var color2 = configuration.FlameColor.ToColor();
        var fireTextureRegion = contentRepository.Particle;
        Emitters = new List<ParticleEmitter>
            {
                new ParticleEmitter(fireTextureRegion, 15000, TimeSpan.FromSeconds(1),
                                    Profile.Circle(16, Profile.CircleRadiation.Out)
                                    )
                {
                    AutoTrigger = false,
                    Parameters = new ParticleReleaseParameters
                    {
                        Color = new Range<HslColor>(color.ToHsl(), color2.ToHsl()),
                        Speed = new Range<float>(10, 100),
                        Quantity = 15,
                        Rotation = new Range<float>(-1, 1),
                        Scale = new Range<float>(0.75f, 2)
                    },
                    Modifiers =
                    {
                        new RotationModifier { RotationRate = .1f },
                        new DragModifier
                        {
                            Density = 1f, DragCoefficient = 1f
                        },
                        new LinearGravityModifier
                        {
                            Direction = new Vector2(0,-1),
                            Strength = 500f
                        },
                        new OpacityFastFadeModifier(),
                        new AgeModifier
                        {
                            Interpolators =
                            {
                                new ColorInterpolator
                                {
                                    StartValue = color.ToHsl(),
                                    EndValue = color2.ToHsl()
                                }
                            }
                        }
                    }
                }
            };
    }
}