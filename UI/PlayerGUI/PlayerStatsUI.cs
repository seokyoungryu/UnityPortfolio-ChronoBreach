using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum StatInformaionUIIndex
{
    NAME = 0,
    TITLE = 1,
    LV = 2,
    EXP = 3,
    REPUTATION = 4,
    STATPOINT = 5,
    STR = 6,
    DEX = 7,
    LUC = 8,
    INT = 9,
}
public enum StatDetailInfoIndex
{
    TOTALDAMAGE = 0,
    SKILLDAMAGE = 1,
    HP = 2,
    STAMINA = 3,
    ATK = 4,
    ATKSPEED = 5,
    DEFENSE = 6,
    MAGICDEFENSE = 7,
    CRITICALCHANCE = 8,
    CRITICALDAMAGE = 9,
    EVASION = 10,
    HP_REGEN = 11,
    STAMINA_REGEN = 12,

}


public class PlayerStatsUI : UIRoot
{
    [Header("Detail Information UI")]
    [SerializeField] private GameObject detailUI = null;

    [Header("[0]:Name,[1]:Title,[2]:Lv,[3]:Exp,[4]:Reputation,[5]:Statpoint")]
    [Header("[6]:Str,[7]:Dex,[8]:Luc,[9]:Int")]
    [SerializeField] private TMP_Text[] statInformation_Text;

    [Header("[0]:최종데미지,[1]:Hp,[2]:Stamina,[3]:Stat Damage,[4]:Skill Damage")]
    [Header("[5]:AtkSpeed,[6]:Defense,[7]:MagicDef,[8]:CriChance,[9]:CriDmg,[10]:Evasion")]
    [SerializeField] private TMP_Text[] detailInformation_Text;

    [Header("Plus Stats Text - [0]:Str,[1]:Dex,[2]:Luc,[3]:Int")]
    [SerializeField] private TMP_Text[] plusStatsTexts;
    [SerializeField] private Button[] plusBtns;

    [Header("[0]:Apply,[1]:Cancel Button")]
    [SerializeField] private Button[] functionBtns;


    private PlayerStatus playerStatus;

    private bool isInitTempStat = false;
    private bool isOpendDetailUI = false;



    protected override void Start()
    {
       // DisableFunctionBtns();
    }

    public override void StartResetActive()
    {
        base.StartResetActive();
        gameObject.SetActive(false);
    }

    public override void OpenUIWindow()
    {
        base.OpenUIWindow();

        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;

        if (playerStatus != null)
        {
            Debug.Log("Stats창 열림");
            UpdateStatsUIs(playerStatus);
        }
    }

    public override void CloseUIWindow()
    {
        base.CloseUIWindow();
        if (playerStatus.RemainingStatPoint > 0)
            CancelTempStat();
        DisablePlusBtns();
        DisableStatText();
    }

    #region Stats Information 
    public void UpdateStatsUIs(PlayerStatus playerStatus)
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;

        playerStatus.UpdateStats();

