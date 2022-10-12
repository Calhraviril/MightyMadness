using MightyMadness;

#region Beginning
// Coloring
Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.Red;
Console.Title = "Mighty Madness";

// Stupid stuff that i have to put here because "CTRL + Z was specifically asked", despite that being overcomplex and unnecessary
ConsoleKeyInfo key;

// Values and Bools
bool selecting = true;
bool gameActive = true;
int keyTurn = 0; // Selected key
int skill = 0; // Stores chosen skill
int target = 0; // Stores chosen target
int curOrder = 0;

// Start stuff
Writer writer = new();
Battlecontrol mainBattle = new();
DH.BattleState state = DH.BattleState.menu;
#endregion

// The games loop
while (gameActive)
{
    switch (state)
    {
        case DH.BattleState.menu:
            writer.WriteColored("-------------------------------------", 1, "          Mighty Madness", 1, "-------------------------------------", 1, "Press any key to begin");
            Console.ReadKey();
            state = DH.BattleState.armyCreate;
            break;
        case DH.BattleState.armyCreate:
            mainBattle.ManifestArmy(6);
            state = DH.BattleState.battle;
            break;
        case DH.BattleState.battle:
            while (true)
            {
                HonsoleClear();
                int order = DH.battleOrder.Count;
                if (curOrder < order && state != DH.BattleState.gameOver)
                {
                    Unit unit = DH.battleOrder[curOrder];
                    TrueBattle(unit);
                    curOrder++;
                }
                else
                {
                    curOrder = 0;
                    break;
                }
            }
            mainBattle.DeleteHistory();
            break;
        case DH.BattleState.gameOver:
            Console.Clear();
            writer.WriteColored(ConsoleColor.Gray, "GAME OVER", 1);
            Thread.Sleep(1000);
            gameActive = false;
            break;
    }
}

#region Combat
void TrueBattle(Unit unit)
{
    if (DH.enemies.Contains(unit))
    {
        FoeBattle(unit);
    }
    else if (DH.allies.Contains(unit))
    {
        PlayerBattle(unit);
    }
}
void PlayerBattle(Unit unit)
{
    if (DH.allies.Contains(unit))
    {
        selecting = true;
        keyTurn = 0;

        while (selecting)
        {
            if (keyTurn < 0) { keyTurn = 0; }
            switch (keyTurn)
            {
                case 0:
                    writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose skill:" + writer.WriteList((object[])unit.skills.ToArray()), 1);
                    break;
                case 1:
                    writer.WriteColored(ConsoleColor.White, unit.Name, 0, " Choose target:" + writer.WriteList((object[])DH.enemies.ToArray), 1);
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
                        if (target > DH.enemies.Count) { target = DH.enemies.Count; }
                        Console.WriteLine("");
                        keyTurn++;
                        writer.WriteColored("Press CTRL + T at any time to open a tutorial listing", 1);
                        break;
                    case 2:
                        selecting = false;
                        break;
                }
            }
        }
        mainBattle.PlayerScenario(unit, target, skill);
    }
    LastProtocol();
}
void FoeBattle(Unit unit)
{
    mainBattle.FoeScenario(unit);
    LastProtocol();
}
void LastProtocol()
{
    if (!DH.allies.Any || !DH.enemies.Any)
    {
        Console.Clear();
        state = DH.BattleState.gameOver;
    }
}
#endregion
#region Misc
void CommandInput()
{
    if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key.ToString() == "Q")
    {
        keyTurn--;
        HonsoleClear();
        writer.WriteColored(1, "Cant revert that far", 1);
    }
    else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key.ToString() == "T")
    {
        HonsoleClear();
        writer.Tutor();
    }
    else if ((key.Modifiers & ConsoleModifiers.Control) != 0 && key.Key.ToString() == "Z")
    {
        try
        {
            mainBattle.Revert(DH.protocols[DH.protocols.Count - 1]);
            if(DH.protocols.Remove(DH.protocols[DH.protocols.Count - 1]))
            {
                HonsoleClear();
                curOrder--;
                if (curOrder < 0) { curOrder = DH.battleOrder.Count; }
            }
        }
        catch(Exception e)
        {
            HonsoleClear();
            writer.WriteColored("" + (DH.protocols.Count - 1));
            Console.WriteLine("It broke : " + e.Message);
        }
        
    }
    else
    {
        HonsoleClear();
    }
}
void HonsoleClear()
{
    Console.Clear();
    mainBattle.ReadHistory();
    mainBattle.ListArmies();
}
#endregion
#region Variables
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
bool CanBeCastToInt(ConsoleKeyInfo choseKey)
{
    try { int conCheck = Convert.ToInt32(choseKey.KeyChar.ToString()); }
    catch { return false; }
    return true;
}
#endregion