namespace Roguecraft.Engine.Core
{
    public class Configuration
    {
        public string BackgroundColor { get; } = "#192136";
        public float BaseCreatureAreaOfInfluenceRadius { get; } = 90;
        public float BaseCreatureRadius { get; } = 45;
        public float BaseCreatureSpeed { get; } = 500;
        public string BloodColor { get; } = "#781B27";
        public string DeadColor { get; } = "#A74749";
        public string EnemyColor { get; } = "#6AA572";
        public string FireColor { get; } = "E25039";
        public string FlameColor { get; } = "EBA839";
        public string FootstepColor { get; } = "22325a";
        public string PlayerColor { get; } = "#CCB7A4";
        public string PotionColor { get; } = "#5362b6";
        public int RoomsPerDungeon { get; } = 30;
        public string SplashColor { get; } = "#3CA29C";
        public int StepDrawOffset { get; } = 6;
        public int StepFrequencyBase { get; } = 200;
        public string WallColor { get; } = "#476C7B";
        public int WallSize { get; } = 100;
        public string WoodColor { get; } = "66523D";
    }
}