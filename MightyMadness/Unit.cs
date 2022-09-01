using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyMadness
{
    public class Unit
    {
        private int damage;
        private int hp;
        private int mana;
        private int defence;
        private string name;

        private int curHealth;
        private int curMana;

        public Unit(string name, int damage, int health, int mana, int defence)
        {
            this.name = name;
            this.hp = health;
            this.damage = damage;
            this.defence = defence;
            this.mana = mana;

            this.curHealth = this.hp;
            this.curMana = this.mana;
        }
        public void Receive(int received)
        {
            curHealth = curHealth - (received - defence);
        }
        public void Attack(Unit target)
        {
            target.Receive(damage);
        }
        public bool Dead()
        {
            if (curHealth <= 0) return true;
            return false;
        }
        public string Namer()
        {
            return name;
        }
    }
}
