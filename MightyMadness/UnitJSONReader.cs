using Newtonsoft.Json;

namespace MightyMadness
{
    public class UnitJSONReader
    {
        public List<JsonUnit> Units { get; set; }
    }

    public class JsonUnit
    {
        public string Name { get; set; }
        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public int Damage { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Defence { get; set; }
    }
}
