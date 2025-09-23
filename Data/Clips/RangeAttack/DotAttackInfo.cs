using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ParticleLifeType
{
    NONE = -1,
    DURATION = 0,
    LIFE_TIME = 1,
    DURATION_LIFETIME_SAME = 2,
    DURATION_LIFETIME_EACH = 3,
    RETURN_OBP_TIME = 4,
}

[System.Serializable]
public class DotAttackInfo
{
    public enum DotRegisterType { ONCE = 0, MULTI = 1, }   //즉 한번 등록할것인지, 데미지 시에 계속 등록받을것인지.
    public bool isSkill = false;
    public List<SoundList> hitSounds = null;
    public List<RandomEffectInfo> hitEffects = new List<RandomEffectInfo>();
    public List<AttackStrengthType> strengthTypes;
    public ParticleLifeType particleType = ParticleLifeType.NONE;
    public DotRegisterType dotRegisterType = DotRegisterType.ONCE;
    public Color drawDotLineColor;
    public Color drawDotEndColor;
    public List<float> damage = null;
    public List<float> hitDelay = null;
    public List<bool> isRandom = new List<bool>();
    public int maxTargetCount = 1;//maxCOunt 음.. 데미지 입히는 수? 
    public float stayTime = 5f;
    public int hitCount => damage.Count;    //한번 충돌했을때 입힐 데미지수 
    public Dictionary<int, BaseController> registerTargetDic = new Dictionary<int, BaseController>();
    public List<int> canDamageTargets = new List<int>();
    public List<int> endDamagedTarget = new List<int>();

    [Header("Dot Multi Register")]
    [SerializeReference] private float multiDelayCoolTime = 1f;

    [Header("Dot Check Detect")]
    public Vector2 detectBoxSize = Vector3.one;
    public float registerPointDelayTime = 0.2f;
    //private List<Vector3> dotPointPositions = new List<Vector3>();
    private List<DotRegisterInfo> dotPointPositions = new List<DotRegisterInfo>();
    private List<ReturnObjectToObjectPooling> effects = new List<ReturnObjectToObjectPooling>();

    private bool isDone = false;
    private int detectCount = 0;
    private Vector3 direction;
    private Vector3 center;
    private Vector3 boxSize;
    private Quaternion lookDirection;
    private LayerMask targetLayer;
    public bool isHitRandom = false;
    public RandomEffectInfo dotEffect;
    public bool eachDotCoolTime = false;
    public float particleStayTime = 0f;
    public float particleDurationTime = 0f;
    public bool IsDone => isDone;
    //public List<Vector3> DotPointPositions => dotPointPositions;
    public List<DotRegisterInfo> DotPointPositions => dotPointPositions;

    public List<RandomEffectInfo> HitEffects { get { return hitEffects; } set { hitEffects = value; } }
    public List<SoundList> HitSounds { get { return hitSounds; } set { hitSounds = value; } }
    public List<AttackStrengthType> StrengthTypes { get { return strengthTypes; } set { strengthTypes = value; } }
    public DotRegisterType RegisterDotType { get { return dotRegisterType; } set { dotRegisterType = value; } }
    public Color DrawDotLineColor { get { return drawDotLineColor; } set { drawDotLineColor = value; } }
    public Color DrawDotEndColor { get { return drawDotEndColor; } set { drawDotEndColor = value; } }
    public List<float> Damage { get { return damage; } set { damage = value; } }
    public List<float> HitDelay { get { return hitDelay; } set { hitDelay = value; } }
    public int MaxTargetCount { get { return maxTargetCount; } set { maxTargetCount = value; } }
    public float StayTime { get { return stayTime; } set { stayTime = value; } }
    public float PointDelayTime { get { return registerPointDelayTime; } set { registerPointDelayTime = value; } }
    public Vector2 DetectBoxSize { get { return detectBoxSize; } set { detectBoxSize = value; } }
    public float MultiDelayCoolTime { get { return multiDelayCoolTime; } set { multiDelayCoolTime = value; } }


