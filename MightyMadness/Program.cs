using MightyMadness;
using System;

// Coloring
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Red;
Console.Clear();

// Start stuff
Battlecontrol mainBattle = new();
Writer writer = new();

mainBattle.ManifestArmy(3);


// The battle itself
while (true)
{
    mainBattle.ListArmies();
    // Choose Unit
    Console.Write("Your turn: ");
    int e = 0;
    foreach (Unit unit in mainBattle.allies)
    {
        e++;
        writer.WriteColored(e + ". ", ConsoleColor.White, unit.Namer(), null, " ");
    }
    Console.WriteLine("");
    int attacker = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;
    Console.WriteLine("");

    // Choose Skill
    Console.Write("Choose Skill: ");
    int a = 0;
    foreach (Skill skilled in mainBattle.allies[attacker].skills)
    {
        a++;
        writer.WriteColored(a + ". ", ConsoleColor.White, skilled.Name, null, " ");
    }
    Console.WriteLine("");
    int skill = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;
    Console.WriteLine("");

    // Target
    Console.Write("Who to attack: ");
    int o = 0;
    foreach (Unit unit in mainBattle.enemies)
    {
        o++;
        writer.WriteColored(o + ". ", ConsoleColor.Gray, unit.Namer(), null, " ");
    }
    Console.WriteLine("");
    int attacked = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;
    Console.WriteLine("");

    // Actions
    mainBattle.BattleScenario(attacker, skill, attacked);
    
    if (mainBattle.DeadMansClaw())
    {
        break;
    }
}

