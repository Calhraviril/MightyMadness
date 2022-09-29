using MightyMadness;

// Coloring
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Red;
Console.Title = "Mighty Madness";

// Stupid stuff that i have to put here because "CTRL + Z was specifically asked", despite that being overcomplex and unnecessary
ConsoleKeyInfo key;

// Values and Bools
bool selecting = true;
int keyTurn = 0; // Selected key
int skill = 0; // Stores chosen skill
int target = 0; // Stores chosen target

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
            selecting = true;
            keyTurn = 0;
            while (selecting)
            {
                switch (keyTurn)
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
                key = Console.ReadKey();
                if (!CanBeCastToInt(key))
                {
                    CommandInput();
                }
                else
                {
                    if(keyTurn < 0) { keyTurn = 0; }
                    switch (keyTurn)
                    {
                        case 0:
                            skill = StringConvert(key) - 1;
                            if (skill > unit.skills.Count) { skill = unit.skills.Count; }
                            Console.WriteLine("");
                            keyTurn++;
                            break;
                        case 1:
                            target = StringConvert(key) - 1;
                            if (target > mainBattle.enemies.Count) { target = mainBattle.enemies.Count; }
                            Console.WriteLine("");
                            keyTurn++;
                            writer.WriteColored("Press CTRL + T at any time to open a tutorial listing", 1);
                            break;
                        case 2:
                            selecting = false;
                            break;
                        default :
                            Console.WriteLine("Something has gone wrong");
                            keyTurn++; // Just to make sure you did not somehow get a negative value
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
int StringConvert(ConsoleKeyInfo convertable)
{
    try
    {
        return Convert.ToInt32(convertable.KeyChar.ToString());
    }
    catch
    {
        return 0;
    }
}
void CommandInput()
{
    if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key.ToString() == "Q")
    {
        keyTurn--;
        HonsoleClear();
        writer.WriteColored(1, "Reverting one choice backwards", 1, "If you reverted too far, just act as if you have to choose a skill. Ill fix it later", 1);
    }
    else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key.ToString() == "T")
    {
        HonsoleClear();
        writer.Tutor();
    }
}
bool CanBeCastToInt(ConsoleKeyInfo choseKey)
{
    try { int conCheck = Convert.ToInt32(choseKey.KeyChar.ToString()); }
    catch { return false; }
    return true;
}
void HonsoleClear()
{
    Console.Clear();
    mainBattle.ReadHistory();
    mainBattle.ListArmies();
}