    public DotAttackInfo Clone()
    {
        DotAttackInfo clone = new DotAttackInfo();
        clone.hitEffects = new List<RandomEffectInfo>(hitEffects);
        clone.hitSounds = new List<SoundList>(hitSounds);
        clone.strengthTypes = new List<AttackStrengthType>(strengthTypes);
        clone.dotRegisterType = dotRegisterType;
        clone.drawDotLineColor = drawDotLineColor;
        clone.drawDotEndColor = drawDotEndColor;
        clone.damage = new List<float>(damage);
        clone.hitDelay = new List<float>(hitDelay);
        clone.isRandom = new List<bool>(isRandom);
        clone.registerTargetDic = new Dictionary<int, BaseController>(registerTargetDic);
        clone.canDamageTargets = new List<int>(canDamageTargets);
        clone.endDamagedTarget = new List<int>(endDamagedTarget);
        //clone.dotPointPositions = new List<Vector3>(dotPointPositions);
        clone.dotPointPositions = new List<DotRegisterInfo>(dotPointPositions);
        clone.maxTargetCount = maxTargetCount;
        clone.stayTime = stayTime;
        clone.multiDelayCoolTime = multiDelayCoolTime;
        clone.detectBoxSize = detectBoxSize;
        clone.registerPointDelayTime = registerPointDelayTime;
        clone.isDone = isDone;
        clone.detectCount = detectCount;
        clone.direction = direction;
        clone.effects = new List<ReturnObjectToObjectPooling>(effects);
        clone.center = center;
        clone.boxSize = boxSize;
        clone.lookDirection = lookDirection;
        clone.particleStayTime = particleStayTime;
        clone.targetLayer = targetLayer;
        clone.particleType = particleType;
        clone.dotEffect = dotEffect;
        clone.isHitRandom = isHitRandom;
        clone.eachDotCoolTime = eachDotCoolTime;
        clone.particleDurationTime = particleDurationTime;
        return clone;
    }

    public void SetTargetLayerMask(LayerMask targetMask) => targetLayer = targetMask;
    public void AddDotPointPosition(Vector3 point) => dotPointPositions.Add(new DotRegisterInfo(point,stayTime));
    public void ClearDotPointPosition() => dotPointPositions.Clear();

    public void CheckAndRegisterTargetInDotArea(ref Collider[] retColliders)
    {
        if (dotPointPositions.Count <= 0 || registerTargetDic.Count >= maxTargetCount) return;

        canDamageTargets.Clear();
        boxSize = detectBoxSize;

        for (int i = 0; i < dotPointPositions.Count; i++)
        {
            detectCount = 0;
            if (dotPointPositions.Count > i + 1)
            {
                direction = (dotPointPositions[i + 1].dotPosition - dotPointPositions[i].dotPosition).normalized;
                center = (dotPointPositions[i + 1].dotPosition - dotPointPositions[i].dotPosition) / 2;
                boxSize.z = (dotPointPositions[i + 1].dotPosition - dotPointPositions[i].dotPosition).magnitude;
                lookDirection = Quaternion.LookRotation(direction);
                detectCount = Physics.OverlapBoxNonAlloc(dotPointPositions[i].dotPosition + center, boxSize / 2, retColliders, lookDirection, targetLayer);
            }
            else
            {
                boxSize = Vector3.one;
                detectCount = Physics.OverlapBoxNonAlloc(dotPointPositions[i].dotPosition, boxSize / 2, retColliders, Quaternion.identity, targetLayer);
            }

            if (detectCount > 0) 
                ExcuteRegister(retColliders);
        }
    }


