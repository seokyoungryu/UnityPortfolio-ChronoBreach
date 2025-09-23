using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;

public enum SymbolType
{
    NONE = -1,
    PLUS = 0,
    MINUS = 1,
    RESET = 2,
}

[System.Serializable]
public struct HelpOption
{
    public string optionKey;
    public string description;
}

[System.Serializable]
public class CreateObjInfo
{
    public string command = string.Empty;
    public string obpName = string.Empty;
    public GameObject obp_Go = null;
}

public class ConsoleProcess : MonoBehaviour
{
    [SerializeField] private GlobalPresentRegisterDB globalPresentDB;
    [SerializeField] private List<ConsoleProcessInfo> infos = new List<ConsoleProcessInfo>();
    [SerializeField] private List<SearchDictionary> filters = new List<SearchDictionary>();
    [SerializeField] private List<HelpOption> helpOptions = new List<HelpOption>();
    private ConsoleProcessInfo currentProcessInfo;
    private string currValueStr = string.Empty;
    private string currHelpOption = string.Empty;
    private string[] currAllowRange;

    [SerializeField] private CameraShakeInfo[] shakeInfo;
    private List<CreateObjInfo> testObjects = new List<CreateObjInfo>();

    #region Events
    public delegate void ExcuteLog(string log, ConsoleSystemLogType type);
    public delegate void Clear();

    public event ExcuteLog onExcuteLog;
    public event Clear onClear;
    #endregion


    public string tes;

    [ContextMenu("T")]
    public void Tess()
    {
        Excute(tes);
    }

