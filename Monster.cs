namespace TextDungeon
{
    internal class Monster
    {
        public string Name { get; }
        public int Hp { get; private set; }
        public int MaxHp { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }

        public bool IsAlive => Hp > 0;

        public Monster(string name, int maxHp, int minDamage, int maxDamage)
        {
            Name = name;
            MaxHp = maxHp;
            Hp = maxHp;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        /// <summary>Sebzést kap, a HP 0 és max HP között marad. Visszaadja a tényleges sebzést.</summary>
        public int TakeDamage(int amount)
        {
            int before = Hp;
            Hp = Math.Clamp(Hp - amount, 0, MaxHp);
            return before - Hp;
        }

        /// <summary>Megüti a játékost, és leírja, mi történt.</summary>
        public string Attack(Player target, Random random)
        {
            int damage = random.Next(MinDamage, MaxDamage + 1);
            int lost = target.TakeDamage(damage);
            return $"{Name} megüt: -{lost} HP.";
        }
    }
}
