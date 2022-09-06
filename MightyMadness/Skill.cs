namespace MightyMadness
{
    public class Skill
    {
        // The actual stored data
        private string name;
        private int damage;
        private int manaCost;
        private int speciality;

        // Data
        public Skill(string name, int damage, int manaCost, int speciality)
        {
            this.name = name;
            this.manaCost = manaCost;
            this.damage = damage;
            this.speciality = speciality;
        }

        public string Name { get { return name; } }
        public int Damage { get { return damage; } }
        public int Speciality { get { return speciality; } }
    }
}
