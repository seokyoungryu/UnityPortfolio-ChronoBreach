using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDetailUI : MonoBehaviour
{
    [SerializeField] private BaseSkillClip selectedSkillClip = null;
    [SerializeField] private TextMeshProUGUI skillName_Text = null;
    [SerializeField] private Image selectSkill_Img = null;
    [SerializeField] private TextMeshProUGUI description_Text = null;
    [SerializeField] private TextMeshProUGUI condition_Text = null;
    [SerializeField] private Button accept_Btn = null;
    [SerializeField] private TextMeshProUGUI acceptBtn_Text = null;
    public bool activeWindow = false;

    public BaseSkillClip SelectedSkillClip { set { selectedSkillClip = value; } get { return selectedSkillClip; } }
    #region Events
    public delegate void OnAcceptBtn(SkillDetailUI skillDetailUI);
    public delegate void OnInit();


    public event OnAcceptBtn onAcceptBtn;
    public event OnInit onUpdateSlots;
    #endregion

    public void ClickSetting(BaseSkillClip skillClip)
    {
        if (skillClip == null) return;

        selectedSkillClip = skillClip;
        selectSkill_Img.sprite = skillClip.icon;
        UpdateText();
     }

    public void UpdateText()
    {
        if (selectedSkillClip == null) return;

        skillName_Text.text = selectedSkillClip.displayName;
        description_Text.text = selectedSkillClip.description;
        SetConditionText(selectedSkillClip.GetConditionDescriptions());
        if (selectedSkillClip.CurrentSkillUpgradeType == SkillUpgradeType.LOCK)
            acceptBtn_Text.text = "잠금 해제";
        else if (selectedSkillClip.CurrentSkillUpgradeType == SkillUpgradeType.UPGRADE)
            acceptBtn_Text.text = "업그레이드";
        else
            acceptBtn_Text.text = "MAX";
        GameManager.Instance.UpdateSkillInfo();
    }

    private void SetConditionText(string[] conditions)
    {
        if (selectedSkillClip == null) return;

        if (selectedSkillClip.currentSkillUpgradeType == SkillUpgradeType.LOCK)  condition_Text.text = "잠금 해제 조건\n";
        else if (selectedSkillClip.currentSkillUpgradeType == SkillUpgradeType.UPGRADE) condition_Text.text = "다음 업그레이드 조건\n";
        else if(selectedSkillClip.currentSkillUpgradeType == SkillUpgradeType.DONE)  condition_Text.text = "- M A X -\n";

        if (conditions == null || conditions.Length <= 0) return;
        for (int i = 0; i < conditions.Length; i++)
            condition_Text.text += conditions[i] + "\n";
    }


    public void Accept_Btn()
    {
        onAcceptBtn?.Invoke(this);
        UpdateText();
    }

   // public IEnumerator CheckCanSkillUpgrade(PlayerStateController controller)
   // {
   //     activeWindow = true;
   //     while (activeWindow)
   //     {
   //        // Debug.Log($"{selectedSkillClip.displayName} 스킬 가능 체크 : " + selectedSkillClip.CheckCanUpgrade(controller));
   //         if (selectedSkillClip != null)
   //         {
   //             if (selectedSkillClip.CheckCanUpgrade(controller))
   //                 accept_Btn.interactable = true;
   //             else
   //                 accept_Btn.interactable = false;
   //
   //             onUpdateSlots?.Invoke();
   //         }
   //         yield return new WaitForSeconds(0.2f);
   //     }
   //
   // }


    public void UpdateSkillInfos(PlayerStateController controller)
    {
        if (selectedSkillClip != null)
        {
            if (GameManager.Instance.ignoreSkillCondition)
                accept_Btn.interactable = true;
            else
            {
                if (selectedSkillClip.CheckCanUpgrade(controller))
                    accept_Btn.interactable = true;
                else
                    accept_Btn.interactable = false;
            }
            onUpdateSlots?.Invoke();
        }
    }

}
