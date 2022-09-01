using Newtonsoft.Json;
namespace MightyMadness
{
    public class Battlecontrol
    {
        UnitJSONReader allyHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\units.json"));
        UnitJSONReader foeHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\foes.json"));
        Random random = new Random();
        private List<Unit> allies = new List<Unit>();
        private List<Unit> enemies = new List<Unit>();

        public void ManifestArmy(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(allyHandler.Units.Count);
                var call = allyHandler.Units[chosen].Stats;
                allies.Add(new Unit(allyHandler.Units[chosen].Name, call.Damage, call.Health, call.Mana, call.Defence));
            }
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(foeHandler.Units.Count);
                var call = foeHandler.Units[chosen].Stats;
                enemies.Add(new Unit(foeHandler.Units[chosen].Name, call.Damage, call.Health, call.Mana, call.Defence));
            }
        }

        public void ListArmies()
        {
            Console.Write("Your units are: ");
            foreach (Unit unit in allies)
            {
                Console.Write(unit.Namer() + ", ");
            }
            Console.WriteLine("");
            Console.Write("Your enemies are: ");
            foreach (Unit unit in enemies)
            {
                Console.Write(unit.Namer() + ", ");
            }
            Console.WriteLine("");
        }

        public void BattleScenario(int attacker, int attacked)
        {
            // Making sure the attacker + attacked arent empty
            if(attacker > allies.Count || attacked > enemies.Count)
            {
                Console.WriteLine("Trying to use/attack a non-existent unit");
                return;
            }

            // Players attack
            allies[attacker].Attack(enemies[attacked]);
            Console.WriteLine(allies[attacker].Namer() + " attacked " + enemies[attacked].Namer());

            // Random values x 2
            int ran1 = random.Next(0, enemies.Count);
            int ran2 = random.Next(0, allies.Count);

            // Enemies attack
            enemies[ran1].Attack(allies[ran2]);
            Console.WriteLine(enemies[ran1].Namer() + " attacked " + allies[ran2].Namer());
        }

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
