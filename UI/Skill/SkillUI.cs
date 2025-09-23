using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillUI : StaticContainertUI
{
    [SerializeField] private SkillDetailUI skillDetailUI = null;
    [SerializeField] private List<BaseSkillClip> skills = new List<BaseSkillClip>();
    [SerializeField] private int skillUIIndex = -1;
    public List<BaseSkillClip> Skills => skills;

    public int SkillUIIndex { get { return skillUIIndex; } set { skillUIIndex = value; } }
    private List<SkillSlot> skillSlots = new List<SkillSlot>();

    #region Events
    public delegate SkillData OnSkillClick(int skillID);
    public delegate PlayerStateController GetPlayerController();
    public delegate RequierdSkillSetting GetRequipedSkillSetting();


    public event GetRequipedSkillSetting getRequipedSkillSetting;
    public event GetPlayerController getPlayerController;
    public event OnSkillClick onStartDrag;
    public event OnSkillClick onSkillClick;
    #endregion

    protected override void Awake()
    {
        inventoryObject.Clear();     
        base.Awake();
        uIType = ContainerType.SKILL;
        //Debug.Log("Skill Inven :" + inventoryObject.name);
    }

    protected override void CreateSlotUIs()
    {
        if (staticSlots == null) return;
        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            if ((staticSlots.Length - 1) < i) return;

            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;
            GameObject go = staticSlots[i];

            // 0 - detail  1 - dragSlot
            UIHelper.AddEventTrigger(go.transform.GetChild(0).gameObject, EventTriggerType.PointerClick, delegate { DetailOnPointerClick(go); });

            UIHelper.AddEventTrigger(go.transform.GetChild(1).gameObject, EventTriggerType.PointerClick, delegate { DetailOnPointerClick(go); });
            UIHelper.AddEventTrigger(go.transform.GetChild(1).gameObject, EventTriggerType.PointerEnter, delegate { OnPointEnter(go); });
            UIHelper.AddEventTrigger(go.transform.GetChild(1).gameObject, EventTriggerType.PointerExit, delegate { OnPointExit(go); });
            SlotFunction(go);

            //Debug.Log(i + " 스킬등록  : " + skills[i].codeName);
            Item item = new Item(skills[i]);
            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].UpdateSlot(item, 1);
            slotUIs.Add(go, inventoryObject.slots[i]);
            skillSlots.Add(go.GetComponent<SkillSlot>());
        }
    }
              
    public void UpdateSkillSlotInfo()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            BaseSkillClip clip = GetPlayerSkillData(skills[i].ID).skillClip;
            skillSlots[i].SetInfo(clip);
        }
    }

    protected override void OnPostUpdate(InventorySlot slot)
    {
        if (slot.item.id < 0)
            return;
        BaseSkillClip clip = slot.item.skillClip;
        GameObject dragSlot = slot.slotUI.transform.GetChild(1).gameObject;

        dragSlot.transform.GetChild(1).GetComponent<Image>().sprite = clip.icon;
        dragSlot.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        dragSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";

        //Debug.Log("업데이트 : " + slot.objectName);
    }

    protected override void SlotFunction(GameObject slot)
    {
        if (uIType != ContainerType.SKILL) return;

        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.BeginDrag, delegate { OnStartDrag(slot); });
        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.Drag, delegate { OnDraging(slot); });
        UIHelper.AddEventTrigger(slot.transform.GetChild(1).gameObject, EventTriggerType.EndDrag, delegate { OnEndDrag(slot, inventoryObject); });
    }


    protected override void OnStartDrag(GameObject go)
    {
        if (go.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<Image>()?.sprite == null) return;
        if (slotUIs[go].item.skillClip is PassiveSkillClip) return;
        if (slotUIs[go].item.skillClip is ComboSkillClip || slotUIs[go].item.skillClip is CounterSkillClip)
            getRequipedSkillSetting?.Invoke().gameObject.SetActive(true);


        BaseSkillClip skillClip = onStartDrag?.Invoke(slotUIs[go].item.skillClip.ID)?.skillClip;
        if (skillClip.ID == -1 || skillClip.skillState == CurrentSkillState.LOCK)
        {
            CommonUIManager.Instance.ExcuteGlobalSimpleNotifer("스킬이 잠금되어 있습니다.");
            return;
        }

        MouseUIData.tempDraggingImage = CreateDragImage(go);
        MouseUIData.dragSlot = go;
    }


    protected override void OnEndDrag(GameObject go, InventoryObject inventory)
    {
        base.OnEndDrag(go, inventory);
        if (MouseUIData.tempDraggingImage == null) return;
        Destroy(MouseUIData.tempDraggingImage);

        if (MouseUIData.enterUIRoot != null && MouseUIData.enterUIRoot.uIType == ContainerType.QUICK
            && MouseUIData.enterSlot != null)
        {
            Debug.Log("퀵 세팅!");
            if (slotUIs[go].item.skillClip is ComboSkillClip || slotUIs[go].item.skillClip is CounterSkillClip)
            {
                CommonUIManager.Instance.ExcuteGlobalNotifer("필수스킬 세팅창이 아닙니다.");
                return;
            }
            else
            {
                
                QuickUI quickUI = MouseUIData.enterUIRoot.GetComponent<QuickUI>();
                quickUI.slotUIs[MouseUIData.enterSlot].UpdateSlot(slotUIs[MouseUIData.dragSlot].item, 1);
            }
        }
        else if (MouseUIData.enterUIRoot != null && MouseUIData.enterUIRoot.uIType == ContainerType.REQUIPEDSKILLSETTING
            && MouseUIData.enterSlot != null)
        {
            RequipedSkillSlot requipedSlot = MouseUIData.enterSlot.GetComponent<RequipedSkillSlot>();
            if (slotUIs[MouseUIData.dragSlot].item.skillClip is ComboSkillClip && requipedSlot.RequipedSkillType == RequipedSkillType.COMBO)
            {
                EquipComboSkill(requipedSlot, go);
                getRequipedSkillSetting?.Invoke().slotUIs[MouseUIData.enterSlot].UpdateSlot(new Item(slotUIs[go].item.skillClip), 1);
            }
            else if (slotUIs[MouseUIData.dragSlot].item.skillClip is CounterSkillClip && requipedSlot.RequipedSkillType == RequipedSkillType.COUNTER)
            {
                EquipCounterSkill(requipedSlot, go);
                getRequipedSkillSetting?.Invoke().slotUIs[MouseUIData.enterSlot].UpdateSlot(new Item(slotUIs[go].item.skillClip), 1);
            }

        }


        MouseUIData.dragSlot = null;
        //requierdSkillSettingUI.gameObject.SetActive(false);
    }

    public SkillData GetPlayerSkillData(int skillID)
    {
        SkillData data = onSkillClick?.Invoke(skillID);
        return data;
    }

    private void DetailOnPointerClick(GameObject go)
    {
        Debug.Log("클릭 : " + slotUIs[go].item.skillClip);
        SkillData skillData = onSkillClick?.Invoke(slotUIs[go].item.skillClip.ID);

        if (skillData != null)
            skillDetailUI.ClickSetting(skillData.skillClip);

    }


   //protected override void InterfaceOnPointerEnter(GameObject go) { }
   //
   //protected override void InterfaceOnPointerExit(GameObject go) { }


    private void EquipComboSkill(RequipedSkillSlot requipedSlot, GameObject slot)
    {
        PlayerStateController playerController = getPlayerController?.Invoke();
        PlayerSkillController skillController = playerController.skillController;
        ComboSkillClip comboClip = slotUIs[slot].item.skillClip as ComboSkillClip;
        requipedSlot.RequipedClip = comboClip;
        skillController.GetOwnSkillClip<ComboSkillClip>(comboClip.ID).UseRequipedSkill(skillController);
        playerController.comboController.SettingClip(playerController.skillController);

    }

    private void EquipCounterSkill(RequipedSkillSlot requipedSlot, GameObject slot)
    {
        PlayerStateController playerController = getPlayerController?.Invoke();
        PlayerSkillController skillController = playerController.skillController;
        CounterSkillClip counterClip = slotUIs[slot].item.skillClip as CounterSkillClip;
        requipedSlot.RequipedClip = counterClip;
        skillController.GetOwnSkillClip<CounterSkillClip>(counterClip.ID).UseRequipedSkill(skillController);
        playerController.GetState<CounterAttackState>().SettingClip();
    }

}
