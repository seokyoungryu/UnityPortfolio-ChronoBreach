using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Attack Data/Projectile Creator", fileName = "ProjectileCreator_")]
public class ProjectileCreator : ScriptableObject
{
    public int count = 0;
    public List<ProjectileCreatorInfo> infos = new List<ProjectileCreatorInfo>();


    public void ExcuteCreate(BaseController owner, Transform target , MonoBehaviour monoBehaviour)
    {
        for (int i = 0; i < count; i++)
            monoBehaviour.StartCoroutine(ProjectileCreate_Co(owner,target ,infos[i]));
    }


    public IEnumerator ProjectileCreate_Co(BaseController owner, Transform target, ProjectileCreatorInfo info)
    {
        GameObject projecile = EffectManager.Instance.GetEffectObjectRandom(info.RangeInfo.projectileEffect, Vector3.zero, Vector3.zero, Vector3.zero);
        if (projecile.GetComponent<RangeAttackProjectile>() == null)
            projecile.AddComponent<RangeAttackProjectile>();

        yield return new WaitForSeconds(info.SpawnDelay);
       // projecile.transform.rotation = Quaternion.LookRotation(RetRotation(owner, target, projecile.transform, info));
        projecile.GetComponent<RangeAttackProjectile>()?.Setting(owner, target, info.RangeInfo, info);
        EffectManager.Instance.GetEffectObjectRandom(info.RangeInfo.flashEffect, projecile.transform.position, projecile.transform.eulerAngles, Vector3.zero);
    }




#if UNITY_EDITOR
    private void OnValidate()
    {
        if (infos.Count == count) return;
        if (infos == null)
            infos = new List<ProjectileCreatorInfo>();

        while(infos.Count != count)
        {
            if (infos.Count < count)
                infos.Add(new ProjectileCreatorInfo());
            if (infos.Count > count)
                infos.RemoveAt(infos.Count - 1);
        }
    }
#endif
}





[System.Serializable]
public class ProjectileCreatorInfo
{
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private RangeAttackInfo rangeInfo;


    public float SpawnDelay => spawnDelay;
    public RangeAttackInfo RangeInfo => rangeInfo;

}