    public void Excute(string inputCommand)
    {
        currentProcessInfo = null;
        currHelpOption = GetHelpOtipon(inputCommand);
        string rapping = inputCommand.TrimEnd();
        if(CheckIsAllSameSentence(inputCommand))
        {
            onExcuteLog?.Invoke("Error! Please Change Value", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (CheckIsHelpOption(currHelpOption))
            rapping = inputCommand.Replace(currHelpOption, "").TrimEnd();

        ConsoleProcessInfo findInfo = FindCommand(rapping);
        if (findInfo == null)
        {
            Debug.Log("<color=red>findInfo 없음 : " + rapping + "</color>");
            onExcuteLog?.Invoke("Error! Is Not Exist Command!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        else Debug.Log("<color=green>찾음 : " + findInfo.Command + "</color>");
       
        currentProcessInfo = findInfo;
        currValueStr = GetCurrentValue(rapping);
        currAllowRange = GetAllowRange(currentProcessInfo);

        if (currentProcessInfo != null)
        {
            Debug.Log("Excute!");
            currentProcessInfo.OnExcute?.Invoke();
        }
        else
            Debug.Log("Not Excute!");
    }

    private ConsoleProcessInfo FindCommand(string inputCommand)
    {
        string[] inputSplit = inputCommand.ToLower().Split(' ');
        string[] infoSplit;

        ConsoleProcessInfo findInfo = null;
        bool isFind = false;

        for (int i = 0; i < infos.Count; i++)
        {
            if (isFind) break;

            infoSplit = infos[i].Command.ToLower().Split(' ');
            for (int x = 0; x < infoSplit.Length; x++)
            {
                if (x >= inputSplit.Length || (infoSplit[x] != inputSplit[x] && infos[i].ValueIndex != x)) break;
                if (infos[i].ValueIndex == x && x != (infoSplit.Length - 1)) continue;
                if (x == (infoSplit.Length - 1) && x == inputSplit.Length - 1)
                {
                    if (infoSplit[x] == inputSplit[x] || infos[i].ValueIndex == x)
                    {
                        findInfo = infos[i];
                        isFind = true;
                        break;
                    }
                }
            }
        }

        return findInfo;
    }

    private string GetCurrentValue(string command)
    {
        return GetValue(command, currentProcessInfo.ValueIndex);
    }

    private string GetValue(string command, int valueIndex)
    {
        string[] split = command.ToLower().Split(' ');
        if (valueIndex < 0 || split.Length < valueIndex)
            return "-1";
        else
            return split[valueIndex];
    }

    private string[] GetAllowRange(ConsoleProcessInfo info)
    {
        if (info == null || info.AllowRange == null || info.AllowRange.Length < 0)
            return null;

        return info.AllowRange;
    }

    private string GetHelpOtipon(string command)
    {
        string[] split = command.Split(' ');
        if (split.Length <= 0)
            return string.Empty;

        for (int i = 0; i < helpOptions.Count; i++)
            if (helpOptions[i].optionKey.Trim() == split[split.Length - 1].Trim())
                return helpOptions[i].optionKey;

        return string.Empty;
    }

    public List<SearchDictionary> GetTextFilters()
    {
        return filters;
    }

    public string[] GetTextList()
    {
        List<string> texts = new List<string>();
        string[] split;

        for (int i = 0; i < infos.Count; i++)
        {
            split = infos[i].Command.Split(' ');
            for (int x = 0; x < split.Length; x++)
                if (infos[i].ValueIndex < 0 && x == infos[i].ValueIndex)
                    split[x] = GetTransFilter(split[x]);

            string sum = string.Empty;
            for (int x = 0; x < split.Length; x++)
                sum += split[x] + " ";
            texts.Add(sum);
        }

        return texts.ToArray();
    }

    public string GetTransFilter(string inputKey)
    {
        for (int i = 0; i < filters.Count; i++)
            if (filters[i].transValue == inputKey)
                return filters[i].key;
        return "Null";
    }

    public bool CheckIsAllSameSentence(string inputCommand)
    {
        for (int i = 0; i < infos.Count; i++)
            if (inputCommand == infos[i].Command && infos[i].ValueIndex != -1)
                return true;

        return false;
    }

    public bool CheckCommonCondition()
    {
        if (currentProcessInfo == null || currValueStr == string.Empty || currValueStr == "-1")
            return false;

        return true;
    }

    public bool CheckSameHelpOption(string inputOptionKey, string currOptionKey)
    {
        if (inputOptionKey == currOptionKey)
            return true;
        return false;
    }

    public bool ValidateCurrHelpOption(params string[] keys)
    {
        if (currHelpOption == string.Empty || currHelpOption.Trim() == "")
            return true;

        for (int i = 0; i < keys.Length; i++)
            if (keys[i] == currHelpOption)
                return true;

        onExcuteLog?.Invoke($"Error! [{currHelpOption}] This Help Option Not Exist",ConsoleSystemLogType.SYSTEM_ERROR_LOG );
        return false;
    }

    public bool CheckIsHelpOption(string optionKey)
    {
        if (optionKey == string.Empty || optionKey == " ")
            return false;

        bool isFind = false;
        for (int i = 0; i < helpOptions.Count; i++)
        {
            if (helpOptions[i].optionKey.Trim() == optionKey.Trim())
            {
                isFind = true;
                break;
            }
        }

        if (isFind)
            return true;
        else
        {
            onExcuteLog?.Invoke($"Error! {optionKey} is not Exist!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return false;
        }

    }

    public bool CompareTwoAllowRange(string[] range, float value)
    {
        if (range != null && range.Length == 2)
        {
            float min = float.Parse(range[0]);
            float max = float.Parse(range[1]);
            Debug.Log($"Range {min} ~ {max}");
            if (value < min || value > max)
            {
                Debug.Log($"Range {min} ~ {max}  IN!");
                onExcuteLog?.Invoke($"Error! Value is out range _ Min {min} , Max {max}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
                return false;
            }
        }

        Debug.Log($"Range exit");
        return true;
    }

    public bool CompareStrings(string[] range, string valueStr)
    {
        if (range != null)
        {
            bool isFind = false;
            for (int i = 0; i < range.Length; i++)
            {
                if (valueStr.ToLower() == currAllowRange[i].ToLower())
                {
                    Debug.Log($"<color=green>{valueStr} == {currAllowRange[i]}</color>");
                    isFind = true;
                }
                else
                    Debug.Log($"<color=red>{valueStr} != {currAllowRange[i]}</color>");
            }
            if (!isFind)
            {
                onExcuteLog?.Invoke($"Error! Value is not Find", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
                return false;
            }
        }
        return true;
    }

    private CreateObjInfo GetCreatedTestObj(string command)
    {
        for (int i = 0; i < testObjects.Count; i++)
            if (testObjects[i].command == command)
                return testObjects[i];

        return null;
    }

    #region Etc
    public void ExcutePrintAllCommands()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Exist All Commands", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        Debug.Log("Excute All COmmand");

        for (int i = 0; i < infos.Count; i++)
            onExcuteLog?.Invoke(infos[i].Command, ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Exist All Commands";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcuteClearCommandTasks()
    {
        if (!ValidateCurrHelpOption("-h")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Clear All Command Tasks In Console", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        onClear?.Invoke();
    }


    #endregion

    #region Player Info
    public void ExcutePrintPlayerConditions()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Player Editable Conditions", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        PlayerConditions condition = GameManager.Instance.Player.Conditions;
        onExcuteLog?.Invoke($"---- Current Player Editable Conditions ----", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Immortal : {condition.IsImmortal}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Ignore Damage : {condition.IgnoreDamaged}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Skill No CoolTime : {condition.SkillNoCooltime}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Infinity Stamina : {condition.InfinityStamina}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"--------------------------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Player Editable Conditions!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePrintPlayerEditStats()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Player Edit Stats", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        PlayerStatus stats = GameManager.Instance.Player.playerStats;
        onExcuteLog?.Invoke($"---- Current Player Edit Stats ----", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Atk             : {stats.EditAtk}  ( Total Atk : {stats.TotalAtk} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Atk Speed       : {stats.EditAtkSpeed}  ( Total Atk Speed: {stats.TotalAtkSpeed} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Max Health      : {stats.EditHealth}  ( Total Max Health : {stats.TotalHealth} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Max Stamina     : {stats.EditStamina}  ( Total Max Stamina : {stats.TotalStamina} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Evasion         : {stats.EditEvasion}  ( Total Evasion : {stats.TotalEvasion} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Defense         : {stats.EditDefense}  ( Total Defense : {stats.TotalDefense} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Magic Defense   : {stats.EditMagicDefense}  ( Total Magic Defense : {stats.TotalMagicDefense} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Critical Chance : {stats.EditCriChance}  ( Total Critical Chance : {stats.TotalCriChance} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Critical Damage : {stats.EditCriDmg}  ( Total Critical Damage : {stats.TotalCriDmg} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-                                                ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit All Move Speed        : {stats.EditAllMoveSpeed}   ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Walk Move Speed       : {stats.EditWalkSpeed}  ( Total Walk Move Speed : {stats.OriginWalkSpeed + stats.EditWalkSpeed} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit Sprint Move Speed     : {stats.EditSprintSpeed}  ( Total Sprint Move Speed : {stats.OriginSprintSpeed + stats.EditSprintSpeed} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"-Edit LimitBreak Move Speed : {stats.EditLimitBreakSpeed}  ( Total LimitBreak Move Speed : {stats.OriginLimitBreakSpeed + stats.EditLimitBreakSpeed} )", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"--------------------------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Player Edit Stats!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcuteGetMoney()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Add Player Own Money , Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int money = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, money))
            return;

        GameManager.Instance.SetPlusOwnMoney(money);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Money +{money}G  , Current own Money {GameManager.Instance.OwnMoney}G";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteGetReputation()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Add Player Reputation, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int reputation = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, reputation))
            return;


        GameManager.Instance.SetPlusOwnMoney(reputation);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Reputation +{reputation}  , Current Reputation {GameManager.Instance.Reputation}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteLevelUp()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If Executed Command, It Will Level Up The Player, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int level = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, level))
            return;

        GameManager.Instance.Player.playerStats.AddLevel(level);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Excute Player Level UP +{level} , Current Player Lv.{GameManager.Instance.Player.playerStats.Level}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteSkillPoint()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Add Player Skill Point, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int skillPoint = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, skillPoint))
            return;

        GameManager.Instance.Player.playerStats.AddSkillPoint(skillPoint);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Skill Point +{skillPoint}  , Current Skill Point{ GameManager.Instance.Player.playerStats.RemainingSkillPoint}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteStatPoint()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Add Player Stat Point, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int statPoint = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, statPoint))
            return;

        GameManager.Instance.Player.playerStats.AddStatsPoint(statPoint);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Player Stat Point +{statPoint}  , Current Player Stat Point{ GameManager.Instance.Player.playerStats.RemainingSkillPoint}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditAtk(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Atk Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int editAtk = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editAtk))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditAtk += editAtk;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditAtk -= editAtk;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditAtk = 0;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Excute Player Atk +{editAtk} , Current Edit Value {GameManager.Instance.Player.playerStats.EditAtk}, Total Player Atk {GameManager.Instance.Player.playerStats.TotalAtk}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditAtkSpeed(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Atk Speed Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editAtkSpeed = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editAtkSpeed))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditAtkSpeed += editAtkSpeed;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditAtkSpeed -= editAtkSpeed;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditAtkSpeed = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Atk Speed {editAtkSpeed} , Current Edit Atk Speed {GameManager.Instance.Player.playerStats.EditAtkSpeed}, Total Player Atk Speed {GameManager.Instance.Player.playerStats.TotalAtkSpeed}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditHealth(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Health Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int editHealth = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editHealth))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.AddEditMaxHealth(editHealth);
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.RemoveEditMaxHealth(editHealth);
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.ResetEditMaxHealth();

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Max HP {editHealth} , Current Edit Max HP {GameManager.Instance.Player.playerStats.EditHealth}, Total Player HP {GameManager.Instance.Player.playerStats.TotalHealth}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditStanima(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Stanima Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int editStamina = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editStamina))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.AddEditMaxStamina(editStamina);
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.RemoveEditMaxStamina(editStamina);
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.ResetEditMaxStamina();

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Max Stamina {editStamina} , Current Edit Max Stamina {GameManager.Instance.Player.playerStats.EditStamina}, Total Player Stamina {GameManager.Instance.Player.playerStats.TotalStamina}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }

    }
    public void ExcuteEditEvasion(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Evasion Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editEvasion = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editEvasion))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditEvasion += editEvasion;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditEvasion -= editEvasion;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditEvasion = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Evasion {editEvasion}%, Current Edit Evasion {GameManager.Instance.Player.playerStats.EditEvasion}%, Total Player Evasion {GameManager.Instance.Player.playerStats.TotalEvasion}%";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditDefense(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Defense Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int editDefense = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editDefense))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditDefense += editDefense;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditDefense -= editDefense;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditDefense = 0;

        GameManager.Instance.Player.playerStats.UpdateStats();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Defense {editDefense}, Current Edit Defense {GameManager.Instance.Player.playerStats.EditDefense}, Total Player Defense {GameManager.Instance.Player.playerStats.TotalDefense}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditMagicDefense(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Defense Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int editMagicDefense = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editMagicDefense))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditMagicDefense += editMagicDefense;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditMagicDefense -= editMagicDefense;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditMagicDefense = 0;

        GameManager.Instance.Player.playerStats.UpdateStats();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Magic Defense {editMagicDefense}, Current Edit Magic Defense {GameManager.Instance.Player.playerStats.EditMagicDefense}, Total Player Magic Defense {GameManager.Instance.Player.playerStats.TotalMagicDefense}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditCriChance(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Critical Chance Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        float editCriChance = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editCriChance))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditCriChance += editCriChance;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditCriChance -= editCriChance;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditCriChance = 0;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Critical Chance {editCriChance}%, Current Edit Critical Chance {GameManager.Instance.Player.playerStats.EditCriChance}%, Total Player Critical Chance {GameManager.Instance.Player.playerStats.TotalCriChance}%";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditCriDmg(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Critical Damage Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        float editCriDmg = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editCriDmg))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditCriDmg += editCriDmg;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditCriDmg -= editCriDmg;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditCriDmg = 0;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Critical Damage {editCriDmg}%, Current Edit Critical Damage {GameManager.Instance.Player.playerStats.EditCriDmg}%, Total Player Critical Damage {GameManager.Instance.Player.playerStats.TotalCriDmg}%";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditAllMoveSpeed(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player All Movement Speed Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editAllMoveSpeed = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editAllMoveSpeed))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditAllMoveSpeed += editAllMoveSpeed;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditAllMoveSpeed -= editAllMoveSpeed;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditAllMoveSpeed = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} All Move Speed {editAllMoveSpeed} , Current Edit All Move Speed {GameManager.Instance.Player.playerStats.EditAllMoveSpeed}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditWalkMoveSpeed(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Only Walk Movement Speed Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editWalkMoveSpeed = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editWalkMoveSpeed))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditWalkSpeed += editWalkMoveSpeed;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditWalkSpeed -= editWalkMoveSpeed;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditWalkSpeed = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Walk Move Speed {editWalkMoveSpeed} , Current Edit Walk Move Speed {GameManager.Instance.Player.playerStats.EditWalkSpeed}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditSprintMoveSpeed(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Only Sprint Movement Speed Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editSprintMoveSpeed = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editSprintMoveSpeed))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditSprintSpeed += editSprintMoveSpeed;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditSprintSpeed -= editSprintMoveSpeed;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditSprintSpeed = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Sprint Move Speed {editSprintMoveSpeed} , Current Edit Sprint Move Speed {GameManager.Instance.Player.playerStats.EditSprintSpeed}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteEditLimitBreakMoveSpeed(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Can Edit Player Only Limit Break Movement Speed Value, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float editLBMoveSpeed = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, editLBMoveSpeed))
            return;

        if ((SymbolType)index == SymbolType.PLUS) GameManager.Instance.Player.playerStats.EditLimitBreakSpeed += editLBMoveSpeed;
        else if ((SymbolType)index == SymbolType.MINUS) GameManager.Instance.Player.playerStats.EditLimitBreakSpeed -= editLBMoveSpeed;
        else if ((SymbolType)index == SymbolType.RESET) GameManager.Instance.Player.playerStats.EditLimitBreakSpeed = 0f;

        GameManager.Instance.Player.playerStats.UpdateStats();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player {(SymbolType)index} Limit Break Move Speed {editLBMoveSpeed} , Current Edit Limit Break Move Speed {GameManager.Instance.Player.playerStats.EditLimitBreakSpeed}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcuteAllResetEditStats()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"All Reset Edit Player Stats ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Player.playerStats.AllResetEditStats();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Reset All Player Edit Stats";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePlayerSkillNoCooltime()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Player All Skill Can Excute No CoolTime! , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.Player.Conditions.SkillNoCooltime = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Player skill no coolTime {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePlayerSkillCooltimeReset()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Player's Skills That Are Currently On Cooldown", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (GameManager.Instance.Player.skillController.GetCurrCoolTimeCount <= 0)
        {
            string log = $"Error! It does not exist skill on cooldown.";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
            return;
        }

        GameManager.Instance.Player.skillController.ResetSkillCoolTime();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! All Reset Player Skill Cooltime.";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteIgnoreDamage()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Player Ignore All Damaged! , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.Player.Conditions.IgnoreDamaged = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Player becomes Ignore Damage {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteInfinityStamina()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Player Have Infinity Stamina! , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.Player.Conditions.InfinityStamina = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Player becomes Infinity Stamina {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteImmortal()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Player Immortal! , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.Player.Conditions.IsImmortal = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Player becomes Immortal {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePlayerSetFullHealth()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If Excute, Player Set Full Health Once.", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Player.playerStats.SetFullHP();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Set Player Full Health";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteDealPlayerDamaged()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        PlayerStateController player = GameManager.Instance.Player;
        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Excute Deal Damage {player.playerStats.CurrentHealth} / {player.playerStats.TotalHealth}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int damageValue = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, damageValue))
            return;

        GameManager.Instance.Player.playerStats.Damaged(damageValue,null,false,false,AttackStrengthType.NORMAL);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Deal Damage To Player <color=red>-{damageValue}</color>  Curr Hp : {player.playerStats.CurrentHealth} / {player.playerStats.TotalHealth}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteIgnoreSkillCondition()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Ignore Skill Upgrade Condition , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.ignoreSkillCondition = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Ignore Skill Upgrade Condition {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteIgnoreItemEquipCondition()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"If True, Ignore Item Equipment Condition , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        GameManager.Instance.ignoreItemEquipCondition = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Ignore Item Equipment Condition {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    #endregion

    #region Inven
    public void ExcuteClearInventory()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"<color=red>!! Warning Excute !! </color> Clear All Player Inventory!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        CommonUIManager.Instance.playerInventory.ClearAllInventory();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Clear Inventory";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteGenerateRangomItem()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Gain Only One Random Item ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int count = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, count))
            return;