    private void ExcuteRegister(Collider[] detectedColliders)
    {
        for (int i = 0; i < detectedColliders.Length; i++)
        {
            if (detectedColliders[i] == null || detectedColliders[i].GetComponent<IDamageable>() == null || endDamagedTarget.Contains(detectedColliders[i].GetInstanceID()))
                continue;
            if (!registerTargetDic.ContainsKey(detectedColliders[i].GetInstanceID()) && registerTargetDic.Count < maxTargetCount)
            {
                registerTargetDic.Add(detectedColliders[i].GetInstanceID(), detectedColliders[i].GetComponent<BaseController>());
                canDamageTargets.Add(detectedColliders[i].GetInstanceID());
            }
        }

    }

    public void ExcuteDamage(BaseController owner)
    {
        for (int i = 0; i < canDamageTargets.Count; i++)
        {
            if (registerTargetDic.ContainsKey(canDamageTargets[i]) && !endDamagedTarget.Contains(canDamageTargets[i]))
            {
                owner.StartCoroutine(DamageProcess_Co(owner, registerTargetDic[canDamageTargets[i]], canDamageTargets[i]));
                if (dotRegisterType == DotRegisterType.MULTI)
                    owner.StartCoroutine(CheckResetEndDamagedTarget_Co(canDamageTargets[i]));
            }

        }
    }

    public void CheckRegisterTimer()
    {
        if (dotPointPositions.Count <= 0) return;

        for (int i = 0; i < dotPointPositions.Count; i++)
        {
            if(!dotPointPositions[i].isDone)
            {
                dotPointPositions[i].currOutTimer += Time.deltaTime;
                if (dotPointPositions[i].currOutTimer >= dotPointPositions[i].outTime)
                    dotPointPositions[i].isDone = true;
            }
        }
    }

    public void CheckDotDone()
    {
        if (dotPointPositions.Count <= 0) return;

        for (int i = 0; i < dotPointPositions.Count; i++)
        {
            if (dotPointPositions[i].isDone)
                dotPointPositions.RemoveAt(i);
        }

    }

