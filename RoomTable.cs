namespace TextDungeon
{
    /// <summary>
    /// Egy szobatípus adatai: esély (súly), leírások és ajtójelek.
    /// </summary>
    internal class RoomInfo
    {
        public RoomType Type { get; }
        public int Chance { get; }
        public string[] Descriptions { get; }
        public string[] Hints { get; }

        public RoomInfo(RoomType type, int chance, string[] descriptions, string[] hints)
        {
            Type = type;
            Chance = chance;
            Descriptions = descriptions;
            Hints = hints;
        }
    }

    /// <summary>
    /// A szobatípusok táblázata: sorsolás, leírás és ajtójel mind innen jön.
    /// </summary>
    internal static class RoomTable
    {
        private static readonly RoomInfo[] rooms =
        {
            new RoomInfo(RoomType.Monster, 40, Texts.MonsterRooms, Texts.MonsterHints),
            new RoomInfo(RoomType.Gold, 18, Texts.GoldRooms, Texts.GoldHints),
            new RoomInfo(RoomType.Empty, 15, Texts.EmptyRooms, Texts.EmptyHints),
            new RoomInfo(RoomType.Potion, 12, Texts.PotionRooms, Texts.PotionHints),
            new RoomInfo(RoomType.Trap, 8, Texts.TrapRooms, Texts.TrapHints),
            new RoomInfo(RoomType.Fountain, 7, Texts.FountainRooms, Texts.FountainHints),
            // A boss szobát nem sorsoljuk (0 esély): minden 5. floor utolsó szobája.
            new RoomInfo(RoomType.Boss, 0, Texts.BossRooms, Texts.BossHints),
        };

        /// <summary>Szobatípust sorsol az esélyek szerint.</summary>
        public static RoomType RollType(Random random)
        {
            int total = rooms.Sum(r => r.Chance);
            int roll = random.Next(total);
            foreach (RoomInfo room in rooms)
            {
                if (roll < room.Chance)
                {
                    return room.Type;
                }
                roll -= room.Chance;
            }
            return rooms[0].Type;
        }

        public static string RandomDescription(RoomType type, Random random)
        {
            string[] texts = Get(type).Descriptions;
            return texts[random.Next(texts.Length)];
        }

        public static string RandomHint(RoomType type, Random random)
        {
            string[] texts = Get(type).Hints;
            return texts[random.Next(texts.Length)];
        }

        /// <summary>
        /// Létrehoz egy ajtót: 75%-ban a valódi típus jelét kapja, különben egy másik típusét.
        /// </summary>
        public static Door CreateDoor(Random random)
        {
            RoomType type = RollType(random);
            RoomType hintType = type;
            if (random.Next(100) >= 75)
            {
                RoomType[] others = rooms
                    .Where(r => r.Type != type && r.Chance > 0)
                    .Select(r => r.Type)
                    .ToArray();
                hintType = others[random.Next(others.Length)];
            }
            return new Door(type, RandomHint(hintType, random));
        }

        private static RoomInfo Get(RoomType type)
        {
            return rooms.First(r => r.Type == type);
        }
    }
}
