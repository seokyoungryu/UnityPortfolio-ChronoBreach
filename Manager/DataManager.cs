using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private EffectData effectData = null;
    private SoundData soundData = null;
    private PotentialData potentialData = null;
    private ItemData itemData = null;
    private AIInfoData aiInfoData = null;

    protected override void Awake()
    {
        base.Awake();
        InitData();
    }


    private void InitData()
    {
        if (potentialData == null)
        {
            potentialData = ScriptableObject.CreateInstance<PotentialData>();
            potentialData.LoadData();
        }
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
        if (itemData == null)
        {
            itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.LoadData();
        }
        if (aiInfoData == null)
        {
            aiInfoData = ScriptableObject.CreateInstance<AIInfoData>();
            aiInfoData.LoadData();
        }
    }


    #region Get Data
    public EffectData GetEffectData()
    {
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
        return effectData;
    }

    public SoundData GetSoundData()
    {
        if (soundData == null)
        {
            soundData = ScriptableObject.CreateInstance<SoundData>();
            soundData.LoadData();
        }
        return soundData;
    }

    public PotentialData GetPotentialData()
    {
        if (potentialData == null)
        {
            potentialData = ScriptableObject.CreateInstance<PotentialData>();
            potentialData.LoadData();
        }
        return potentialData;
    }

    public ItemData GetItemData()
    {
        if (itemData == null)
        {
            itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.LoadData();
        }
        return itemData;
    }

    public AIInfoData GetAIInfoData()
    {
        if (aiInfoData == null)
        {
            aiInfoData = ScriptableObject.CreateInstance<AIInfoData>();
            aiInfoData.LoadData();
        }
        return aiInfoData;
    }

    #endregion
}
