namespace TextDungeon
{
    /// <summary>
    /// Monstereket készít az aktuális floor erőssége szerint.
    /// </summary>
    internal static class MonsterFactory
    {
        // Floor-onként +3 HP és +1 max sebzés, minden második floor-on +1 min sebzés.
        public static int BaseHp(int floor) => 8 + 3 * (floor - 1);
        public static int BaseMinDamage(int floor) => 1 + (floor - 1) / 2;
        public static int BaseMaxDamage(int floor) => 3 + (floor - 1);

        public static Monster Create(int floor, Random random, DifficultySettings settings)
        {
            string name = Texts.MonsterNames[random.Next(Texts.MonsterNames.Length)];
            int min = Math.Max(1, BaseMinDamage(floor) + settings.MonsterDamageModifier);
            int max = Math.Max(min, BaseMaxDamage(floor) + settings.MonsterDamageModifier);
            return new Monster(name, BaseHp(floor), min, max, 3 + floor);
        }
    }
}
