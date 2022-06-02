using Microsoft.Xna.Framework;
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
                    new ParticleEmitter(healTextureRegion, 15000, TimeSpan.FromSeconds(0.75f),
                                        Profile.Spray(new Vector2(0, -1), 3f)
                                        )
                    {
                        AutoTrigger = false,
                        Parameters = new ParticleReleaseParameters
                        {
                            Color = new Range<HslColor>(color.ToHsl(), color2.ToHsl()),
                            Speed = new Range<float>(100, 200),
                            Quantity = 10,
                            Rotation = new Range<float>(-1, 1),
                            Scale = new Range<float>(0.5f, 1)
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
                                Direction = new Vector2(0,1),
                                Strength = 300f
                            },
                            new OpacityFastFadeModifier()
                        }
                    }
                };
    }
}