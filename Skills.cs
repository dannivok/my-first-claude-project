namespace TextDungeon
{
    /// <summary>
    /// Minden kaszt skill-je ezt az interfészt valósítja meg. A harc csak ezen keresztül használja.
    /// </summary>
    internal interface ISkill
    {
        string Name { get; }
        string Description { get; }
        bool IsReady { get; }
        int RemainingCooldown { get; }

        /// <summary>Használja a skill-t, és leírja, mi történt.</summary>
        string Use(Player user, Monster target, Random random);

        /// <summary>Egy harci kör vége (a cooldown itt csökken).</summary>
        void EndRound();

        /// <summary>Harc elején: a skill újra használható.</summary>
        void Reset();
    }

    /// <summary>A cooldown közös kezelése: használat után 3 harci körig nem használható.</summary>
    internal abstract class CooldownSkill : ISkill
    {
        private const int Cooldown = 3;
        private bool usedThisRound;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public int RemainingCooldown { get; private set; }
        public bool IsReady => RemainingCooldown == 0;

        public string Use(Player user, Monster target, Random random)
        {
            RemainingCooldown = Cooldown;
            usedThisRound = true;
            return Activate(user, target, random);
        }

        protected abstract string Activate(Player user, Monster target, Random random);

        public void EndRound()
        {
            // A használat körében nem csökken: ha az 1. körben használtad, az 5.-ben jön újra.
            if (usedThisRound)
            {
                usedThisRound = false;
            }
            else if (RemainingCooldown > 0)
            {
                RemainingCooldown--;
            }
        }

        public void Reset()
        {
            RemainingCooldown = 0;
            usedThisRound = false;
        }
    }

    internal class PowerStrike : CooldownSkill
    {
        public override string Name => "Power Strike";
        public override string Description => "dupla sebzés";

        protected override string Activate(Player user, Monster target, Random random)
        {
            int dealt = target.TakeDamage(user.RollDamage(random) * 2);
            return $"Power Strike! {target.Name} -{dealt} HP.";
        }
    }

    internal class Fireball : CooldownSkill
    {
        public override string Name => "Fireball";
        public override string Description => "6-10 sebzés";

        protected override string Activate(Player user, Monster target, Random random)
        {
            int dealt = target.TakeDamage(random.Next(6, 11));
            return $"Fireball! A lángok elnyelik: {target.Name} -{dealt} HP.";
        }
    }

    internal class Steal : CooldownSkill
    {
        public override string Name => "Steal";
        public override string Description => "3 sebzés és 5-15 gold";

        protected override string Activate(Player user, Monster target, Random random)
        {
            int dealt = target.TakeDamage(3);
            int gold = random.Next(5, 16);
            user.AddGold(gold);
            return $"Steal! {target.Name} -{dealt} HP, és elcsensz {gold} gold-ot.";
        }
    }
}
