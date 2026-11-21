namespace TextDungeon
{
    internal enum Difficulty
    {
        Easy,
        Normal,
        Hard,
    }

    /// <summary>
    /// Minden difficulty-függő érték egyetlen helyen dől el. A játék többi része
    /// csak ezeket a beállított értékeket használja.
    /// </summary>
    internal class DifficultySettings
    {
        public Difficulty Difficulty { get; }
        public int HpBonus { get; }
        public int MonsterDamageModifier { get; }
        public int StartGold { get; }
        public int FleeChance { get; }
        public double ScoreMultiplier { get; }

        private DifficultySettings(Difficulty difficulty, int hpBonus, int monsterDamageModifier,
            int startGold, int fleeChance, double scoreMultiplier)
        {
            Difficulty = difficulty;
            HpBonus = hpBonus;
            MonsterDamageModifier = monsterDamageModifier;
            StartGold = startGold;
            FleeChance = fleeChance;
            ScoreMultiplier = scoreMultiplier;
        }

        public static DifficultySettings For(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    return new DifficultySettings(difficulty, 10, -1, 20, 70, 0.5);
                case Difficulty.Hard:
                    return new DifficultySettings(difficulty, -5, 1, 0, 30, 1.5);
                default:
                    return new DifficultySettings(Difficulty.Normal, 0, 0, 0, 50, 1.0);
            }
        }
    }
}
