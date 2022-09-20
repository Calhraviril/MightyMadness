using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using MightyMadness;
using System.Data;
using System.Runtime.InteropServices;

namespace MightyMadness
{
    public class Battlecontrol
    {
        UnitJSONReader allyHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\units.json"));
        UnitJSONReader foeHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\foes.json"));
        Random random = new();
        Writer writer = new();
        public List<Unit> allies = new();
        public List<Unit> enemies = new();
        public List<Unit> battleOrder = new();
        #region "Army creation"
        // Creates both armies
        public void ManifestArmy(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(allyHandler.Units.Count);
                var call = allyHandler.Units[chosen].Stats;
                allies.Add(new Unit(allyHandler.Units[chosen].Name, call.Health, call.Mana, call.Defence, call.Speed, Skills(chosen, allyHandler)));
            }
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(foeHandler.Units.Count);
                var call = foeHandler.Units[chosen].Stats;
                enemies.Add(new Unit(foeHandler.Units[chosen].Name, call.Health, call.Mana, call.Defence, call.Speed, Skills(chosen, foeHandler)));
            }
            SetBattleOrder();
        }
        // Set order of attackers
        private void SetBattleOrder()
        {
            List<Unit> packList = new();
            packList.AddRange(allies);
            packList.AddRange(enemies);
            battleOrder = packList.OrderBy(o => o.Speed).ToList();
        }
        // Conversion from JsonSkill to Skill
        public List<Skill> Skills(int chosen, UnitJSONReader file)
        {
            List<Skill> list = new();

            foreach (JsonSkill skill in file.Units[chosen].Skills)
            {
                list.Add(new Skill(skill.Name, skill.Damage, skill.ManaCost, skill.Speciality));
            }

            return list;
        }
        #endregion

        // Lists the current armies
        public void ListArmies()
        {
            writer.WriteColored(ConsoleColor.Yellow, "- - - Armies List - - -", 0);
            Console.WriteLine("");
            Console.Write("Your units are: ");
            foreach (Unit unit in allies)
            {
                writer.WriteColored(ConsoleColor.White, unit.Name + " (" + unit.LifePercentage() + "%) ", 0, ":", ConsoleColor.White, writer.WriteList((object[])unit.skills.ToArray()), 0, " / ");
            }
            Console.WriteLine("");
            Console.Write("Your enemies are: ");
            foreach (Unit unit in enemies)
            {
                writer.WriteColored(ConsoleColor.Gray, unit.Name + " (" + unit.LifePercentage() + "%) ", 0, ":", ConsoleColor.White, writer.WriteList((object[])unit.skills.ToArray()), 0, " / ");
            }
            Console.WriteLine("");
        }

        #region "Combat turns"
        public void PlayerScenario(Unit unit, int target, int skillIndex)
        {
            try
            {
                Skill chosenSkill = unit.skills[skillIndex];
                SetHistory(unit.Name + " attacked " + enemies[target].Name + " with " + chosenSkill.Name + ", dealing " + chosenSkill.Damage + " damage");
                Attack(unit, enemies[target], chosenSkill);
                writer.Flash(ConsoleColor.DarkRed, 250);
            }
            catch (IndexOutOfRangeException)
            {
               Console.WriteLine("Selected skill not found");
            }
        }
        public void FoeScenario(Unit unit)
        {
            int ran1 = random.Next(0, allies.Count); // Random chosen from allies
            int ran2 = random.Next(0, unit.skills.Count); // Random chosen from enemy skills
            SetHistory(unit.Name + " attacked " + allies[ran1].Name + " with " + unit.skills[ran2].Name + ", dealing " + unit.skills[ran2].Damage + " damage");
            Attack(unit, allies[ran1], unit.skills[ran2]);
            writer.Flash(ConsoleColor.DarkRed, 50);
        }
        #endregion

        // Damagedealer
        public void Attack(Unit attacker, Unit attacked, Skill skillChosen)
        {
            Skill choseSkill = attacker.skills[0];
            try
            {
                choseSkill = skillChosen;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Skill Index out of range, defaulting to basic attack");
            }
            if (attacker.UseSkill(choseSkill))
            {
                switch (choseSkill.Speciality)
                {
                    case 0: // Single Target
                        if (attacked.Receive(choseSkill.Damage)) 
                        {
                            try { enemies.Remove(attacked); }
                            catch { allies.Remove(attacked); }
                        }
                        break;
                    case 1: // Area Attack
                        if (enemies.Contains(attacked))
                        {
                            for (int i = enemies.Count - 1; i > -1; i--)
                            {
                                if (enemies[i].Receive(choseSkill.Damage)) { enemies.Remove(enemies[i]); }
                            }
                        }
                        else if (allies.Contains(attacked))
                        {
                            for (int i = allies.Count - 1; i > -1; i--)
                            {
                                if (allies[i].Receive(choseSkill.Damage)) { allies.Remove(allies[i]); }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Something has gone wrong with using the skill");
                        break;
                }
            }
        }

        #region "Battle history"
        private List<string> history = new();
        public void SetHistory(string historize)
        {
            history.Add(historize);
        }
        public void ReadHistory()
        {
            writer.WriteColored(ConsoleColor.Yellow, "- - - Battle Results - - -", 0, 1);
            foreach (string segment in history)
            {
                Console.WriteLine(segment);
            }
        }
        public void DeleteHistory()
        {
            history.Clear();
        }
        #endregion
    }
}
