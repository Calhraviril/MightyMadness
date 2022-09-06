using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using MightyMadness;

namespace MightyMadness
{
    public class Battlecontrol
    {
        UnitJSONReader allyHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\units.json"));
        UnitJSONReader foeHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\foes.json"));
        Random random = new();
        Writer writer = new();
        public List<Unit> allies = new List<Unit>();
        public List<Unit> enemies = new List<Unit>();

        // Creates both armies
        public void ManifestArmy(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(allyHandler.Units.Count);
                var call = allyHandler.Units[chosen].Stats;
                allies.Add(new Unit(allyHandler.Units[chosen].Name, call.Health, call.Mana, call.Defence, Skills(chosen)));
                allies[i].SkillCall();
            }
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(foeHandler.Units.Count);
                var call = foeHandler.Units[chosen].Stats;
                enemies.Add(new Unit(foeHandler.Units[chosen].Name, call.Health, call.Mana, call.Defence, Skills(chosen)));
            }
        }
        // Conversion from JsonSkill to Skill
        public List<Skill> Skills(int chosen)
        {
            List<Skill> list = new();

            foreach(JsonSkill skill in allyHandler.Units[chosen].Skills)
            {
                list.Add(new Skill(skill.Name, skill.Damage, skill.ManaCost, skill.Speciality));
            }

            return list;
        }

        // Lists the current armies
        public void ListArmies()
        {
            Console.Write("Your units are: ");
            foreach (Unit unit in allies)
            {
                writer.WriteColored(ConsoleColor.White, unit.Namer(), null, ": ", ConsoleColor.White, unit.SkillCall(), null, "/ ");
            }
            Console.WriteLine("");
            Console.Write("Your enemies are: ");
            foreach (Unit unit in enemies)
            {
                writer.WriteColored(ConsoleColor.Gray, unit.Namer(), null, ": ");
            }
            Console.WriteLine("");
        }

        // The battle event
        public void BattleScenario(int attacker, int skill, int attacked)
        {
            // Making sure the attacker + attacked arent empty
            if(attacker > allies.Count || attacked > enemies.Count)
            {
                Console.WriteLine("Trying to use/attack a non-existent unit");
                return;
            }

            // Players attack
            Attack(allies[attacker], enemies[attacked], skill);
            Console.WriteLine(allies[attacker].Namer() + " attacked " + enemies[attacked].Namer());

            // Random values x 2
            int ran1 = random.Next(0, enemies.Count);
            int ran2 = random.Next(0, allies.Count);

            // Enemies attack
            Attack(allies[ran2], enemies[ran1], 0);
            Console.WriteLine(enemies[ran1].Namer() + " attacked " + allies[ran2].Namer());
        }

        // Damagedealer
        public void Attack(Unit attacked, Unit attacker, int skillChosen)
        {
            Skill choseSkill = attacker.skills[skillChosen];
            if (attacker.UseSkill(choseSkill))
            {
                switch (choseSkill.Speciality)
                {
                    case 0: // Single Target
                        attacked.Receive(choseSkill.Damage);
                        break;
                    case 1: // Area Attack
                        if (enemies.Contains(attacked))
                        {
                            foreach (Unit foe in enemies)
                            {
                                foe.Receive(choseSkill.Damage);
                            }
                        }
                        else if (allies.Contains(attacked))
                        {
                            foreach (Unit foe in allies)
                            {
                                foe.Receive(choseSkill.Damage);
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Something has gone wrong with using the skill");
                        break;
                }
            }
        }
        // Checks for if someone has died
        public bool DeadMansClaw()
        {
            foreach (Unit item in allies)
            {
                if (item.Dead())
                {
                    allies.Remove(item);
                    Console.WriteLine(item.Namer() + " died");
                    break;
                }
            }
            if (allies.Count == 0)
            {
                Console.WriteLine("No more units left");
                return true;
            }
            foreach (Unit item in enemies)
            {
                if (item.Dead())
                {
                    enemies.Remove(item);
                    Console.WriteLine(item.Namer() + " died");
                    break;
                }
            }
            if (enemies.Count == 0)
            {
                Console.WriteLine("No more foes left. You win!");
                return true;
            }
            return false;
        }
    }
}