        if (playerStatus.RemainingStatPoint > 0)
        {
            EnableStatsPointUI();
        }
        else
            DisableStatsPointUI();
    }

    private void EnableStatsPointUI()
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;

        if (playerStatus.RemainingStatPoint <= 0) return;

        UpdateStatInformationDatas(playerStatus);
        EnablePlusBtns();
        EnableFunctionBtns();
    }

    private void DisableStatsPointUI()
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;

        if (playerStatus.RemainingStatPoint > 0) return;
        if (playerStatus.TempRemainingStatPoint <= 0)
        {
            DisablePlusBtns();
            DisableStatText();
            DisableFunctionBtns();
        }
        
    }

    public void UpdateStatInformationDatas(PlayerStatus playerStatus)
    {
        statInformation_Text[(int)StatInformaionUIIndex.NAME].text = "이름임";
        statInformation_Text[(int)StatInformaionUIIndex.TITLE].text = GameManager.Instance.title;
        statInformation_Text[(int)StatInformaionUIIndex.LV].text = playerStatus.Level.ToString();
        statInformation_Text[(int)StatInformaionUIIndex.EXP].text 
            = playerStatus.CurrentExp + " / " + playerStatus.NextExp.RequiredExp;
        statInformation_Text[(int)StatInformaionUIIndex.REPUTATION].text = GameManager.Instance.Reputation.ToString();
        statInformation_Text[(int)StatInformaionUIIndex.STATPOINT].text = playerStatus.RemainingStatPoint.ToString();
        statInformation_Text[(int)StatInformaionUIIndex.STR].text 
            = playerStatus.TotalStrength + " (" + playerStatus.OriginStrength + "+" +( playerStatus.ExtraStrength + playerStatus.TotalAllStats) + ")";
        statInformation_Text[(int)StatInformaionUIIndex.DEX].text 
            = playerStatus.TotalDexterity + " (" + playerStatus.OriginDexterity + "+" + (playerStatus.ExtraDexterity + playerStatus.TotalAllStats )+")";
        statInformation_Text[(int)StatInformaionUIIndex.LUC].text
            = playerStatus.TotalLuck + " (" + playerStatus.OriginLuck + "+" + (playerStatus.ExtraLuck + playerStatus.TotalAllStats) + ")";
        statInformation_Text[(int)StatInformaionUIIndex.INT].text
            = playerStatus.TotalIntelligence + " (" + playerStatus.OriginIntelligence + "+" + (playerStatus.ExtraIntelligence + playerStatus.TotalAllStats )+")";
    }

    public void TempStatsUp(int index)
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;
        if (playerStatus.RemainingStatPoint <= 0) return;
        if (!isInitTempStat) InitTempStat(playerStatus);

        switch (index)
        {
            case (int)StatInformaionUIIndex.STR:
                EnableStatText(0);
                playerStatus.AddTempStr(1);
                plusStatsTexts[0].text = "+" + playerStatus.TempSTR.ToString();
                break;
            case (int)StatInformaionUIIndex.DEX:
                EnableStatText(1);
                playerStatus.AddTempDex(1);
                plusStatsTexts[1].text = "+" + playerStatus.TempDex.ToString();
                break;
            case (int)StatInformaionUIIndex.LUC:
                EnableStatText(2);
                playerStatus.AddTempLuc(1);
                plusStatsTexts[2].text = "+" + playerStatus.TempLuc.ToString();
                break;
            case (int)StatInformaionUIIndex.INT:
                EnableStatText(3);
                playerStatus.AddTempInt(1);
                plusStatsTexts[3].text = "+" + playerStatus.TempInt.ToString();
                break;
        }
        playerStatus.UseStatsPoint(1);
        UpdateStatInformationDatas(playerStatus);
        playerStatus.UpdateStats();
        SoundManager.Instance.PlayUISound(UISoundType.STATS_BTN);

        if (playerStatus.RemainingStatPoint <= 0)
            DisablePlusBtns();
    }

    private void InitTempStat(PlayerStatus playerStatus)
    {
        isInitTempStat = true;
        playerStatus.TempRemainingStatPoint = playerStatus.RemainingStatPoint;
    }

    private void EnableStatText(int index)
    {
       // if (plusStatsTexts[index].gameObject.activeInHierarchy == false)
            plusStatsTexts[index].gameObject.SetActive(true);
    }
    private void DisableStatText()
    {
        for (int i = 0; i < plusStatsTexts.Length; i++)
        {
            plusStatsTexts[i].text = "";
            plusStatsTexts[i].gameObject.SetActive(false);
        }
    }

    private void EnableFunctionBtns()
    {
        for (int i = 0; i < functionBtns.Length; i++)
            functionBtns[i].gameObject.SetActive(true);

        if (!functionBtns[0].gameObject.activeInHierarchy)
            for (int i = 0; i < functionBtns.Length; i++)
                functionBtns[i].gameObject.SetActive(true);

        for (int i = 0; i < functionBtns.Length; i++)
            Debug.Log("Enable Fun : " + functionBtns[i].gameObject.name + " Active self : " + functionBtns[i].gameObject.activeInHierarchy);
    }
    private void DisableFunctionBtns()
    {
        for (int i = 0; i < functionBtns.Length; i++)
            functionBtns[i].gameObject.SetActive(false);
    }

    private void EnablePlusBtns()
    {
        for (int i = 0; i < plusBtns.Length; i++)
            plusBtns[i].gameObject.SetActive(true);
    }
    private void DisablePlusBtns()
    {
        for (int i = 0; i < plusBtns.Length; i++)
            plusBtns[i].gameObject.SetActive(false);
    }


    public void ApplyTempStat()
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;
        if (playerStatus.GetTempStatsCount() <= 0) return;

        isInitTempStat = false;
        SoundManager.Instance.PlayUISound(UISoundType.STATS_APPLY);
        playerStatus.ApplyTempStats();
        UpdateStatsUIs(playerStatus);
        UpdateDetailStatsInfos(playerStatus);
        DisableStatText();

        if (playerStatus.RemainingStatPoint <= 0)
            UpdateStatsUIs(playerStatus);

    }

    public void CancelTempStat()
    {
        if (playerStatus == null)
            playerStatus = GameManager.Instance.Player.playerStats;
        if (playerStatus.GetTempStatsCount() <= 0) return;

        playerStatus.ResetTempStats();
        SoundManager.Instance.PlayUISound(UISoundType.STATS_CENCEL); 
        UpdateStatsUIs(playerStatus);
        DisableStatText();
        for (int i = 0; i < plusStatsTexts.Length; i++)
            EnableStatText(i);

        UpdateDetailStatsInfos(playerStatus);
    }

    #endregion


    #region Detail Stats Information
    public void OpenCloseDetailInformationUI()
    {
        isOpendDetailUI = !isOpendDetailUI;
        if (isOpendDetailUI)
            detailUI.SetActive(false);
        else
            detailUI.SetActive(true);
    }


    public void UpdateDetailStatsInfos(PlayerStatus playerStatus)
    {
        detailInformation_Text[(int)StatDetailInfoIndex.TOTALDAMAGE].text 
            = playerStatus.GetMinDamage(true).ToString("F0") + " ~ " + playerStatus.GetMaxDamage(true).ToString("F0");
        detailInformation_Text[(int)StatDetailInfoIndex.SKILLDAMAGE].text 
            = playerStatus.GetMinSkillDamage(true).ToString("F0") + " ~ " + playerStatus.GetMaxSkillDamage(true).ToString("F0");
        detailInformation_Text[(int)StatDetailInfoIndex.HP].text = (playerStatus.TotalHealth + (playerStatus.TempDex * 4)).ToString();
        detailInformation_Text[(int)StatDetailInfoIndex.STAMINA].text = playerStatus.TotalStamina.ToString();
        detailInformation_Text[(int)StatDetailInfoIndex.ATK].text = playerStatus.TotalAtk.ToString();
        detailInformation_Text[(int)StatDetailInfoIndex.ATKSPEED].text = playerStatus.TotalAtkSpeed.ToString("F3");
        detailInformation_Text[(int)StatDetailInfoIndex.DEFENSE].text = (playerStatus.TotalDefense + (playerStatus.TempDex)).ToString();
        detailInformation_Text[(int)StatDetailInfoIndex.MAGICDEFENSE].text = (playerStatus.TotalMagicDefense + (playerStatus.TempLuc * 0.7f)).ToString();
        detailInformation_Text[(int)StatDetailInfoIndex.CRITICALCHANCE].text = (playerStatus.TotalCriChance + (playerStatus.TempLuc * 0.06f)).ToString("F2");
        detailInformation_Text[(int)StatDetailInfoIndex.CRITICALDAMAGE].text = ((1 + playerStatus.TotalCriDmg) * 100).ToString("F2");
        detailInformation_Text[(int)StatDetailInfoIndex.EVASION].text = (playerStatus.TotalEvasion + (playerStatus.TempLuc * 0.04f)).ToString("F2");
        detailInformation_Text[(int)StatDetailInfoIndex.HP_REGEN].text = playerStatus.TotalHpRegenPerSec.ToString("F2");
        detailInformation_Text[(int)StatDetailInfoIndex.STAMINA_REGEN].text = playerStatus.TotalStaminaRegenPerSec.ToString("F2");

    }

    #endregion
}
