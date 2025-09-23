using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : Singleton<EffectManager>
{
    private EffectData effectData = null;
    private Transform effectRoot = null;


    protected override void Awake()
    {
        base.Awake();
        if (effectData == null)
        {
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();
        }
    }

    public Vector3[] GetRandomValues(RandomEffectInfo info)
    {
        List<Vector3> retValues = new List<Vector3>();
        retValues.Add(ReturnPos(info));
        retValues.Add(ReturnRot(info));
        retValues.Add(ReturnScale(info));
        return retValues.ToArray();
    }

    public Vector3 ReturnPos(RandomEffectInfo info)
    {
        if(info.maxPosition == Vector3.zero)
          return info.minPosition;
        else
        return MathHelper.GetRandomVector3(info.minPosition, info.maxPosition);
    }
    public Vector3 ReturnRot(RandomEffectInfo info)
    {
        if (info.maxRotation == Vector3.zero)
            return info.minRotation;
        else
            return MathHelper.GetRandomVector3(info.minRotation, info.maxRotation);
    }
    public Vector3 ReturnScale(RandomEffectInfo info)
    {
        if (info.maxScale == Vector3.zero)
            return info.minScale;
        else
            return MathHelper.GetRandomVector3(info.minScale, info.maxScale);
    }


    public EffectData GetEffectData() => effectData;

    public GameObject GetEffectObjectRandom(RandomEffectInfo info, Vector3 position, Vector3 effectRotation, Vector3 effectScale)
    {
        if (info == null) return null;

        Vector3[] transform = GetRandomValues(info);
        return GetEffectObject(info.effect, position + transform[0], effectRotation + transform[1], effectScale + transform[2]);
    }

    public GameObject GetEffectObjectInfo(EffectInfo info, Vector3 position, Vector3 effectRotation, Vector3 effectScale)
    {
        if (info == null || info.effect == EffectList.None) return null;

        return GetEffectObject(info.effect, position + info.spawnPosition, effectRotation + info.spawnRotation, effectScale + info.spawnScale);
    }

    public GameObject GetEffectObject(EffectList list, Vector3 position, Vector3 effectRotation, Vector3 effectScale, bool isUIEffect = false)
    {
        if ( list == EffectList.None ) return null;

        GameObject retGo = ObjectPooling.Instance.GetEffectOBP((int)list);
   
        EffectClip clip = effectData.effectClips[(int)list];
        ReturnObjectToObjectPooling pool = retGo.GetComponent<ReturnObjectToObjectPooling>();
        if (pool == null)
        {
            pool = retGo.AddComponent<ReturnObjectToObjectPooling>();
            pool.returnTime = 1f;
        }

        if (retGo.activeSelf == false)
            retGo.SetActive(true);

        pool.objectPoolName = clip.effectName;
        if (isUIEffect)
        {
            retGo.transform.parent = CommonUIManager.Instance.uiEffectParent;
            retGo.GetComponent<RectTransform>().anchoredPosition =(Vector2)position;
        }
        else
        {
            retGo.transform.localPosition = position;
            retGo.transform.rotation = Quaternion.Euler(effectRotation);
        }


        if (clip.applyChildScale)
        {
            Transform[] childTrs = retGo.GetComponentsInChildren<Transform>();
            for (int i = 0; i < childTrs.Length; i++)
                childTrs[i].transform.localScale = effectScale;
        }
        else
            retGo.transform.localScale = effectScale;

        return retGo;
    }



}


[System.Serializable]
public class EffectInfo
{
    public SoundList effectSound;
    public EffectList effect;
    public Vector3 spawnPosition = Vector3.zero;
    public Vector3 spawnRotation = Vector3.zero;
    public Vector3 spawnScale = Vector3.one;
}


[System.Serializable]
public class ControllerEffectInfo : EffectInfo
{
    public float effectFrame = 0f;
    public ControllerInPosition spawnType = ControllerInPosition.DAMAGED;

}

[System.Serializable]
public class RandomEffectInfo
{
    public SoundList effectSound;
    public EffectList effect;
    public Vector3 minPosition = Vector3.zero;
    public Vector3 maxPosition = Vector3.zero;
    public Vector3 minRotation = Vector3.zero;
    public Vector3 maxRotation = Vector3.zero;
    public Vector3 minScale = Vector3.one;
    public Vector3 maxScale = Vector3.one;

}