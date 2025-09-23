using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfomationUI : UIRoot
{
    #region OBP Name
    private const string normalTaskOBP = "ItemInfomationNormalTask";
    private const string smallTaskOBP = "ItemInfomationSmallTask";
    private const string largeTaskOBP = "ItemInfomationLargeTask";
    private const string borderLineTaskOBP = "ItemInfomationBorderLineTask";
    #endregion

    [SerializeField] protected float itemDetailTime = 0.5f;
    [SerializeField] private Image itemIcon_Img = null;
    [SerializeField] private TMP_Text itemName_Text = null;
    [SerializeField] private TMP_Text description_Text = null;
    [SerializeField] private TMP_Text unbreakable_Text = null;
    [SerializeField] private TMP_Text sell_Text = null;
    [SerializeField] private TMP_Text requireLv_Text = null;

    [Header("[0] : normal [1] : rare [2] : uniqe [3] : regendary")]
    [SerializeField] private Color[] potentialColor;


    private RectTransform rectTransform = null;
    private CustomVerticalLayoutGroup verticalLayoutGroup = null;

    public float ItemDetailTime => itemDetailTime;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        verticalLayoutGroup = GetComponent<CustomVerticalLayoutGroup>();
        CommonUIManager.Instance.playerInventory.onExcuteCloseWindowUI += CloseWindowUI;
    }


    public override void StartResetActive()
    {
        base.StartResetActive();
        gameObject.SetActive(false);

    }

    public void SettingItem(Item item, Vector3 mousePosition, ContainerType uiType)
    {
        ResetItems();
        if (item.itemClip.itemCategoryType == ItemCategoryType.EQUIPMENT)
            SettingEquipmentItem(item, uiType);
        else
            BaseInformation(item);

        InitRectTransform(GetComponentsInChildren<RectTransform>());
        verticalLayoutGroup.Excute();
        rectTransform.position = GetWindowPosition(mousePosition);

         Debug.Log(uiType + " : "  +item.objectName + " : " + item.itemClip.instanceID);

    }

    public void CloseWindowUI() => OnItemInformationPointerExit(null);


    public void ResetItems()
    {
        ItemInfomationTask[] tasks = GetComponentsInChildren<ItemInfomationTask>();
        for (int i = 0; i < tasks.Length; i++)
            ObjectPooling.Instance.SetOBP(tasks[i].OBPName, tasks[i].gameObject);
    }


    private void InitOBPRectSize(RectTransform rect)
    {
        RectTransform rectTr = rect.GetComponent<RectTransform>();
        rectTr.localScale = new Vector3(1f, 1f, 1f);
    }


    private void SettingEquipmentItem(Item item, ContainerType uiType)
    {
        BaseInformation(item);
        if (item.itemClip is WeaponItemClip)
            WeaponVariables(item.itemClip as WeaponItemClip, uiType);
        else if (item.itemClip is ArmorItemClip)
            ArmorVariables(item.itemClip as ArmorItemClip, uiType);
        else if (item.itemClip is AccessoryItemClip)
            AccessoryVariables(item.itemClip as AccessoryItemClip, uiType);
        else if (item.itemClip is TitleItemClip)
            TitleVariables(item.itemClip as TitleItemClip, uiType);
    }

    #region Window Position
    private Vector3 GetWindowPosition(Vector3 mousePosition)
    {
        int direction = GetMouseDirection(mousePosition);
        Vector3 retMousePosition = Vector3.zero;
        switch (direction)
        {
            case 1:
                retMousePosition = new Vector3(0, 0, 0);
                break;
            case 2:
                retMousePosition = new Vector3(-rectTransform.rect.width, 0, 0);
                break;
            case 3:
                retMousePosition = new Vector3(0, rectTransform.rect.height, 0);
                break;
            case 4:
                retMousePosition = new Vector3(-rectTransform.rect.width, rectTransform.rect.height);
                break;
        }

        // Debug.Log($"mousePosition({mousePosition}) + retMousePosition({retMousePosition}) = {mousePosition + retMousePosition} ");

        return mousePosition + retMousePosition;
    }

    private int GetMouseDirection(Vector3 mousePosition)
    {
        int retPos = 0;
        float widthHalf = Screen.width / 2;
        float heightHalf = Screen.height / 2;

        if (mousePosition.x < widthHalf && mousePosition.y >= heightHalf)
            retPos = 1;  //좌측 상단
        else if (mousePosition.x >= widthHalf && mousePosition.y >= heightHalf)
            retPos = 2;  //우측 상단
        else if (mousePosition.x < widthHalf && mousePosition.y < heightHalf)
            retPos = 3;  //좌측 하단
        else if (mousePosition.x >= widthHalf && mousePosition.y < heightHalf)
            retPos = 4;  // 우측 하단.

        // Debug.Log(retPos + " : 위치 ");

        return retPos;
    }

    #endregion

    #region Create Tasks 
    private void BaseInformation(Item item)
    {
        itemName_Text.text = item.itemClip.uiItemName;
        itemIcon_Img.sprite = item.itemClip.itemTexture;
        description_Text.text = item.itemClip.description;
        sell_Text.text = $"판매가 : {item.itemClip.sellCost}G";
        if (item.itemClip.isUnbreakable)
            unbreakable_Text.gameObject.SetActive(true);
        else
            unbreakable_Text.gameObject.SetActive(false);

        if (item.itemClip.itemCategoryType == ItemCategoryType.EQUIPMENT)
        {
            requireLv_Text.gameObject.SetActive(true);
            if (GameManager.Instance.Player.playerStats.Level >= item.itemClip.itemLevel)
                requireLv_Text.text = $"<color=white>요구 레벨 : Lv.{item.itemClip.itemLevel} </color>";
            else
                requireLv_Text.text = $"<color=red>요구 레벨 : Lv.{item.itemClip.itemLevel} </color>";
        }
        else
            requireLv_Text.gameObject.SetActive(false);
    }

    private ItemInfomationTask[] CommonEquipmentVariables(BaseItemClip clip, ContainerType uiType)
    {
        ItemInfomationTask[] tasks = new ItemInfomationTask[4];
        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();

        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)
            || uiType == ContainerType.DUNGEON_PORTALDETAIL || uiType == ContainerType.SCOREUI)
        {
            tasks[0].SettingText($"STR : +{clip.minStr} ~ {clip.maxStr}", clip.maxStr);
            tasks[1].SettingText($"DEX : +{clip.minDex} ~ {clip.maxDex}", clip.maxDex);
            tasks[2].SettingText($"LUK : +{clip.minLuc} ~ {clip.maxLuc}", clip.maxLuc);
            tasks[3].SettingText($"INT : +{clip.minInt} ~ {clip.maxInt}", clip.maxInt);
        }
        else
        {
            tasks[0].SettingText($"STR : +{clip.strength}", clip.strength);
            tasks[1].SettingText($"DEX : +{clip.dexterity}", clip.dexterity);
            tasks[2].SettingText($"LUK : +{clip.luck}", clip.luck);
            tasks[3].SettingText($"INT : +{clip.intelligence}", clip.intelligence);
        }


        return tasks;
    }

    private void WeaponVariables(WeaponItemClip clip, ContainerType uiType)
    {
        ItemInfomationTask[] tasks = new ItemInfomationTask[4];
        ItemInfomationTask borderLineTask = ObjectPooling.Instance.GetOBP(borderLineTaskOBP).GetComponent<ItemInfomationTask>();
        ItemInfomationTask sortTask = ObjectPooling.Instance.GetOBP(largeTaskOBP).GetComponent<ItemInfomationTask>();

        ItemInfomationTask[] statsTasks = CommonEquipmentVariables(clip, uiType);

        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();

        sortTask.SettingText("분류 : 무기", -1);
        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)
           || uiType == ContainerType.DUNGEON_PORTALDETAIL || uiType == ContainerType.SCOREUI)
        {
            tasks[0].SettingText($"공격력 : +{(int)clip.minAtk} ~ {(int)clip.maxAtk}", (int)clip.maxAtk);
            tasks[1].SettingText($"공격속도 : +{ (1 + clip.minAtkSpeed).ToString("F2")} ~ {(1 + clip.maxAtkSpeed).ToString("F2")}", clip.maxAtkSpeed);
            tasks[2].SettingText($"치명타 확률 : +{clip.minCriChance.ToString("F2")} ~ {clip.maxCriChance.ToString("F2")}", clip.maxCriChance);
            tasks[3].SettingText($"치명타 데미지 증가 : +{(clip.minCriDmg * 100f).ToString()} ~ {(clip.maxCriDmg * 100f).ToString()}", clip.maxCriDmg);
        }
        else
        {
            tasks[0].SettingText($"공격력 : +{(int)clip.atkValue}", (int)clip.atkValue);
            tasks[1].SettingText($"공격속도 : +{ (1 + clip.atkSpeed).ToString("F2")}", clip.atkSpeed);
            tasks[2].SettingText($"치명타 확률 : +{clip.criticalChance.ToString("F2")}", clip.criticalChance);
            tasks[3].SettingText($"치명타 데미지 증가 : +{(clip.criticalDamage * 100f).ToString()}", clip.criticalDamage);
        }


        Sorting(borderLineTask, sortTask, statsTasks[0], statsTasks[1], statsTasks[2], statsTasks[3]
            , tasks[0], tasks[1], tasks[2], tasks[3]);

        PotentialVariables(clip, uiType);
    }

    private void ArmorVariables(ArmorItemClip clip, ContainerType uiType)
    {
        ItemInfomationTask[] tasks = new ItemInfomationTask[5];
        ItemInfomationTask borderLineTask = ObjectPooling.Instance.GetOBP(borderLineTaskOBP).GetComponent<ItemInfomationTask>();
        ItemInfomationTask sortTask = ObjectPooling.Instance.GetOBP(largeTaskOBP).GetComponent<ItemInfomationTask>();

        ItemInfomationTask[] statsTasks = CommonEquipmentVariables(clip, uiType);

        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();

        string sortType = string.Empty;
        if (clip.armorType == ArmorType.HEAD) sortType = "머리";
        else if (clip.armorType == ArmorType.UPPER) sortType = "상체";
        else if (clip.armorType == ArmorType.LOWER) sortType = "하체";
        else if (clip.armorType == ArmorType.HAND) sortType = "손";
        else if (clip.armorType == ArmorType.LEG) sortType = "다리";

        sortTask.SettingText($"분류 : {sortType} 방어구", -1);

        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)
          || uiType == ContainerType.DUNGEON_PORTALDETAIL || uiType == ContainerType.SCOREUI)
        {
            tasks[0].SettingText($"방어력 : +{clip.minDef } ~ {clip.maxDef }", clip.maxDef);
            tasks[1].SettingText($"마법 방어력 : +{clip.minMagicDef } ~ {clip.maxMagicDef }", clip.maxMagicDef);
            tasks[2].SettingText($"회피율 : +{clip.minEvasion.ToString("F2") } ~ {clip.maxEvasion.ToString("F2") }", clip.maxEvasion);
            tasks[3].SettingText($"체력 리젠 : +{clip.minHpRegeneration.ToString("F2")} ~ {clip.maxHpRegeneration.ToString("F2") }", clip.maxHpRegeneration);
            tasks[4].SettingText($"스테미너 리젠 : +{clip.minStRegeneration.ToString("F2")} ~ {clip.maxStRegeneration.ToString("F2") }", clip.maxStRegeneration);
        }
        else
        {
            tasks[0].SettingText($"방어력 : +{clip.defense } ", clip.defense);
            tasks[1].SettingText($"마법 방어력 : +{clip.magicDefense }", clip.magicDefense);
            tasks[2].SettingText($"회피율 : +{clip.evasion.ToString("F2") }", clip.evasion);
            tasks[3].SettingText($"체력 리젠 : +{clip.healthRegeneration.ToString("F2") }", clip.healthRegeneration);
            tasks[4].SettingText($"스테미너 리젠 : +{clip.staminaRegeneration.ToString("F2")  }", clip.staminaRegeneration);
        }


        Sorting(borderLineTask, sortTask, statsTasks[0], statsTasks[1], statsTasks[2], statsTasks[3]
           , tasks[0], tasks[1], tasks[2], tasks[3], tasks[4]);

        PotentialVariables(clip, uiType);
    }

    private void AccessoryVariables(AccessoryItemClip clip, ContainerType uiType)
    {
        ItemInfomationTask[] tasks = new ItemInfomationTask[10];
        ItemInfomationTask borderLineTask = ObjectPooling.Instance.GetOBP(borderLineTaskOBP).GetComponent<ItemInfomationTask>();
        ItemInfomationTask sortTask = ObjectPooling.Instance.GetOBP(largeTaskOBP).GetComponent<ItemInfomationTask>();

        ItemInfomationTask[] statsTasks = CommonEquipmentVariables(clip, uiType);

        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();

        string sortType = string.Empty;
        if (clip.accessoryType == AccessoryType.BELT) sortType = "벨트";
        else if (clip.accessoryType == AccessoryType.BRACELET) sortType = "팔찌";
        else if (clip.accessoryType == AccessoryType.EARING) sortType = "귀걸이";
        else if (clip.accessoryType == AccessoryType.RING) sortType = "반지";
        else if (clip.accessoryType == AccessoryType.SOUL) sortType = "소울류";
        else if (clip.accessoryType == AccessoryType.CLOAK) sortType = "망토";

        sortTask.SettingText($"분류 : {sortType}", -1);

        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)
          || uiType == ContainerType.DUNGEON_PORTALDETAIL || uiType == ContainerType.SCOREUI)
        {
            tasks[0].SettingText($"올스탯 : +{clip.minAllStats} ~ {clip.maxAllStats}", clip.maxAllStats);
            tasks[1].SettingText($"공격력 : +{(int)clip.minAtk} ~ {(int)clip.maxAtk}", (int)clip.maxAtk);
            tasks[2].SettingText($"공격속도 : +{( 1 + clip.minAtkSpeed).ToString("F2")} ~ {(1 + clip.maxAtkSpeed).ToString("F2")}", clip.maxAtkSpeed);
            tasks[3].SettingText($"치명타 확률 : +{clip.minCriChance.ToString("F2")} ~ {clip.maxCriChance.ToString("F2")}", clip.maxCriChance);
            tasks[4].SettingText($"치명타 데미지 증가 : +{(clip.minCriDmg * 100f).ToString()} ~ {(clip.maxCriDmg* 100f).ToString()}", clip.maxCriDmg);
            tasks[5].SettingText($"방어력 : +{clip.minDef} ~ {clip.maxDef}", clip.maxDef);
            tasks[6].SettingText($"마법 방어력 : +{clip.minMagicDef} ~ {clip.maxMagicDef}", clip.maxMagicDef);
            tasks[7].SettingText($"회피율 : +{clip.minEvasion.ToString("F2") } ~ {clip.maxEvasion }", clip.maxEvasion);
            tasks[8].SettingText($"체력 리젠 : +{clip.minHpRegeneration.ToString("F2")} ~ {clip.maxHpRegeneration.ToString("F2") }", clip.maxHpRegeneration);
            tasks[9].SettingText($"스테미너 리젠 : +{clip.minStRegeneration.ToString("F2")} ~ {clip.maxStRegeneration.ToString("F2") }", clip.maxStRegeneration);
        }
        else
        {
            tasks[0].SettingText($"올스탯 : +{clip.allStats  }", clip.allStats);
            tasks[1].SettingText($"공격력 : +{(int)clip.atk }", (int)clip.atk);
            tasks[2].SettingText($"공격속도 : +{(1 + clip.atkSpeed).ToString("F2")  }", clip.atkSpeed);
            tasks[3].SettingText($"치명타 확률 : +{clip.critialChance.ToString("F2")  }", clip.critialChance);
            tasks[4].SettingText($"치명타 데미지 증가 : +{(clip.criticalDamage * 100f).ToString()  }", clip.criticalDamage);
            tasks[5].SettingText($"방어력 : +{clip.defense }", clip.defense);
            tasks[6].SettingText($"마법 방어력 : +{clip.magicDefense }", clip.magicDefense);
            tasks[7].SettingText($"회피율 : {clip.evasion.ToString("F2") }", clip.evasion);
            tasks[8].SettingText($"체력 리젠 : {clip.healthRegeneration.ToString("F2") }", clip.healthRegeneration);
            tasks[9].SettingText($"스테미너 리젠 : +{clip.staminaRegenaration.ToString("F2")  }", clip.staminaRegenaration);

        }


        Sorting(borderLineTask, sortTask, tasks[0], statsTasks[0], statsTasks[1], statsTasks[2], statsTasks[3]
           , tasks[1], tasks[2], tasks[3], tasks[4], tasks[5], tasks[6], tasks[7], tasks[8], tasks[9]);

        PotentialVariables(clip, uiType);
    }

    private void TitleVariables(TitleItemClip clip, ContainerType uiType)
    {
        ItemInfomationTask[] tasks = new ItemInfomationTask[9];
        ItemInfomationTask borderLineTask = ObjectPooling.Instance.GetOBP(borderLineTaskOBP).GetComponent<ItemInfomationTask>();
        ItemInfomationTask sortTask = ObjectPooling.Instance.GetOBP(largeTaskOBP).GetComponent<ItemInfomationTask>();

        ItemInfomationTask[] statsTasks = CommonEquipmentVariables(clip, uiType);

        for (int i = 0; i < tasks.Length; i++)
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();

        sortTask.SettingText($"분류 : 칭호", -1);

        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)
          || uiType == ContainerType.DUNGEON_PORTALDETAIL || uiType == ContainerType.SCOREUI)
        {
            tasks[0].SettingText($"올스탯 : +{clip.minAllStats} ~ {clip.maxAllStats}", clip.maxAllStats);
            tasks[1].SettingText($"공격력 : +{(int)clip.minAtk} ~ {(int)clip.maxAtk}", (int)clip.atk);
            tasks[2].SettingText($"공격속도 : +{ (1 + clip.minAtkSpeed).ToString("F2")} ~ {(1 + clip.maxAtkSpeed).ToString("F2")}", clip.atkSpeed);
            tasks[3].SettingText($"치명타 확률 : +{clip.minCriChance.ToString("F2")} ~ {clip.maxCriChance.ToString("F2")}", clip.critialChance);
            tasks[4].SettingText($"치명타 데미지 증가 : +{(clip.minCriDmg * 100f).ToString()} ~ {(clip.maxCriDmg * 100f).ToString()}", clip.criticalDamage);
            tasks[5].SettingText($"방어력 : +{clip.minDef} ~ {clip.maxDef}", clip.maxDef);
            tasks[6].SettingText($"마법 방어력 : +{clip.minMagicDef} ~ {clip.maxMagicDef}", clip.maxMagicDef);
            tasks[7].SettingText($"체력 리젠 : +{clip.minHpRegeneration.ToString("F2")} ~ {clip.maxHpRegeneration.ToString("F2") }", clip.maxHpRegeneration);
            tasks[8].SettingText($"스테미너 리젠 : +{clip.minStRegeneration.ToString("F2")} ~ {clip.maxStRegeneration.ToString("F2") }", clip.maxStRegeneration);
        }
        else
        {
            tasks[0].SettingText($"올스탯 : +{clip.allStats  }", clip.allStats);
            tasks[1].SettingText($"공격력 : +{(int)clip.atk }", (int)clip.atk);
            tasks[2].SettingText($"공격속도 : +{(1 + clip.atkSpeed).ToString("F2")  }", clip.atkSpeed);
            tasks[3].SettingText($"치명타 확률 : +{clip.critialChance.ToString("F2")  }", clip.critialChance);
            tasks[4].SettingText($"치명타 데미지 증가 : +{(clip.criticalDamage * 100f).ToString()  }", clip.criticalDamage);
            tasks[5].SettingText($"방어력 : +{clip.defense }", clip.defense);
            tasks[6].SettingText($"마법 방어력 : +{ 1 + clip.magicDefense }", clip.magicDefense);
            tasks[7].SettingText($"체력 리젠 : {clip.healthRegeneration.ToString("F2") }", clip.healthRegeneration);
            tasks[8].SettingText($"스테미너 리젠 : +{clip.staminaRegenaration.ToString("F2")  }", clip.staminaRegenaration);

        }

        Sorting(borderLineTask, sortTask, tasks[0], statsTasks[0], statsTasks[1], statsTasks[2], statsTasks[3]
           , tasks[1], tasks[2], tasks[3], tasks[4], tasks[5], tasks[6], tasks[7], tasks[8]);


        PotentialVariables(clip, uiType);
    }

    private void PotentialVariables(BaseItemClip clip, ContainerType uiType)
    {
        if (clip.potentialOptionCount <= 0 || clip.potentialRank == ItemPotentialRankType.NONE)
            return;

        if ((uiType == ContainerType.NPC_STORE && CommonUIManager.Instance.storeUI.CurrentCategory == ItemCountConfirmCategory.BUY)) 
            return;
        ItemInfomationTask[] tasks = new ItemInfomationTask[clip.potentialOptionCount];
        ItemInfomationTask borderLineTask = ObjectPooling.Instance.GetOBP(borderLineTaskOBP).GetComponent<ItemInfomationTask>();
        ItemInfomationTask sortTask = ObjectPooling.Instance.GetOBP(largeTaskOBP).GetComponent<ItemInfomationTask>();

        string potentialString = string.Empty;
        string htmlColor = string.Empty;

        if (clip.potentialRank == ItemPotentialRankType.NORMAL)
        {
            htmlColor = ColorUtility.ToHtmlStringRGB(potentialColor[0]);
            potentialString = $"<color=#{htmlColor}>잠재능력 : 노멀</color>";
        }
        if (clip.potentialRank == ItemPotentialRankType.RARE)
        {
            htmlColor = ColorUtility.ToHtmlStringRGB(potentialColor[1]);
            potentialString = $"<color=#{htmlColor}>잠재능력 : 레어</color>";
        }
        if (clip.potentialRank == ItemPotentialRankType.UNIQUE)
        {
            htmlColor = ColorUtility.ToHtmlStringRGB(potentialColor[2]);
            potentialString = $"<color=#{htmlColor}>잠재능력 : 유니크</color>";
        }
        if (clip.potentialRank == ItemPotentialRankType.LEGENDARY)
        {
            htmlColor = ColorUtility.ToHtmlStringRGB(potentialColor[3]);
            potentialString = $"<color=#{htmlColor}>잠재능력 : 레전더리</color>";
        }

        sortTask.SettingText(potentialString, -1);
        Sorting(borderLineTask, sortTask);
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = ObjectPooling.Instance.GetOBP(normalTaskOBP).GetComponent<ItemInfomationTask>();
            if (clip.ownPotential[i].clipData.isFloatValue)
                tasks[i].SettingText($"<color=#{htmlColor}>{clip.ownPotential[i].potentialName} : +{clip.ownPotential[i].potentialValue.ToString("0.0")}{clip.ownPotential[i].lastWord}</color>", clip.ownPotential[i].potentialValue);
            else
                tasks[i].SettingText($"<color=#{htmlColor}>{clip.ownPotential[i].potentialName} : +{clip.ownPotential[i].potentialValue}{clip.ownPotential[i].lastWord}</color>", clip.ownPotential[i].potentialValue);

            Sorting(tasks[i]);
        }

    }

    private void InitRectTransform(RectTransform[] rects)
    {
        for (int i = 0; i < rects.Length; i++)
            InitOBPRectSize(rects[i]);
    }

    private void Sorting(params ItemInfomationTask[] tasks)
    {
        for (int i = 0; i < tasks.Length; i++)
            tasks[i].transform.SetAsLastSibling();
    }
    #endregion


    public override void OpenUIWindow()
    {
        OpenUIWindowData();
    }

    public override void CloseUIWindow()
    {
        CloseUIWindowData();
    }
}
