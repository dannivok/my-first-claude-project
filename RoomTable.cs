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
            new RoomInfo(RoomType.Monster, 45, Texts.MonsterRooms, Texts.MonsterHints),
            new RoomInfo(RoomType.Gold, 20, Texts.GoldRooms, Texts.GoldHints),
            new RoomInfo(RoomType.Empty, 20, Texts.EmptyRooms, Texts.EmptyHints),
            new RoomInfo(RoomType.Potion, 15, Texts.PotionRooms, Texts.PotionHints),
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

        private static RoomInfo Get(RoomType type)
        {
            return rooms.First(r => r.Type == type);
        }
    }
}
