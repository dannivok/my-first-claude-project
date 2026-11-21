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

        public static Monster Create(int floor, Random random)
        {
            string name = Texts.MonsterNames[random.Next(Texts.MonsterNames.Length)];
            return new Monster(name, BaseHp(floor), BaseMinDamage(floor), BaseMaxDamage(floor));
        }
    }
}
