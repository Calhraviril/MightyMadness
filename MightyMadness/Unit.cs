namespace MightyMadness
{
    public class Unit
    {
        #region Data
        private int hp;
        private int mana;
        private int defence;
        private string name;

        public List<Skill> skills;
        public int speed;

        public string Name { get { return name; } }

        public float curHp;
        float curMana;        
        public Unit(string name, int health, int mana, int defence, int speed, List<Skill> skills)
        {
            this.name = name;
            this.hp = health;
            this.defence = defence;
            this.mana = mana;
            this.speed = speed;

            this.curHp = this.hp;
            this.curMana = this.mana;

            this.skills = skills;
            this.speed = speed;
        }
        #endregion

        // Reduces health by received damage by the correct amounts
        public bool Receive(int received)
        {
            if (received > 0) { curHp = curHp - (received - defence); }
            else { curHp = curHp + -received - defence; }
            if (curHp <= 0) return true;
            return false;
        }
        // Reduces mana by received cost by the correct amount
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
        // Returns a percentage taken from HP / MaxHP
        public float LifePercentage()
        {
            return curHp / hp * (float)100;
        }
    }
}
