namespace MightyMadness
{
    public class Unit
    {
        // The actual stored data
        private int hp;
        private int mana;
        private int defence;
        private int speed;
        private string name;

        float curHealth;
        float curMana;

        public List<Skill> skills;
        // Data
        public Unit(string name, int health, int mana, int defence, int speed, List<Skill> skills)
        {
            this.name = name;
            this.hp = health;
            this.defence = defence;
            this.mana = mana;
            this.speed = speed;

            this.curHealth = this.hp;
            this.curMana = this.mana;

            this.skills = skills;
            this.speed = speed;
        }
        // Combat specifics
        public bool Receive(int received)
        {
            curHealth = curHealth - (received - defence);
            if (curHealth <= 0) return true;
            return false;
        }

        // Information Conversions
        public string Name { get { return name; } }
        public int Speed { get { return speed; } }
        public bool UseSkill(Skill skill)
        {
            if (skill.ManaCost <= curMana)
            {
                curMana = curMana - skill.ManaCost;
                return true;
            }
            Console.WriteLine("Failed to cast " + skill.Name);
            return false;
        }
        public float LifePercentage()
        {
            return curHealth / hp * (float)100;
        }
    }
}
