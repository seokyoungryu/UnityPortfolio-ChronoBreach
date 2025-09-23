using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text name_text = null;
    [SerializeField] private TMP_Text currLv_text = null;
    [SerializeField] private TMP_Text requipLv_text = null;
    [SerializeField] private TMP_Text money_text = null;


    [SerializeField] private Color lvConditionColor;
    [SerializeField] private Color moneyConditionColor;


    public void SetInfo(BaseSkillClip clip)
    {
        if (clip == null)
        {
            return;
        }

        

        if (clip.currentSkillUpgradeType == SkillUpgradeType.LOCK) currLv_text.text = "현재 레벨 : 잠김 ";
        else if (clip.currentSkillLevel == clip.maxSkillLevel) currLv_text.text = "현재 레벨 :" + clip.currentSkillLevel.ToString() +$"(MAX)";
        else currLv_text.text = "현재 레벨 :" + clip.currentSkillLevel.ToString();


        int nextLv = clip.GetNextRequireLv();
        string lvCondition = string.Empty;

        int nextMoney = clip.GetNextRequireMoney();
        string moneyCondition = string.Empty;
        int nextSkillPoint = clip.GetNextRequireSkillPoint();

        if (GameManager.Instance.Player.playerStats.RemainingSkillPoint < nextSkillPoint)
            name_text.text = "<color=red>" + clip.displayName + "</color>";
        else
            name_text.text = clip.displayName;




        if (clip.currentSkillLevel == clip.maxSkillLevel)
        {
            lvCondition = "요구 레벨 : -";
            requipLv_text.text = lvCondition;
        }
        else
        {
            lvCondition = "요구 레벨 : " + nextLv;
            if (nextLv > GameManager.Instance.Player.playerStats.Level)
                requipLv_text.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(lvConditionColor)}>" + lvCondition + "</color>";
            else requipLv_text.text = $"<color=white>" + lvCondition + "</color>";

        }

        if (clip.currentSkillLevel == clip.maxSkillLevel)
        {
            moneyCondition = "요구 골드 : -";
            money_text.text = moneyCondition;
        }
        else
        {
            moneyCondition = "요구 골드 : " + nextMoney;
            if (nextMoney > GameManager.Instance.OwnMoney)
                money_text.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(moneyConditionColor)}>" + moneyCondition + "</color>";
            else money_text.text = $"<color=white>" + moneyCondition + "</color>";

        }

       

    }

}

