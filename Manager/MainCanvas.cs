using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : Singleton<MainCanvas>
{
    [SerializeField] private RectTransform rootCanvas = null;
    [SerializeField] private SkillUIContainer skillUIContainer;
    [SerializeField] private SkillDetailUI skillDetailUI;
    [SerializeField] private QuickSlotContainer quickSlotContainer;
    [SerializeField] private RequierdSkillSetting requierdSkill;

    public RectTransform RootCanvas => rootCanvas;

    public SkillUIContainer SkillUIContainer => skillUIContainer;
    public SkillDetailUI SkillDetailUI => skillDetailUI;
    public QuickSlotContainer QuickSlotContainer => quickSlotContainer;
    public RequierdSkillSetting RequierdSkill => requierdSkill;

}

