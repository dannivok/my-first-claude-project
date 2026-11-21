namespace TextDungeon
{
    /// <summary>
    /// Egy ajtó: a mögötte lévő szoba típusa és a kiírt jel.
    /// </summary>
    internal class Door
    {
        public RoomType RoomType { get; }
        public string Hint { get; }

        public Door(RoomType roomType, string hint)
        {
            RoomType = roomType;
            Hint = hint;
        }
    }
}
