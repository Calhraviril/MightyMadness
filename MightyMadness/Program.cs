using MightyMadness;

// Coloring
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Red;
Console.Title = "Mighty Madness";

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
            bool selecting = true;
            string action = "";
            int actionTurn = 0; // Selected action
            int skill = 0; // Stores chosen skill
            int target = 0; // Stores chosen target
            while (selecting)
            {
                switch (actionTurn)
                {
                    case 0:
                        writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose skill:" + writer.WriteList((object[])unit.skills.ToArray()), 1);
                        break;
                    case 1:
                        writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose target:" + writer.WriteList((object[])mainBattle.enemies.ToArray()), 1);
                        break;
                    default:
                        break;
                }
                action = Console.ReadKey().KeyChar.ToString();
                if (action == "z")
                {
                    actionTurn--;
                }
                else
                {
                    switch (actionTurn)
                    {
                        case 0:
                            skill = StringConvert(action) - 1;
                            if (skill > unit.skills.Count) { skill = unit.skills.Count; }
                            Console.WriteLine("");
                            actionTurn++;
                            break;
                        case 1:
                            target = StringConvert(action) - 1;
                            if (target > mainBattle.enemies.Count) { target = mainBattle.enemies.Count; }
                            Console.WriteLine("");
                            actionTurn++;
                            writer.WriteColored(ConsoleColor.Yellow, "Are you sure of these actions?", 1, "Press Z to return to previous decision", 1, "Press anything else to continue", 1, 0);
                            break;
                        case 2:
                            selecting = false;
                            break;
                    }
                }
            }
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
int StringConvert(string convertable)
{
    try
    {
        return Convert.ToInt32(convertable);
    }
    catch
    {
        return 0;
    }
}