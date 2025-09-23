using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboData
{
    [Header("0 : Left Click , 1 : Right Click")]
    public List<ComboMouseInputType> inputs;
    public ComboClip comboClip = null;
    [HideInInspector] public List<int> comboInput;

    public ComboData() { }
    public ComboData(ComboClip clip)
    {
        inputs = clip.comboInputKey;
        comboInput = clip.GetInputKeyInt();
        comboClip = clip;
    }

    public bool CheckSameInput(List<int> input)
    {
        for (int i = 0; i < comboInput.Count; i++)
        {
            if (comboInput.Count != input.Count) return false;
            else if (comboInput[i] != input[i])  return false;
        }

        return true;
    }
}

public class ComboController : MonoBehaviour
{
    public List<ComboData> combodatas = new List<ComboData>();
    private PlayerSkillController skillController = null;

    private void Start()
    {
        skillController = GetComponent<PlayerSkillController>();
        SettingClip(skillController);
    }

    public void SettingClip(PlayerSkillController skillController)
    {
        if (skillController == null) return;

        ComboSkillClip clip = skillController.GetUseSkillClip<ComboSkillClip>();
        clip?.UseRequipedSkill(skillController);

        if (clip == null)
        {
            ComboSkillClip tmpClip = skillController.OwnSkillDatabase.FindSkillType<ComboSkillClip>();
            clip = skillController.GetOwnSkillClip<ComboSkillClip>(tmpClip.ID);
            clip?.UseRequipedSkill(skillController);
        }
        combodatas = clip?.ComboDatas;
    }

    public ComboData GetComboData(List<int> inputs)
    {
        for (int i = 0; i < combodatas.Count; i++)
            if (combodatas[i].CheckSameInput(inputs))
                return combodatas[i];
        return null;
    }

    public bool FindHaveCombo(List<int> inputs)
    {
        for (int i = 0; i < combodatas.Count; i++)
            if (combodatas[i].CheckSameInput(inputs))
                return true;
        return false;
    }

    private void SetComboInput()
    {
        for (int i = 0; i < combodatas.Count; i++)
        {
            if (combodatas[i].comboClip == null) continue;
            combodatas[i].comboInput = combodatas[i].comboClip.GetInputKeyInt();
        }
    }

    public bool HaveFirstLeftAttack()
    {
        for (int i = 0; i < combodatas.Count; i++)
        {
            if (combodatas[i].comboInput.Count != 1) continue;
            if(combodatas[i].comboInput[0] == 0) return true;
        }
        return false;
    }

    public bool HaveFirstRightAttack()
    {
        for (int i = 0; i < combodatas.Count; i++)
        {
            if (combodatas[i].comboInput.Count != 1) continue;
            if (combodatas[i].comboInput[0] == 1) return true;
        }
        return false;
    }



    private void OnValidate()
    {
        SetComboInput();

    }


    //테스트용
    float time = 0;
    bool isStart = false;
    private void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
        }
        
    }

    public void TestAnimStart()
    {
        isStart = true;
        time = 0;
        //Debug.Log("공격시작 : " + time);
    }

    public void TestAnimTiming()
    {
       //Debug.Log("공격 타이밍 : " + time);
    }

    public void TestAnimEnd()
    {
       // Debug.Log("끝 : " + time);
        isStart = false;
    }
}
