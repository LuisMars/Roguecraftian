using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Core;

namespace Roguecraft.Engine.Helpers
{
    public static class ColorExtensions
    {
        public static Color GetCreatureColor(this Configuration configuration, Creature creature)
        {
            return (creature switch
            {
                Hero => configuration.PlayerColor,
                Enemy => configuration.EnemyColor,
                _ => configuration.EnemyColor
            }).ToColor();
        }

        public static Color ToColor(this string hex, float alpha = 1)
        {
            return new Color(ColorHelper.FromHex(hex), alpha);
        }
    }
}