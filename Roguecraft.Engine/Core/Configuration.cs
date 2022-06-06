namespace Roguecraft.Engine.Core
{
    public class Configuration
    {
        public string BackgroundColor { get; } = "#111111";
        public float BaseCreatureAreaOfInfluenceRadius { get; } = 90;
        public float BaseCreatureRadius { get; } = 45;
        public float BaseCreatureSpeed { get; } = 500;
        public string BloodColor { get; } = "#781B27";
        public string DeadColor { get; } = "#A74749";
        public string EnemyColor { get; } = "#5C7D6C";
        public string FireColor { get; } = "E25039";
        public string FlameColor { get; } = "EBA839";
        public string FootstepColor { get; } = "1D2225";
        public string PlayerColor { get; } = "#EFEAD7";
        public string PotionColor { get; } = "#5362b6";
        public int RoomsPerDungeon { get; } = 30;
        public string SplashColor { get; } = "#3CA29C";
        public int StepDrawOffset { get; } = 6;
        public int StepFrequencyBase { get; } = 200;
        public string WallColor { get; } = "#323230";
        public int WallSize { get; } = 100;
    }
}