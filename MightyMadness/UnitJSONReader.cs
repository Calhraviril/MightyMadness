using Newtonsoft.Json;

namespace MightyMadness
{
    public class UnitJSONReader
    {
        public List<JsonUnit> Units { get; set; }
    }
    // Character stuff
    public class JsonUnit
    {
        public string Name { get; set; }
        public Stats Stats { get; set; }
        public List<JsonSkill> Skills { get; set; }
    }

    public class Stats
    {
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Defence { get; set; }
    }
    
    //Skill Stuff
    public class JsonSkill
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int ManaCost { get; set; }
        public int Speciality { get; set; }
    }
}
