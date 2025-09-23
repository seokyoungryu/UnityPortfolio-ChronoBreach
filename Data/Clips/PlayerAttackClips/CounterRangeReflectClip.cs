using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Attack Data/Counter Range Reflect Clip")]
public class CounterRangeReflectClip : ScriptableObject
{
    private ProjectileCreator upgradeRangeInfo = null;
    private int currentUpgradeIndex = -1;

    [Header("Animation")]
    public AnimationClip reflectCounterAnim = null;
    [SerializeField] private string animName = "";
    [SerializeField] private int fullFrame = 0;
    [SerializeField] private int reflectTimingFrame = 0;
    [SerializeField] private int endAnimFrame = 0;

    [SerializeField] private List<UpgradeCounterReflect> upgrades = new List<UpgradeCounterReflect>();
    public string AnimName => animName;

    public int CurrentUpgradeIndex => currentUpgradeIndex;
   // public RandomEffectInfo ProjectileEffect => upgradeRangeInfo?.projectileEffect;
    public ProjectileCreator Info => upgradeRangeInfo;
    public int ReflectTimingFrame => reflectTimingFrame;
    public int EndAnimFrame => endAnimFrame;

    public CounterRangeReflectClip() { }
    public CounterRangeReflectClip(CounterRangeReflectClip clip)
    {
        upgradeRangeInfo = clip.upgradeRangeInfo;
        currentUpgradeIndex = clip.currentUpgradeIndex;
        reflectCounterAnim = clip.reflectCounterAnim;
        animName = clip.animName;
        fullFrame = clip.fullFrame;
        reflectTimingFrame = clip.reflectTimingFrame;
        endAnimFrame = clip.endAnimFrame;
        upgrades = clip.upgrades;
    }


    public void Upgrade(int upgradeIndex)
    {
        if (upgradeIndex < 0 || upgradeIndex >= upgrades.Count) return;

        currentUpgradeIndex = upgrades[upgradeIndex].upgradeLv;
        upgradeRangeInfo = upgrades[upgradeIndex].upgradeRangeInfo;
       // Debug.Log("Upgrade " + upgradeIndex + " - " + upgradeRangeInfo);
    }


    private void OnValidate()
    {
        if(reflectCounterAnim != null)
        {
            fullFrame = (int)(reflectCounterAnim.length * reflectCounterAnim.frameRate);
            if (endAnimFrame == 0)
                endAnimFrame = fullFrame;
        }

        if(upgrades.Count > 0)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].upgradeName = "Upgrade " + (1+i);
                upgrades[i].upgradeLv = 1 + i;
            }
        }
    }

}

[System.Serializable]
public class UpgradeCounterReflect
{
    public string upgradeName = "";
    [HideInInspector] public int upgradeLv = 0;
    public ProjectileCreator upgradeRangeInfo = null;
}
