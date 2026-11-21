namespace TextDungeon
{
    /// <summary>
    /// A monstertípusok közös absztrakt őse. A különleges képességet a típusok
    /// az Attack vagy a ReduceDamage felülírásával kapják meg.
    /// </summary>
    internal abstract class Monster
    {
        public string Name { get; }
        public int Hp { get; private set; }
        public int MaxHp { get; }
        public int MinDamage { get; }
        public int MaxDamage { get; }
        public int XpReward { get; }

        /// <summary>A különleges képesség rövid leírása.</summary>
        public abstract string AbilityDescription { get; }

        public bool IsAlive => Hp > 0;

        protected Monster(string name, int maxHp, int minDamage, int maxDamage, int xpReward)
        {
            Name = name;
            MaxHp = Math.Max(1, maxHp);
            Hp = MaxHp;
            MinDamage = Math.Max(1, minDamage);
            MaxDamage = Math.Max(MinDamage, maxDamage);
            XpReward = xpReward;
        }

        /// <summary>Sebzést kap, a HP 0 és max HP között marad. Visszaadja a tényleges sebzést.</summary>
        public int TakeDamage(int amount)
        {
            amount = ReduceDamage(amount);
            int before = Hp;
            Hp = Math.Clamp(Hp - amount, 0, MaxHp);
            return before - Hp;
        }

        /// <summary>Gyógyul, legfeljebb max HP-ig. Visszaadja a tényleges gyógyulást.</summary>
        protected int Heal(int amount)
        {
            int before = Hp;
            Hp = Math.Clamp(Hp + amount, 0, MaxHp);
            return Hp - before;
        }

        /// <summary>A kapott sebzés módosítása (alapból nincs).</summary>
        protected virtual int ReduceDamage(int amount)
        {
            return amount;
        }

        /// <summary>Megtámadja a játékost, és leírja, mi történt.</summary>
        public virtual string Attack(Player target, Random random)
        {
            return Strike(target, random, out _);
        }

        /// <summary>Egy sima ütés. A lost-ba kerül, mennyi HP-t vesztett a játékos.</summary>
        protected string Strike(Player target, Random random, out int lost)
        {
            int damage = random.Next(MinDamage, MaxDamage + 1);
            lost = target.TakeDamage(damage);
            return $"{Name} megüt: -{lost} HP.";
        }
    }

    /// <summary>25% eséllyel kétszer üt egy körben.</summary>
    internal class Goblin : Monster
    {
        public Goblin(string name, int hp, int min, int max, int xp)
            : base($"{name} (Goblin)", hp, min, max, xp) { }

        public override string AbilityDescription => "25% eséllyel kétszer üt egy körben.";

        public override string Attack(Player target, Random random)
        {
            string text = Strike(target, random, out _);
            if (target.IsAlive && random.Next(100) < 25)
            {
                text += " Gyors, mint a villám, újra üt! " + Strike(target, random, out _);
            }
            return text;
        }
    }

    /// <summary>Minden kapott sebzésből 1-gyel kevesebbet veszít, de legalább 1-et.</summary>
    internal class Skeleton : Monster
    {
        public Skeleton(string name, int hp, int min, int max, int xp)
            : base($"{name} (Csontváz)", hp + 2, min, max, xp) { }

        public override string AbilityDescription => "Minden kapott sebzésből 1-gyel kevesebbet veszít (legalább 1-et).";

        protected override int ReduceDamage(int amount)
        {
            return Math.Max(1, amount - 1);
        }
    }

    /// <summary>Az általa okozott sebzés felét visszagyógyítja.</summary>
    internal class Vampire : Monster
    {
        public Vampire(string name, int hp, int min, int max, int xp)
            : base($"{name} (Vámpír)", hp - 1, min, max, xp) { }

        public override string AbilityDescription => "Az általa okozott sebzés felét visszagyógyítja.";

        public override string Attack(Player target, Random random)
        {
            string text = Strike(target, random, out int lost);
            int healed = Heal(lost / 2);
            if (healed > 0)
            {
                text += $" Vért szív: +{healed} HP.";
            }
            return text;
        }
    }

    /// <summary>Erős, de 30% eséllyel mellé üt.</summary>
    internal class Orc : Monster
    {
        public Orc(string name, int hp, int min, int max, int xp)
            : base($"{name} (Ork)", hp + hp / 2, min + 1, max + 2, xp + 2) { }

        public override string AbilityDescription => "Erős, de 30% eséllyel mellé üt.";

        public override string Attack(Player target, Random random)
        {
            if (random.Next(100) < 30)
            {
                return $"{Name} nagyot suhint, de mellé üt!";
            }
            return Strike(target, random, out _);
        }
    }
}
