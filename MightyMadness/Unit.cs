namespace MightyMadness
{
    public class Unit
    {
        // The actual stored data
        private int hp;
        private int mana;
        private int defence;
        private string name;

        private int curHealth;
        private int curMana;

        public List<Skill> skills;
        
        // Data
        public Unit(string name, int health, int mana, int defence, List<Skill> skills)
        {
            this.name = name;
            this.hp = health;
            this.defence = defence;
            this.mana = mana;

            this.curHealth = this.hp;
            this.curMana = this.mana;

            this.skills = skills;
        }
        // Combat specifics
        public void Receive(int received)
        {
            curHealth = curHealth - (received - defence);
        }

        // Misc
        public bool Dead()
        {
            if (curHealth <= 0) return true;
            return false;
        }
        public string Namer()
        {
            return name;
        }
        public string SkillCall()
        {
            string returnable = "";
            foreach (Skill skill in skills)
            {
                returnable = returnable + skill.Name + ", ";
            }
            return returnable;
        }
        public bool UseSkill(Skill skill)
        {
            if (skill.ManaCost > curMana)
            {
                Console.WriteLine("Casted " + skill.Name + " for " + skill.ManaCost + ", leaving you with " + curMana);
                curMana = curMana - skill.ManaCost;
                return false;
            }
            Console.WriteLine("Failed to cast " + skill.Name);
            return true;
        }
    }
}
