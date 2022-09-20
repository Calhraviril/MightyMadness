using MightyMadness;

// Coloring
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Red;
Console.Clear();

// Start stuff
Battlecontrol mainBattle = new();
Writer writer = new();
mainBattle.ManifestArmy(3);

// The battle itself
mainBattle.ListArmies();
while (true)
{
    TrueBattle();
    if (!mainBattle.allies.Any())
    {
        Console.Clear();
        Console.WriteLine("All allies dead. How sad.");
        break;
    }
    else if (!mainBattle.enemies.Any())
    {
        Console.Clear();
        Console.WriteLine("All enemies dead! You win");
        break;
    }
    mainBattle.DeleteHistory();
}
// Testing purpose stuff
Thread.Sleep(1000);

void TrueBattle()
{
    foreach (Unit unit in mainBattle.battleOrder)
    {
        if (!mainBattle.allies.Any() || !mainBattle.enemies.Any())
        {
            break;
        }
        if (mainBattle.allies.Contains(unit))
        {
            writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose skill:" + writer.WriteList((object[])unit.skills.ToArray()), 1);
            int skill = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;
            if (skill > unit.skills.Count) { skill = unit.skills.Count; }
            
            Console.WriteLine("");

            // Add a skip here if skill is untargetable
            writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose target:" + writer.WriteList((object[])mainBattle.enemies.ToArray()), 1);
            int target = Convert.ToInt32(Console.ReadKey().KeyChar.ToString()) - 1;
            if (target > mainBattle.enemies.Count) { target = mainBattle.enemies.Count; }

            mainBattle.PlayerScenario(unit, target, skill);
        }
        else if (mainBattle.enemies.Contains(unit))
        {
            mainBattle.FoeScenario(unit);
        }
        mainBattle.ReadHistory();
        mainBattle.ListArmies();
    }
}