        int successCount = 0;
        for (int i = 0; i < count; i++)
        {
            Item item = ItemManager.Instance.GenerateRandomItem();
            if (CommonUIManager.Instance.playerInventory.CheckCanAddItems(item, 1))
            {
                successCount++;
                CommonUIManager.Instance.playerInventory.AddItem(item, 1);
            }
        }

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Random Item : {successCount} Fail : {count - successCount} ";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteGenerateSelectItem()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Gain Only One Select Item , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 2 || !int.TryParse(values[0], out int result1) || !int.TryParse(values[1], out int result2))
        {
            onExcuteLog?.Invoke($"Error! Wrong format (Select Index , Count)", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        int itemIndex = int.Parse(values[0].Trim());
        int count = int.Parse(values[1].Trim());
        int min = 5;
        int max = ItemManager.Instance.GetItemData().allItemClips.Length;
        if (itemIndex < min && itemIndex >= max)
        {
            onExcuteLog?.Invoke($"Error! Value is out range _ Min {min} , Max {max}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        Item item = ItemManager.Instance.GenerateItem(itemIndex);
        if (CommonUIManager.Instance.playerInventory.CheckCanAddItems(item, count))
            CommonUIManager.Instance.playerInventory.AddItem(item, count);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Add Item - {item.objectName}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePrintItemList()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print All Item List", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int itemListCount = ItemManager.Instance.GetItemData().allItemClips.Length;
        for (int i = 6; i < itemListCount; i++)
            onExcuteLog?.Invoke($"{i}:{ItemManager.Instance.GetItemData().allItemClips[i].uiItemName}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Item List {itemListCount}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcutePrintCurrentInventoryList()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Current Inventory Item List Index", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"0 : Equipment, 1 : Consumable, 2 : Material, 3 : QuestItem", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int index = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, index))
            return;

        string category = string.Empty;
        if (index == 0) category = "Equipment";
       else if (index == 1) category = "Consumable";
       else if (index == 2) category = "Material";
        else if (index == 3) category = "QuestItem";

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(index);

        onExcuteLog?.Invoke($"-------------- {category} ----------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        for (int i = 0; i < inventoryUI.inventory.slots.Length; i++)
        {
            if (inventoryUI.inventory.slots[i].item.HaveItem())
                onExcuteLog?.Invoke($"{i} : {inventoryUI.inventory.slots[i].item.itemClip.uiItemName} , Amount : {inventoryUI.inventory.slots[i].amount}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            else
                onExcuteLog?.Invoke($"{i} : Empty Slot , Amount : -", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
        onExcuteLog?.Invoke($"-------------------------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Current Inventory Category : [ {category} ] Item List! ";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcuteAllRandomPotential()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Setting All Random Potential Item!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(0);

        int itemIndex = int.Parse(currValueStr);

        if (itemIndex < 0 || itemIndex >= inventoryUI.inventory.slots.Length)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Item Index   Min : 0 , Max : {inventoryUI.inventory.slots.Length - 1}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        if (!inventoryUI.inventory.slots[itemIndex].item.HaveItem())
        {
            onExcuteLog?.Invoke($"Erorr! Item Index {itemIndex} is Empty!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        BaseItemClip itemClip = inventoryUI.inventory.slots[itemIndex].item.itemClip;
        string beforePotential = itemClip.potentialRank.ToString();
        string itemName = itemClip.uiItemName;
        itemClip.SetInitRandomPotential();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Change Item [ {itemName} ] Potential [{beforePotential}] -> [{itemClip.potentialRank.ToString()}] !";
            onExcuteLog?.Invoke($"------- Current {itemName} Potential -----------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank : { itemClip.potentialRank.ToString()}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            for (int i = 0; i < itemClip.potentialOptionCount; i++)
            {
                if (itemClip.ownPotential[i].clipData.isFloatValue)
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue.ToString("F2")}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                else
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            }
            onExcuteLog?.Invoke($"-------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
    }
    public void ExcuteSettingPotential()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Setting Potential Equipment Only Item In Inventory!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank Range None, Rare, Unique, Legendary", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Count Min : 0 , Max : 3", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(0);

        string[] values = currValueStr.Trim().Split(',');
        int itemIndex = int.Parse(values[0]);
        int potentialCount = int.Parse(values[2]);

        Debug.Log("Excute String : " + values[1]);

        if(itemIndex < 0 || itemIndex >= inventoryUI.inventory.slots.Length )
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Item Index   Min : 0 , Max : {inventoryUI.inventory.slots.Length-1}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (!CompareStrings(currAllowRange, values[1]))
            return;

        if (potentialCount < 0 || potentialCount > 3)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Potential Count  Min : 0 , Max : 3", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (!inventoryUI.inventory.slots[itemIndex].item.HaveItem())
        {
            onExcuteLog?.Invoke($"Erorr! Item Index {itemIndex} is Empty!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        BaseItemClip itemClip = inventoryUI.inventory.slots[itemIndex].item.itemClip;
        string beforePotential = itemClip.potentialRank.ToString();
        string itemName = itemClip.uiItemName;
        ItemPotentialRankType rank;
        bool isValidEnum = System.Enum.TryParse(values[1], true, out rank);

        if (isValidEnum)
            itemClip.SetPotentialRank(rank);
        else
        {
            onExcuteLog?.Invoke($"Erorr! [{values[1]}]  Rank Range : None, Rare, Unique, Legendary", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        itemClip.SetPotentialCount(potentialCount);
        itemClip.SetPotentialValue();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Change Item [ {itemName} ] Potential [{beforePotential}] -> [{itemClip.potentialRank.ToString()}] !";
            onExcuteLog?.Invoke($"------- Current {itemName} Potential -----------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank : { itemClip.potentialRank.ToString()}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            for (int i = 0; i < itemClip.potentialOptionCount; i++)
            {
                if (itemClip.ownPotential[i].clipData.isFloatValue)
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue.ToString("F2")}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                else
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            }
            onExcuteLog?.Invoke($"-------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
    }
    public void ExcuteSettingPotentialCount()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Setting Potential Count Only Item In Inventory!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(0);

        string[] values = currValueStr.Trim().Split(',');
        int itemIndex = int.Parse(values[0]);
        int potentialCount = int.Parse(values[1]);

        if (itemIndex < 0 || itemIndex >= inventoryUI.inventory.slots.Length)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Item Index   Min : 0 , Max : {inventoryUI.inventory.slots.Length - 1}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (potentialCount < 0 || potentialCount > 3)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Potential Count  Min : 0 , Max : 3", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (!inventoryUI.inventory.slots[itemIndex].item.HaveItem())
        {
            onExcuteLog?.Invoke($"Erorr! Item Index {itemIndex} is Empty!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        BaseItemClip itemClip = inventoryUI.inventory.slots[itemIndex].item.itemClip;
        string beforePotential = itemClip.potentialRank.ToString();
        string itemName = itemClip.uiItemName;
        itemClip.SetPotentialCount(potentialCount);

      
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            onExcuteLog?.Invoke($"------- Current {itemName} Potential -----------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank : { itemClip.potentialRank.ToString()}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            for (int i = 0; i < itemClip.potentialOptionCount; i++)
            {
                if (itemClip.ownPotential[i].clipData.isFloatValue)
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue.ToString("F2")}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                else
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            }
            onExcuteLog?.Invoke($"-------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
    }
    public void ExcuteSettingPotentialRandomValue()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Setting Potential Random Value Only Item In Inventory!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(0);

        int itemIndex = int.Parse(currValueStr);

        if (itemIndex < 0 || itemIndex >= inventoryUI.inventory.slots.Length)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Item Index   Min : 0 , Max : {inventoryUI.inventory.slots.Length - 1}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (!inventoryUI.inventory.slots[itemIndex].item.HaveItem())
        {
            onExcuteLog?.Invoke($"Erorr! Item Index {itemIndex} is Empty!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        BaseItemClip itemClip = inventoryUI.inventory.slots[itemIndex].item.itemClip;
        string beforePotential = itemClip.potentialRank.ToString();
        string itemName = itemClip.uiItemName;
        itemClip.SetPotentialValue();


        if (CheckSameHelpOption("-l", currHelpOption))
        {
            onExcuteLog?.Invoke($"------- Current {itemName} Potential -----------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank : { itemClip.potentialRank.ToString()}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            for (int i = 0; i < itemClip.potentialOptionCount; i++)
            {
                if (itemClip.ownPotential[i].clipData.isFloatValue)
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue.ToString("F2")}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                else
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            }
            onExcuteLog?.Invoke($"-------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
    }

    public void ExcuteSettingPotentialRank()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Setting Potential Equipment Only Item In Inventory!", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank Range None, Rare, Unique, Legendary", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Count Min : 0 , Max : 3", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        InventoryUI inventoryUI = CommonUIManager.Instance.playerInventory.GetInventoryUI(0);

        string[] values = currValueStr.Trim().Split(',');
        int itemIndex = int.Parse(values[0]);

        if (itemIndex < 0 || itemIndex >= inventoryUI.inventory.slots.Length)
        {
            onExcuteLog?.Invoke($"Erorr! Exceed The Range Item Index   Min : 0 , Max : {inventoryUI.inventory.slots.Length - 1}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        if (!CompareStrings(currAllowRange, values[1]))
            return;

        if (!inventoryUI.inventory.slots[itemIndex].item.HaveItem())
        {
            onExcuteLog?.Invoke($"Erorr! Item Index {itemIndex} is Empty!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        BaseItemClip itemClip = inventoryUI.inventory.slots[itemIndex].item.itemClip;
        string beforePotential = itemClip.potentialRank.ToString();
        string itemName = itemClip.uiItemName;
        ItemPotentialRankType rank;
        bool isValidEnum = System.Enum.TryParse(values[1], true, out rank);

        if (isValidEnum)
            itemClip.SetPotentialRank(rank);
        else
        {
            onExcuteLog?.Invoke($"Erorr! [{values[1]}]  Rank Range : None, Rare, Unique, Legendary", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }


        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Change Item [ {itemName} ] Potential [{beforePotential}] -> [{itemClip.potentialRank.ToString()}] !";
            onExcuteLog?.Invoke($"------- Current {itemName} Potential -----------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            onExcuteLog?.Invoke($"Rank : { itemClip.potentialRank.ToString()}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            for (int i = 0; i < itemClip.potentialOptionCount; i++)
            {
                if (itemClip.ownPotential[i].clipData.isFloatValue)
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue.ToString("F2")}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                else
                    onExcuteLog?.Invoke($"{itemClip.ownPotential[i].potentialName} : +{itemClip.ownPotential[i].potentialValue}{itemClip.ownPotential[i].lastWord}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            }
            onExcuteLog?.Invoke($"-------------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        }
    }
    #endregion

    #region Data
    public void ExcuteSaveData()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"<color=red>!! Warning Excute !! </color> Edit Save Current All Game Play Data", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        SaveManager.Instance.EditAllSave();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Save Edit All Data!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteLoadData()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"<color=red>!! Warning Excute !! </color> Is Exist Edit Save Data, Load Previus Save Data", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!SaveManager.Instance.IsEditSave)
        {
            onExcuteLog?.Invoke("Error! Does Not Exist Edit Save Data!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        SaveManager.Instance.EditLoad();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Load Edit All Data!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteHelpOptionList()
    {
        if (CheckIsHelpOption(currHelpOption))
        {
            onExcuteLog?.Invoke($"Error! Is Not Exist Help Option {currHelpOption}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        for (int i = 0; i < helpOptions.Count; i++)
            onExcuteLog?.Invoke(helpOptions[i].optionKey + " : " + helpOptions[i].description, ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
    }

    public void ExcuteQuestSaveTemp()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Save QuestList Temp , Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int tempIndex = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, tempIndex))
            return;

        QuestManager.Instance.SaveTemp(tempIndex);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Save QuestList Temp In  TempQuest{tempIndex}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteQuestLoadTemp()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Load QuestList Temp , Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }

        int tempIndex = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, tempIndex))
            return;

        bool success = QuestManager.Instance.LoadTemp(tempIndex);

        if(!success)
            onExcuteLog?.Invoke("Fail! Load Temp Quest!", ConsoleSystemLogType.SYSTEM_ERROR_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Load QuestList Temp In  TempQuest{tempIndex}";
            if (success)
                onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    #endregion

    #region Present
    public void ExcuteGetGlobalPresent()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Send Global Present Items Post, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        int type = int.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, type))
            return;              // 5 < 5
        if (globalPresentDB.GetList.Count <= type || globalPresentDB.GetList[type] == null)
        {
            onExcuteLog?.Invoke($"Error! Global Present {type} is Empty", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        globalPresentDB.GetList[type].Excute();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Sent Global Present Type {type}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }      //

    #endregion

    #region Camera
    public void ExcuteSetCameraSensitivity()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Change Camera Sensitivity. Example ) (Horizontal, Vertical) -> 10,10 , Current Sensitivity :  {GameManager.Instance.Cam.GetOriginCamSensitivity}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 2)
        {
            onExcuteLog?.Invoke("Error! Wrong format : Keep Vector2 -> float,float", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        Vector2 camSensitivity = Vector2.zero;
        camSensitivity.x = float.Parse(values[0].Trim());
        camSensitivity.y = float.Parse(values[1].Trim());

        GameManager.Instance.Cam.SetCamSensitivity(camSensitivity);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Sensitivity is H:{camSensitivity.x}, V:{camSensitivity.y}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteResetCameraSensitivity()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Origin Camera Sensitivity : Current Sensitivity :  {GameManager.Instance.Cam.GetOriginCamSensitivity}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Cam.ResetCamSensitivity();

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Sensitivity is H:{GameManager.Instance.Cam.GetCamSensitivity.x}, V:{GameManager.Instance.Cam.GetCamSensitivity.y}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteSetCameraOffset()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Set Camera Offset Position. Example ) Vector3 (X,Y,Z) -> 10,10,-10 ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 3)
        {
            onExcuteLog?.Invoke($"Error! Wrong format : Keep Vector3 -> float,float,float, Current Camera Offset Position : {GameManager.Instance.Cam.GetCamOffset}", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        Vector3 camPivotOffset = Vector3.zero;
        camPivotOffset.x = float.Parse(values[0].Trim());
        camPivotOffset.y = float.Parse(values[1].Trim());
        camPivotOffset.z = float.Parse(values[2].Trim());

        GameManager.Instance.Cam.SetCamOffset(camPivotOffset);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Offset is ( {camPivotOffset.x} , {camPivotOffset.y} , {camPivotOffset.z} )";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteResetCameraOffset()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Camera Offset Position : {GameManager.Instance.Cam.GetCamOffset}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Cam.ResetCamOffset();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Offset is ( {GameManager.Instance.Cam.GetCamOffset.x} " +
                                                          $", {GameManager.Instance.Cam.GetCamOffset.y}" +
                                                         $" , {GameManager.Instance.Cam.GetCamOffset.z} )";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteSetCameraPivotOffset()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Set Camera Pivot Offset. Example ) Vector3 (X,Y,Z) -> 10,10,-10 , Current Camera Pivot Offset : {GameManager.Instance.Cam.GetOriginCamPivotOffset}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 3)
        {
            onExcuteLog?.Invoke("Error! Wrong format : Keep Vector3 -> float,float,float", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }
        Vector3 camPivotOffset = Vector3.zero;
        camPivotOffset.x = float.Parse(values[0].Trim());
        camPivotOffset.y = float.Parse(values[1].Trim());
        camPivotOffset.z = float.Parse(values[2].Trim());

        GameManager.Instance.Cam.SetCamPivotOffset(camPivotOffset);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Pivot Offset is ( {camPivotOffset.x} , {camPivotOffset.y} , {camPivotOffset.z} )";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteResetCameraPivotOffset()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Camera Pivot Offset , Current Camera Pivot Offset : {GameManager.Instance.Cam.GetOriginCamPivotOffset}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Cam.ResetCamPivotOffset();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Camera Pivot Offset is ( {GameManager.Instance.Cam.GetCamPivotOffset.x} " +
                                                          $", {GameManager.Instance.Cam.GetCamPivotOffset.y}" +
                                                         $" , {GameManager.Instance.Cam.GetCamPivotOffset.z} )";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteSetCameraFOV()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Set Camera Field Of View, Current Cam FOV : {GameManager.Instance.Cam.GetCurrentFOV}, Range {currentProcessInfo.AllowRange[0]} ~ {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 2)
        {
            onExcuteLog?.Invoke("Error! Wrong format : Cam FOV,Lerp -> float,float", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        Vector2 fovValue = Vector2.zero;
        fovValue.x = float.Parse(values[0].Trim());
        fovValue.y = float.Parse(values[1].Trim());

        GameManager.Instance.Cam.SetCamFOV(fovValue.x, fovValue.y);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Current Camera Fov {GameManager.Instance.Cam.GetCurrentFOV}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteResetCameraFOV()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Camera Field Of View, Current Cam FOV : {GameManager.Instance.Cam.GetCurrentFOV}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Cam.ResetCameraFOV();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Reset! Current Camera Fov {GameManager.Instance.Cam.GetCurrentFOV}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteSetCameraLimitYRot()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Set Camera Limit Rotate Y, Current Limit Rotate Y : {GameManager.Instance.Cam.GetOriginLimitCamY}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        string[] values = currValueStr.Trim().Split(',');
        if (values == null || values.Length != 2)
        {
            onExcuteLog?.Invoke("Error! Wrong format : Cam Limit Y Rotate Min°,Max° -> float,float", ConsoleSystemLogType.SYSTEM_ERROR_LOG);
            return;
        }

        Vector2 limitRotYValue = Vector2.zero;
        limitRotYValue.x = float.Parse(values[0].Trim());
        limitRotYValue.y = float.Parse(values[1].Trim());

        GameManager.Instance.Cam.SetLimitCamYRot(limitRotYValue);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Current Camera Limit Y Rotation Min {limitRotYValue.x}°, Max {limitRotYValue.y}° ";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteResetCameraLimitYRot()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Camera Limit Rotate Y, Current Limit Rotate Y : {GameManager.Instance.Cam.GetOriginLimitCamY}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.Cam.ResetLimitCamYRot();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Reset! Current Camera Limit Y Rotation Min {GameManager.Instance.Cam.GetLimitYRot.x}°  , Max {GameManager.Instance.Cam.GetLimitYRot.y}°";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcutePrintCameraInfos()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Camera Infos", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        Vector2 sensitivity = GameManager.Instance.Cam.GetCamSensitivity;
        Vector2 limitYRot = GameManager.Instance.Cam.GetLimitYRot;
        Vector3 camOffet = GameManager.Instance.Cam.GetCamOffset;
        Vector3 camPivotOffet = GameManager.Instance.Cam.GetCamPivotOffset;
        float fov = GameManager.Instance.Cam.GetCurrentFOV;

        onExcuteLog?.Invoke("----- Current Camera Info -----", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Sensitivity H : {sensitivity.x} , V : {sensitivity.y}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"limitYRot Min : {limitYRot.x}°, Max : {limitYRot.y}°", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Cam Offet X : {camOffet.x} , Y : {camOffet.y} , Z : {camOffet.z}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Cam Pivot Offet X : {camPivotOffet.x} , Y : {camPivotOffet.y} , Z : {camPivotOffet.z}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"FOV  {fov}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke("-----------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Camera Infos ";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }               //
    #endregion

    #region Post Process
    public void ExcuteSetInitMotionBlur()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Set Init Post process Motion Blur , Current Value : {GameManager.Instance.MainPP.CurrentInitMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float initMB = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, initMB))
            return;

        GameManager.Instance.MainPP.SetInitMotionBlurShutterAngle(initMB);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Setting Init Motion Blur {GameManager.Instance.MainPP.CurrentInitMB}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }               //
    public void ExcuteSetDamageCurveMotionBlur(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_WEAK)
                onExcuteLog?.Invoke($"Set Post process Weak Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentWeakMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_NORMAL)
                onExcuteLog?.Invoke($"Set Post process Normal Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentNormalMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_STRONG)
            onExcuteLog?.Invoke($"Set Post process Strong Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentStrongMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float dmgMb = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, dmgMb))
            return;

        if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_WEAK) GameManager.Instance.MainPP.SetWeakMotionBlur(dmgMb);
        else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_NORMAL) GameManager.Instance.MainPP.SetNormalMotionBlur(dmgMb);
        else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_STRONG) GameManager.Instance.MainPP.SetStrongMotionBlur(dmgMb);
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Setting {(PPType)index} : {dmgMb}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }          //
    public void ExcuteResetDamageCurveMotionBlur(int index)
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_WEAK)
                onExcuteLog?.Invoke($"Reset Post process Weak Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentWeakMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_NORMAL)
                onExcuteLog?.Invoke($"Reset Post process Normal Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentNormalMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_STRONG)
                onExcuteLog?.Invoke($"Reset Post process Strong Damage Animation Curve Value Motion Blur, Current Value : {GameManager.Instance.MainPP.CurrentStrongMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float dmgMb = float.Parse(currValueStr.Trim());
        if (!CompareTwoAllowRange(currAllowRange, dmgMb))
            return;

        if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_WEAK) GameManager.Instance.MainPP.ResetWeakMotionBlur();
        else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_NORMAL) GameManager.Instance.MainPP.ResetNormalMotionBlur();
        else if ((PPType)index == PPType.MOTIONBLUR_DAMAGED_STRONG) GameManager.Instance.MainPP.ResetStrongMotionBlur();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Setting {(PPType)index}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }      //
    public void ExcuteResetInitMotionBlur()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Reset Post Process Init Motion Blur , Current Value : {GameManager.Instance.MainPP.CurrentInitMB}", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        GameManager.Instance.MainPP.ResetInitMotionBlur();
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Setting Init Motion Blur {GameManager.Instance.MainPP.CurrentInitMB}";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }                  //
    public void ExcutePrintMotionBlurInfo()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Print Post Process Motion Blur Values", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        float initBM = GameManager.Instance.MainPP.CurrentInitMB;
        float weakBM = GameManager.Instance.MainPP.CurrentWeakMB;
        float normalBM = GameManager.Instance.MainPP.CurrentNormalMB;
        float strongBM = GameManager.Instance.MainPP.CurrentStrongMB;

        onExcuteLog?.Invoke("----- Current Motion Blur Info -----", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Init Motion Blur : {initBM} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Weak Dmg Curve Motion Blur : {weakBM} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"Normal Dmg Curve Motion Blur : {normalBM} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke($"strong Dmg Curve Motion Blur : {strongBM} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
        onExcuteLog?.Invoke("-----------------------", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Print Motion Blur Infos ";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }                  //

    #endregion

    #region Test
    public void ExcuteCreateTestCamEnvironment()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Create Test Environment For Camera , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        if (boolValue)
        {
            if (GetCreatedTestObj(currentProcessInfo.Command) != null)
            {
                onExcuteLog?.Invoke($"Already Create Test Environment For Camera ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                return;
            }

            GameObject go = ObjectPooling.Instance.GetOBP(ObjectPoolingList.TestCamera_Environment.ToString());
            CreateObjInfo info = new CreateObjInfo();
            info.command = currentProcessInfo.Command;
            info.obpName = ObjectPoolingList.TestCamera_Environment.ToString();
            info.obp_Go = go;
            testObjects.Add(info);
            go.transform.position = GameManager.Instance.Player.transform.position;
        }
        else
        {
            CreateObjInfo info = GetCreatedTestObj(currentProcessInfo.Command);
            if(info != null)
            {
                testObjects.Remove(info);
                ObjectPooling.Instance.SetOBP(ObjectPoolingList.TestCamera_Environment.ToString(), info.obp_Go);
            }
        }

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Create Test Environment For Camera";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteCreateTestPlayerMoveEnvironment()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Create Test Environment For Player Move  , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        if (boolValue)
        {
            if (GetCreatedTestObj(currentProcessInfo.Command) != null)
            {
                onExcuteLog?.Invoke($"Already Create Test Environment For Player Move ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
                return;
            }

            GameObject go = ObjectPooling.Instance.GetOBP(ObjectPoolingList.TestPlayerMove_Environment.ToString());
            CreateObjInfo info = new CreateObjInfo();
            info.command = currentProcessInfo.Command;
            info.obpName = ObjectPoolingList.TestCamera_Environment.ToString();
            info.obp_Go = go;
            testObjects.Add(info);
            go.transform.position = GameManager.Instance.Player.transform.position;
        }
        else
        {
            CreateObjInfo info = GetCreatedTestObj(currentProcessInfo.Command);
            if (info != null)
            {
                testObjects.Remove(info);
                ObjectPooling.Instance.SetOBP(ObjectPoolingList.TestCamera_Environment.ToString(), info.obp_Go);
            }
        }

        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Create Test Environment For Player Move";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    public void ExcutePlayerDamage()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Test Player Damaged Range : weak, noraml, strong, flydown  ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        if (currValueStr.ToLower().Trim() == "weak")
            GameManager.Instance.Player.playerStats.Damaged(10, null, false, false, AttackStrengthType.WEAK,true);
        else if (currValueStr.ToLower().Trim() == "normal")
            GameManager.Instance.Player.playerStats.Damaged(10, null, false, false, AttackStrengthType.NORMAL, true);
        else if (currValueStr.ToLower().Trim() == "strong")
            GameManager.Instance.Player.playerStats.Damaged(10, null, false, false, AttackStrengthType.STRONG, true);
        else if (currValueStr.ToLower().Trim() == "flydown")
            GameManager.Instance.Player.playerStats.Damaged(10, null, false, false, AttackStrengthType.FLYDOWN, true);


        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player Damaged {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteCameraShake()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Test Camera Shake Range : weak_hit, normal_hit ,strong_hit ,skill_normal ,skill_strong" +
                $" ,dash ,zoomZ_short ,zoomZ_long", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        if (currValueStr.ToLower().Trim() == "weak_hit")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[0],true);
        else if (currValueStr.ToLower().Trim() == "normal_hit")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[1], true);
        else if (currValueStr.ToLower().Trim() == "strong_hit")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[2],true);
        else if (currValueStr.ToLower().Trim() == "skill_normal")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[3],true);
        else if (currValueStr.ToLower().Trim() == "skill_strong")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[4],true);
        else if (currValueStr.ToLower().Trim() == "dash")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[5],true);
        else if (currValueStr.ToLower().Trim() == "zoomz_short")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[6],true);
        else if (currValueStr.ToLower().Trim() == "zoomz_long")
            GameManager.Instance.Cam.ShakeCamera(shakeInfo[7],true);


        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! Player Damaged {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }
    public void ExcuteIgnoreDungeonEntryCondition()
    {
        if (!ValidateCurrHelpOption("-h", "-l")) return;

        if (CheckSameHelpOption("-h", currHelpOption))
        {
            onExcuteLog?.Invoke($"Excute Ignore Dungeon Entry Conditions , Range {currentProcessInfo.AllowRange[0]} , {currentProcessInfo.AllowRange[1]} ", ConsoleSystemLogType.SYSTEM_NORMAL_LOG);
            return;
        }
        if (!CompareStrings(currAllowRange, currValueStr))
            return;

        bool boolValue = bool.Parse(currValueStr.Trim());

        MapManager.Instance.IgnoreEntryConditions = boolValue;
        if (CheckSameHelpOption("-l", currHelpOption))
        {
            string log = $"Success! From Now on, Ignore Dungeon Entry Conditions {currValueStr.ToUpper()}!";
            onExcuteLog?.Invoke(log, ConsoleSystemLogType.SYSTEM_POSITIVE_LOG);
        }
    }

    #endregion

}




[System.Serializable]
public class ConsoleProcessInfo
{
    [SerializeField] private string command = string.Empty;
    [SerializeField] private int valueIndex = -1;
    [SerializeField] private string[] allowRange;
    [SerializeField] private UnityEvent onExcute;

    public string Command => command;
    public UnityEvent OnExcute => onExcute;
    public int ValueIndex => valueIndex;
    public string[] AllowRange => allowRange;
}
