using Microsoft.Xna.Framework.Audio;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Sound;

public class GameSound
{
    private static readonly List<float> _scale = new()
    {
        1,
        1.12246f,
        1.25992f,
        1.33483f,
        1.49831f,
        1.68179f,
        1.88775f,
        2
    };

    private readonly List<SoundEffect> _sounds;

    public GameSound(params SoundEffect[] soundEffects)
    {
        _sounds = soundEffects.ToList();
    }

    public bool PlayRandom(float volume, float pan)
    {
        return _sounds.Choice().Play(volume, MathUtils.RandomNormal(), pan);
    }

    public bool PlayRandomNote(float volume, float pan)
    {
        return _sounds.Choice().Play(volume, _scale.Choice(), pan);
    }
}