    public void Draw()
    {
        for (int i = 0; i < dotPointPositions.Count; i++)
        {
            if (dotPointPositions.Count > i + 1)
            {
                Gizmos.color = Color.black;
                Vector3 center = (dotPointPositions[i + 1].dotPosition + dotPointPositions[i].dotPosition) / 2;
                Gizmos.DrawSphere(center, 0.1f);
            }
        }


        if (dotPointPositions.Count <= 0)
          return;

        for (int i = 0; i < dotPointPositions.Count; i++)
        {
            if (dotPointPositions.Count > i + 1 )
            {
                Vector3 direction = (dotPointPositions[i + 1].dotPosition - dotPointPositions[i].dotPosition).normalized;
                Vector3 boxSize = detectBoxSize;
                boxSize.z = (dotPointPositions[i + 1].dotPosition - dotPointPositions[i].dotPosition).magnitude;
                Vector3 center = (dotPointPositions[i + 1].dotPosition + dotPointPositions[i].dotPosition) / 2; // 중간점 계산
                Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.LookRotation(direction), Vector3.one);
                Gizmos.color = drawDotLineColor;
                Gizmos.DrawWireCube(Vector3.zero, boxSize);
            }
            else
            {
                Vector3 boxSize1 = Vector3.one;
                Gizmos.matrix = Matrix4x4.TRS(dotPointPositions[i].dotPosition, Quaternion.identity, Vector3.one);
                Gizmos.color = drawDotEndColor;
                Gizmos.DrawWireCube(Vector3.zero, boxSize1);
            }

           
        }

      
    }


    public void CreateEffect(int prevIndex, int curIndex)
    {
        if (DotPointPositions.Count <= curIndex) return;

        Vector3 center = (dotPointPositions[curIndex].dotPosition + dotPointPositions[prevIndex].dotPosition) / 2; // 중간점 계산
        ReturnObjectToObjectPooling effect = EffectManager.Instance.GetEffectObjectRandom(dotEffect, center, Vector3.zero, Vector3.zero).GetComponent<ReturnObjectToObjectPooling>();
        SoundManager.Instance.PlayEffect(dotEffect.effectSound);
        ParticleSystem[] particels = effect.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particels.Length; i++)
            particels[i].Stop();

        for (int i = 0; i < particels.Length; i++)
        {
            var main = particels[i].main;
            main.loop = false;
            if (particleType == ParticleLifeType.DURATION)
                main.duration = particleStayTime;
            else if (particleType == ParticleLifeType.LIFE_TIME)
                main.startLifetime = particleStayTime;
            else if (particleType == ParticleLifeType.DURATION_LIFETIME_SAME)
            {
                main.duration = particleStayTime;
                main.startLifetime = particleStayTime;
            }
            else if (particleType == ParticleLifeType.DURATION_LIFETIME_EACH)
            {
                main.duration = particleDurationTime;
                main.startLifetime = particleStayTime;
            }
        }

        for (int i = 0; i < particels.Length; i++)
            particels[i].Play();

        effects.Add(effect);
    }


    public IEnumerator CheckStayTime()
    {
        isDone = false;
        yield return new WaitForSeconds(stayTime);
        if (!eachDotCoolTime)
            isDone = true;
    }

    private IEnumerator CheckResetEndDamagedTarget_Co(int instanceID)
    {
        if (!registerTargetDic.ContainsKey(instanceID) && !endDamagedTarget.Contains(instanceID))
            yield break;

        yield return new WaitForSeconds(multiDelayCoolTime);

        for (int i = 0; i < endDamagedTarget.Count; i++)
            if (endDamagedTarget[i] == instanceID)
                endDamagedTarget.RemoveAt(i);

        for (int i = 0; i < registerTargetDic.Count; i++)
            if (registerTargetDic.ContainsKey(instanceID))
                registerTargetDic.Remove(instanceID);
    }


    //1인 hitCount만큼 데미지 입히기. 
    private IEnumerator DamageProcess_Co(BaseController owner , BaseController target, int targetInstanceID)
    {
        endDamagedTarget.Add(targetInstanceID);

        for (int i = 0; i < hitDelay.Count; i++)
        {
            yield return new WaitForSeconds(hitDelay[i]);
            (bool, float) damageTuple = owner.GetDamageValue(true);
            if (target is PlayerStateController)
            {
                PlayerStateController player = target as PlayerStateController;
                if (player.Conditions.IsCounting || player.Conditions.IsDetectParry)
                    player.GetState<CounterAttackState>().CounterSuccess(owner, CounterAttackType.RANGE);
                else if (player.Conditions.CanDamaged())
                    player.playerStats.Damaged(damage[i] * damageTuple.Item2, owner, damageTuple.Item1, isSkill, strengthTypes[i]);

                if (hitEffects.Count > i)
                    EffectManager.Instance.GetEffectObjectRandom(hitEffects[i], target.transform.position, Vector3.zero, Vector3.zero);
            }
            else if (target is AIController)
            {
                AIController ai = target as AIController;
                if (!ai.aiConditions.IsDead)
                    ai.aiStatus.Damaged(damage[i] * damageTuple.Item2, owner, damageTuple.Item1, isSkill, strengthTypes[i]);

                if (hitEffects.Count > i)
                    EffectManager.Instance.GetEffectObjectRandom(hitEffects[i], target.transform.position, Vector3.zero, Vector3.zero);
            }
        }
    }




    public void ClearInstanceID()
    {
        registerTargetDic.Clear();
        canDamageTargets.Clear();

    }


}

public class DotRegisterInfo
{
    public Vector3 dotPosition = Vector3.zero;
    public float outTime = 0f;
    public float currOutTimer = 0f;
    public bool isDone = false;

    public DotRegisterInfo(Vector3 pos, float outTime)
    {
        dotPosition = pos;
        this.outTime = outTime;
    }
}
