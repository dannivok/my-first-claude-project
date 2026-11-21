namespace TextDungeon
{
    /// <summary>
    /// Kész szövegek a játékhoz. Szabadon használhatod, bővítheted és átírhatod őket.
    /// Véletlen elem egy tömbből: Texts.EmptyRooms[random.Next(Texts.EmptyRooms.Length)]
    /// </summary>
    internal static class Texts
    {
        public static readonly string Title = @"
 _____         _     ____
|_   _|____  _| |_  |  _ \ _   _ _ __   __ _  ___  ___  _ __
  | |/ _ \ \/ / __| | | | | | | | '_ \ / _` |/ _ \/ _ \| '_ \
  | |  __/>  <| |_  | |_| | |_| | | | | (_| |  __/ (_) | | | |
  |_|\___/_/\_\\__| |____/ \__,_|_| |_|\__, |\___|\___/|_| |_|
                                       |___/";

        // ---- Szobaleírások (K3) ----

        public static readonly string[] MonsterRooms =
        {
            "Nyirkos folyosóra érsz. A falakon moha nő.",
            "Egy beomlott terembe lépsz, a sarokban csontok hevernek.",
            "Hideg, kőpadlós kamrába érsz. Valami mozog az árnyékban.",
            "Egy régi őrszobába lépsz, a falon rozsdás fegyverek lógnak.",
            "Fáklyák pislákolnak egy szűk teremben. Nem vagy egyedül.",
        };

        public static readonly string[] GoldRooms =
        {
            "Egy kincseskamrába érsz, a padlón érmék csillognak.",
            "Egy elhagyott tábor maradványai között egy erszényt találsz.",
            "Egy felborult láda mellett gold szóródott szét.",
            "Egy kalandor csontváza még most is egy zacskó gold-ot szorongat.",
        };

        public static readonly string[] EmptyRooms =
        {
            "Üres terembe érsz. Csak a csepegő víz hangja hallatszik.",
            "Poros könyvtárszoba, a polcok rég kiürültek.",
            "Egy keskeny lépcsőfordulóba érsz. Semmi különös.",
            "Egy kerek terem, a közepén kialudt tűzrakás.",
            "Pókhálós raktárhelyiség, üres hordókkal.",
        };

        // ---- Ajtójelek (K4) ----

        public static readonly string[] MonsterHints =
        {
            "Morgás hallatszik mögüle.",
            "Karmolásnyomok vannak az ajtón.",
            "Rothadásszag árad a résekből.",
        };

        public static readonly string[] GoldHints =
        {
            "Csillogás szűrődik ki alatta.",
            "Érmék csörgését hallod.",
            "Az ajtót díszes vasveretek borítják.",
        };

        public static readonly string[] EmptyHints =
        {
            "Teljes csend.",
            "Hideg huzat jön az ajtó alól.",
            "Por és pókháló borítja az ajtót.",
        };

        // ---- Monsternevek (K5) ----

        public static readonly string[] MonsterNames =
        {
            "Goblin", "Kobold", "Rat", "Bat", "Zombie", "Spider", "Ghoul", "Bandit",
        };

        // ---- Potion szoba és ajtójel (K3, K4) ----

        public static readonly string[] PotionRooms =
        {
            "Egy alkimista műhelyébe érsz. Az asztalon egy ép üvegcse áll.",
            "Egy kis kápolnában egy potion-t találsz az oltáron.",
            "Egy kidőlt polc alatt egy piros folyadékkal teli üvegcse lapul.",
        };

        public static readonly string[] PotionHints =
        {
            "Édeskés illat árad mögüle.",
            "Halvány piros fény dereng a résekben.",
        };

        // ---- Csapdák és források (A1) ----

        public static readonly string[] TrapRooms =
        {
            "Egy kő megbillen a lábad alatt, és nyilak röppennek ki a falból!",
            "A padló beszakad alattad, és egy tüskés verembe zuhansz!",
            "Mérges gáz szivárog elő a falak réseiből!",
        };

        public static readonly string[] TrapHints =
        {
            "Kattogó hang hallatszik mögüle.",
            "Furcsa karcolások vannak az ajtó előtt a padlón.",
            "Fémes szag árad mögüle.",
        };

        public static readonly string[] FountainRooms =
        {
            "Egy tiszta vizű forráshoz érsz. Iszol belőle, és erőre kapsz.",
            "Egy ősi szökőkút csobog a terem közepén. A vize gyógyít.",
            "Egy kristálytiszta patak folyik át a termen.",
        };

        public static readonly string[] FountainHints =
        {
            "Vízcsobogás hallatszik.",
            "Nedves az ajtó küszöbe.",
            "Friss, tiszta levegő áramlik ki alatta.",
        };
    }
}
