namespace TextDungeon
{
    /// <summary>
    /// Monstereket készít az aktuális floor erőssége szerint.
    /// A monster típusa csak itt, a sorsoláskor számít.
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
            int hp = BaseHp(floor);
            int min = MinDamage(floor, settings);
            int max = MaxDamage(floor, settings, min);
            int xp = 3 + floor;

            switch (random.Next(4))
            {
                case 0:
                    return new Goblin(name, hp, min, max, xp);
                case 1:
                    return new Skeleton(name, hp, min, max, xp);
                case 2:
                    return new Vampire(name, hp, min, max, xp);
                default:
                    return new Orc(name, hp, min, max, xp);
            }
        }

        /// <summary>
        /// A floor legerősebb sima monstere az Ork (HP + HP/2), a boss HP-ja ennek a háromszorosa + 5.
        /// </summary>
        public static Monster CreateBoss(int floor, Random random, DifficultySettings settings)
        {
            string name = Texts.BossNames[random.Next(Texts.BossNames.Length)];
            int strongestHp = BaseHp(floor) + BaseHp(floor) / 2;
            int min = MinDamage(floor, settings);
            int max = MaxDamage(floor, settings, min);
            return new Boss(name, 3 * strongestHp + 5, min, max, 5 * (3 + floor));
        }

        private static int MinDamage(int floor, DifficultySettings settings)
        {
            return Math.Max(1, BaseMinDamage(floor) + settings.MonsterDamageModifier);
        }

        private static int MaxDamage(int floor, DifficultySettings settings, int min)
        {
            return Math.Max(min, BaseMaxDamage(floor) + settings.MonsterDamageModifier);
        }
    }
}
