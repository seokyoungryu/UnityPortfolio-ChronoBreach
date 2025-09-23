using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RequierdSkillSetting : StaticContainertUI
{
    private RequipedSkillSlot[] requipedSlots = new RequipedSkillSlot[2];

    #region Evenets
    public delegate PlayerSkillController GetSkillController();
    public event GetSkillController getSkillController;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        requipedSlots[0] = staticSlots[0].GetComponent<RequipedSkillSlot>();
        requipedSlots[1] = staticSlots[1].GetComponent<RequipedSkillSlot>();
      // this.gameObject.SetActive(false);
    }

    private void OnEnable() => InitSlotsSetting();
    private void OnDisable() => InitSlotsSetting();

    public void InitSlotsSetting()
    {
        PlayerSkillController skillController = getSkillController?.Invoke();
        if (skillController == null)
        {
            Debug.Log(gameObject.name + "  SKillController NUll! ! : " + GameManager.Instance.Player);
            skillController = GameManager.Instance.Player.skillController;
        }

        if (requipedSlots[0] == null)
        {
            requipedSlots[0] = staticSlots[0].GetComponent<RequipedSkillSlot>();
            requipedSlots[1] = staticSlots[1].GetComponent<RequipedSkillSlot>();
        }


        requipedSlots[0].RequipedClip = skillController.GetUseSkillClip<ComboSkillClip>();
        requipedSlots[1].RequipedClip = skillController.GetUseSkillClip<CounterSkillClip>();
        
        if (slotUIs.ContainsKey(staticSlots[0]) && requipedSlots[0].RequipedClip != null)
            slotUIs[staticSlots[0]].UpdateSlot(new Item(requipedSlots[0].RequipedClip), 1);
        if (slotUIs.ContainsKey(staticSlots[1]) && requipedSlots[1].RequipedClip != null)
            slotUIs[staticSlots[1]].UpdateSlot(new Item(requipedSlots[1].RequipedClip), 1);

    }



}

