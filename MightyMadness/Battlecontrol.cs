using Newtonsoft.Json;
using MightyMadness;
using System.Data;
using System.Runtime.CompilerServices;
using System;

namespace MightyMadness
{
    public static class DH // AKA: DataHandler
    {
        public static UnitJSONReader allyHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\units.json"));
        public static UnitJSONReader foeHandler = JsonConvert.DeserializeObject<UnitJSONReader>(File.ReadAllText(@".\foes.json"));

        public static ArmyData allies;
        public static ArmyData enemies;
        public static List<Unit> battleOrder;

        public static List<string> history = new();
        public static List<attackProtocol> protocols = new();

        public enum BattleState
        {
            menu,
            armyCreate,
            battle,
            gameOver
        }
    }
    public class Battlecontrol
    {
        Random random = new();
        Writer writer = new();

        ArmyData Armed(List<Unit> addUnit)
        {
            ArmyData thisthing = new();

            thisthing.units = addUnit;
            thisthing.grave = new();
            thisthing.Sort();

            return thisthing;
        }

        #region "Army creation"
        // Creates both armies
        public void ManifestArmy(int amount)
        {
            ManifestAlly();
            ManifestEnemy(amount);
            SetBattleOrder();
        }
        private void ManifestAlly()
        {
            int costPoints = 3;

            List<Unit> alli = new List<Unit>();
            while (true)
            {
                Console.Clear();
                writer.WriteColored("Choose an unit: " + costPoints, 1, writer.ListJsonUnit());
                int input = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;

                var call = DH.allyHandler.Units[input].Stats;
                alli.Add(new Unit(DH.allyHandler.Units[input].Name, call.Health, call.Mana, call.Defence, call.Speed, Skills(input, DH.allyHandler)));

                if ((costPoints - call.Cost) > 0) { costPoints = costPoints - call.Cost; }
                else { break; }


            }
            DH.allies = Armed(alli);
        }
        private void ManifestEnemy(int amount)
        {
            List<Unit> enem = new List<Unit>();
            for (int i = 0; i < amount; i++)
            {
                int chosen = random.Next(DH.foeHandler.Units.Count);
                var call = DH.foeHandler.Units[chosen].Stats;
                enem.Add(new Unit(DH.foeHandler.Units[chosen].Name, call.Health, call.Mana, call.Defence, call.Speed, Skills(chosen, DH.foeHandler)));
            }
            DH.enemies = Armed(enem);
        }
        void SetBattleOrder()
        {
            List<Unit> units = new();
            units.AddRange(DH.allies.All);
            units.AddRange(DH.enemies.All);

            DH.battleOrder = units.OrderBy(o => o.speed).ToList();
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

        #region "Combat turns"
        public void PlayerScenario(Unit unit, int target, int skillIndex)
        {
            try
            {
                Unit enemy = DH.enemies.ToArray[target];
                Skill chosenSkill = unit.skills[skillIndex];

                attackProtocol protocol = new attackProtocol(unit, enemy, chosenSkill);
                DH.protocols.Add(protocol);

                Attack(protocol);

                SetHistory(unit.Name + " attacked " + enemy.Name + " with " + chosenSkill.Name + ", dealing " + chosenSkill.Damage + " damage");
                writer.Flash(ConsoleColor.DarkRed, 250);
            }
            catch (Exception e)
            {
                Console.WriteLine(e + " Error occurred due to problems with input");
            }
        }
        public void FoeScenario(Unit unit)
        {
            int ran1 = random.Next(0, DH.allies.Count); // Random chosen from allies
            int ran2 = random.Next(0, unit.skills.Count); // Random chosen from enemy skills
            Unit target = DH.allies.ToArray[ran1];

            attackProtocol protocol = new attackProtocol(unit, target, unit.skills[ran2]);
            DH.protocols.Add(protocol);
            Attack(protocol);

            SetHistory(unit.Name + " attacked " + target.Name + " with " + unit.skills[ran2].Name + ", dealing " + unit.skills[ran2].Damage + " damage");
            writer.Flash(ConsoleColor.DarkRed, 50);
        }
        #endregion

        public void Attack(attackProtocol prt)
        {
            if (prt.atk.UseSkill(prt.skill))
            {
                switch (prt.skill.Speciality)
                {
                    case 0: // Single Target
                        if (prt.def.Receive(prt.skill.Damage))
                        {
                            try { DH.enemies.Remove(prt.def); }
                            catch { DH.allies.Remove(prt.def); }
                        }
                        break;
                    case 1: // Area Attack
                        if (DH.enemies.Contains(prt.def))
                        {
                            for (int i = DH.enemies.Count - 1; i > -1; i--)
                            {
                                if (prt.def.Receive(prt.skill.Damage)) { DH.enemies.Remove(prt.def); }
                            }
                        }
                        else if (DH.allies.Contains(prt.def))
                        {
                            for (int i = DH.allies.Count - 1; i > -1; i--)
                            {
                                if (prt.def.Receive(prt.skill.Damage)) { DH.allies.Remove(prt.def); }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Something has gone wrong with using the skill");
                        break;
                }
            }
        }

        // Completes the given action in reverse, reverting any actions completed during that saved turn
        public void Revert(attackProtocol protoAttack)
        {
            Unit atk = protoAttack.atk;
            Unit def = protoAttack.def;
            Skill skill = protoAttack.skill;

            Skill newSkill = new Skill(skill.Name, skill.Damage, skill.ManaCost, skill.Speciality);

            newSkill.damage = -newSkill.damage;
            attackProtocol revertProto = new attackProtocol(atk, def, newSkill);
            Attack(revertProto);
        }

        // Lists the living units of both armies
        public void ListArmies()
        {
            writer.WriteColored(ConsoleColor.Yellow, "- - - Armies List - - -", 0);
            Console.WriteLine("");
            Console.Write("Your units are: ");
            foreach (Unit unit in DH.allies.All)
            {
                writer.WriteColored(ConsoleColor.White, unit.Name + " (" + unit.LifePercentage() + "%) ", 0, ":", ConsoleColor.White, writer.WriteList((object[])unit.skills.ToArray()), 0, " / ");
            }
            Console.WriteLine("");
            Console.Write("Your enemies are: ");
            foreach (Unit unit in DH.enemies.All)
            {
                writer.WriteColored(ConsoleColor.Gray, unit.Name + " (" + unit.LifePercentage() + "%) ", 0, ":", ConsoleColor.White, writer.WriteList((object[])unit.skills.ToArray()), 0, " / ");
            }
            Console.WriteLine("");
        }

        #region "Battle history"
        public void SetHistory(string historize)
            {
                DH.history.Add(historize);
            }
            public void ReadHistory()
            {
                writer.WriteColored(ConsoleColor.Yellow, "- - - Battle Results - - -", 0, 1);
                foreach (string segment in DH.history)
                {
                    Console.WriteLine(segment);
                }
            }
            public void DeleteHistory()
            {
                DH.history.Clear();
            }
            #endregion
    }
    public struct attackProtocol
    {
        public attackProtocol(Unit attaker, Unit attaked, Skill skilled)
        {
            atk = attaker;
            def = attaked;
            skill = skilled;
        }
        public Unit atk;
        public Unit def;
        public Skill skill;
    }
    public struct ArmyData
        {
            public List<Unit> units;
            public List<Unit> grave;

            #region "Misc"
            public void Sort()
            {
                units = units.OrderBy(o => o.speed).ToList();
            }
            public void Remove(Unit removed)
            {
                if (units.Contains(removed))
                {
                    grave.Add(removed);
                    units.Remove(removed);
                }
            }
            public bool Contains(Unit unit)
            {
                if (units.Contains(unit)) { return true; }
                return false;
            }
            public List<Unit> All { get { return units; } }
            public int Count { get { return units.Count; } }
            public bool Any { get { return units.Any(); } }
            public Unit[] ToArray { get { return units.ToArray(); } }

            #endregion
        }
}
