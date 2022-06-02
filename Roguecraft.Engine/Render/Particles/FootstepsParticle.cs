using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Profiles;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Render.Particles;

public class FootstepsParticle : ParticleEffect
{
    public FootstepsParticle(Configuration configuration, ContentRepository contentRepository)
    {
        var color = configuration.FootstepColor.ToColor();
        var footstepTextureRegion = contentRepository.Footstep;

        Emitters = new List<ParticleEmitter>
        {
            new ParticleEmitter(footstepTextureRegion, 1500, TimeSpan.FromSeconds(5),
                                Profile.Point()
                                )
            {
                AutoTrigger = false,
                Parameters = new ParticleReleaseParameters
                {
                    Color = color.ToHsl(),
                    Speed = new Range<float>(50, 75),
                    Scale = new Range<float>(1.8f, 2.2f)
                },
                Modifiers =
                {
                    new DragModifier
                    {
                        Density = 1f, DragCoefficient = 10f
                    },
                    new OpacityFastFadeModifier()
                }
            }
        };
    }
}