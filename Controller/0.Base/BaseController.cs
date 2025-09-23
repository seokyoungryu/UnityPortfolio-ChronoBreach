using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerInPosition
{
    NONE = -1,
    DAMAGED = 0,
    WEAPON = 1,
    RANGE_SHOOTING = 2,
    SKILL_SHOOTING = 3,
    SLASH = 4,
    PARENT_WEAPON = 5,
    PARENT_SHILD = 6,
    PARENT_RIGHT_HAND= 7,
    PARENT_LEFT_HAND = 8,
    PARENT_TRANSFORM = 9,
}

public abstract class BaseController : MonoBehaviour
{
    public Transform damagedPosition = null;
    public Transform weaponPosition = null;
    public Transform shildPosition = null;
    public Transform rangeShootingPosition = null;
    public Transform skillShootingPosition = null;
    public Transform rightHandPosition = null;
    public Transform leftHandPosition = null;
    public Transform slashPosition = null;
    public LayerMask targetLayer;
    public LayerMask friendlyLayer;
    public LayerMask obstacleLayer;   //공격 및 탐색시 막는 물체 확인용 레이어
    public LayerMask limitDashObstacleLayer;
    public LayerMask rangeProjectileObstacleLayer; // 투사체 충돌 레이어
    public LayerMask groundLayer = TagAndLayerDefine.LayersIndex.Ground;
    private RaycastHit obstacleHit;

    public virtual BaseStatus GetBaseStatus() { return null; }

    public abstract bool IsDead();

    public abstract void Damaged(float damage, BaseController attacker, bool isCritical, bool isSkill, AttackStrengthType attackStrengthType, bool isForceDmg = false);

    public abstract (bool, float) GetDamageValue(bool isSkill);

    public virtual bool CanDetect() { return true; }
    public virtual bool CanDamage() { return true; }

    public void SortFindEmenyByNearDistance(Transform target, ref Collider[] sortArrays)
    {
        if (target == null || sortArrays.Length <= 1) return;

        float prevDistance = -1f;
        float nextDistance = -1f;

        for (int i = 0; i < sortArrays.Length; i++)
        {
            for (int x = 0; x < sortArrays.Length; x++)
            {
                if (sortArrays[i] == null || sortArrays[x] == null || sortArrays[i] == sortArrays[x])
                    continue;

                prevDistance = (sortArrays[i].transform.position - target.position).sqrMagnitude;
                nextDistance = (sortArrays[x].transform.position - target.position).sqrMagnitude;
                if (prevDistance < nextDistance)
                {
                    Collider tmpColl = sortArrays[i];
                    sortArrays[i] = sortArrays[x];
                    sortArrays[x] = tmpColl;
                }
            }
        }
    }


    public Transform GetControllerInPosition(ControllerInPosition controllerPosition ,BaseController target)
    {
        switch (controllerPosition)
        {
            case ControllerInPosition.NONE:
                return target.transform;
            case ControllerInPosition.DAMAGED:
                return target.damagedPosition == null ? target.transform : target.damagedPosition;
            case ControllerInPosition.WEAPON:
                return target.weaponPosition == null ? target.transform : target.weaponPosition;
            case ControllerInPosition.RANGE_SHOOTING:
                return target.rangeShootingPosition == null ? target.transform : target.rangeShootingPosition;
            case ControllerInPosition.SKILL_SHOOTING:
                return target.skillShootingPosition == null ? target.transform : target.skillShootingPosition;
            case ControllerInPosition.SLASH:
                return target.slashPosition == null ? target.transform : target.slashPosition;
            case ControllerInPosition.PARENT_WEAPON:
                return target.weaponPosition == null ? target.transform : target.weaponPosition;
            case ControllerInPosition.PARENT_SHILD:
                return target.shildPosition == null ? target.transform : target.shildPosition;
            case ControllerInPosition.PARENT_LEFT_HAND:
                return target.leftHandPosition == null ? target.transform : target.leftHandPosition;
            case ControllerInPosition.PARENT_RIGHT_HAND:
                return target.rightHandPosition == null ? target.transform : target.rightHandPosition;
            case ControllerInPosition.PARENT_TRANSFORM:
                return target.transform;
            default:
                return target.transform;
        }
    }

    public bool CheckControllerIsDead(BaseController controller)
    {
        if (controller.IsDead())
            return true;

        return false;
    }


    public void DebugDistance(Transform target, Collider[] sortArrays)
    {
        if (target == null || sortArrays.Length <= 0) return;
        for (int i = 0; i < sortArrays.Length; i++)
            if (sortArrays[i] != null)
                Debug.Log(i + " : " + (sortArrays[i].transform.position - target.position).magnitude);
    }

    public bool IsDetectObstacle(Transform originTr, Transform targetTr)
    {
        if (originTr == null || targetTr == null) return false;

        Vector3 dir = targetTr.position - originTr.position;
        float distance = dir.magnitude;

        if (Physics.SphereCast(originTr.position, 0.05f, dir, out obstacleHit, distance, obstacleLayer))
            return true;

        return false;
    }
    public IEnumerator CreateAttackEffect(float delayTime, ControllerEffectInfo info, BaseController target)
    {
        Transform spawnTr = GetControllerInPosition(info.spawnType, target);
        yield return new WaitForSeconds(delayTime);
        Quaternion rot = Quaternion.LookRotation(transform.forward);
        Vector3 spawnPos = Vector3.zero;
        spawnPos = info.spawnPosition.x * transform.right
             + info.spawnPosition.y * transform.up
             + info.spawnPosition.z * transform.forward;
        GameObject effect = EffectManager.Instance.GetEffectObject(info.effect, spawnTr.position + spawnPos, rot.eulerAngles + info.spawnRotation, info.spawnScale);
        SoundManager.Instance.PlayEffect(info.effectSound);

       // Debug.Log($"<color=yellow> {effect}  , {effect?.activeInHierarchy} , T : {effect?.transform.position} </color>");
       // foreach (Transform chid in effect.transform)
       // {
       //     if (chid.GetComponent<ParticleSystem>())
       //         Debug.Log($"<color=white> {chid.name}  , {chid?.gameObject?.activeInHierarchy} , isPlaying : {chid.GetComponent<ParticleSystem>().isPlaying} , IsAlive : {chid.GetComponent<ParticleSystem>().IsAlive()} </color>");
       // }


        if (IsEffectParentPos(info.spawnType))
            effect.transform.parent = spawnTr;
    }

    public IEnumerator CreateTimeParticleEffect(float delayTime, ControllerEffectInfo info,BaseController target ,ParticleLifeType type, float stayTime, float duration, float returnTime)
    {
        Transform spawnTr = GetControllerInPosition(info.spawnType, target);
        yield return new WaitForSeconds(delayTime);
        Quaternion rot = Quaternion.LookRotation(transform.forward);
        Vector3 spawnPos = Vector3.zero;
        spawnPos = info.spawnPosition.x * transform.right
             + info.spawnPosition.y * transform.up
             + info.spawnPosition.z * transform.forward;
        GameObject effect = EffectManager.Instance.GetEffectObject(info.effect, spawnTr.position + spawnPos, rot.eulerAngles + info.spawnRotation, info.spawnScale);
        SoundManager.Instance.PlayEffect(info.effectSound);

        if (IsEffectParentPos(info.spawnType))
            effect.transform.parent = spawnTr;

        if (type == ParticleLifeType.RETURN_OBP_TIME)
            effect.GetComponent<ReturnObjectToObjectPooling>().TimeSetting(returnTime, -1f);
        else
            ParticleHelper.SettingParticleLifeTime(effect, type, stayTime, duration);
    }

    //1. 투사체기능 -> 위치 세팅, 날라가는 종류및기능, 출동 데미지.
    //2. controller에서는 owner 위치, target위치, infos를 받아오면됨.


    public BaseController[] GetBuffTargetList(BaseController ownController, BuffSkillClip clip)
    {
        List<BaseController> retTargets = new List<BaseController>();
        if (clip.buffOwnType == BuffOwnType.INCLUDE_OWN)
            retTargets.Add(ownController);

        if (clip.buffTargetType != BuffTargetType.NONE)
        {
            LayerMask targetLayer = ownController.targetLayer;
            switch (clip.buffTargetType)
            {
                case BuffTargetType.ONLY_ENEMY: targetLayer = ownController.targetLayer; break;
                case BuffTargetType.ONLY_FRIENDLY: targetLayer = ownController.friendlyLayer; break;
                case BuffTargetType.JUST_RANGE: targetLayer = ownController.targetLayer + ownController.friendlyLayer; break;
            }

            int count = 0;
            Collider[] detectTargets = Physics.OverlapSphere(ownController.transform.position, clip.detectRange, targetLayer);
            ownController.SortFindEmenyByNearDistance(ownController.transform, ref detectTargets);

            for (int i = 0; i < detectTargets.Length; i++)
            {
                if (count >= clip.maxTargetCount) break;
                if (detectTargets[i] == null || detectTargets[i].GetComponent<BaseController>() == null) continue;
                BaseController controller = detectTargets[i].GetComponent<BaseController>();
                if (ownController.IsDetectObstacle(ownController.damagedPosition, controller.damagedPosition)) continue;
                Vector3 dir = (detectTargets[i].transform.position - ownController.transform.position);
                dir.y = 0f;
                dir.Normalize();
                if (Vector3.Angle(ownController.transform.forward, dir) > clip.angle) continue;

                count++;
                retTargets.Add(controller);
                Debug.Log("버프 : " + controller.name);
            }
        }

        return retTargets.ToArray();
    }
    public bool IsEffectParentPos(ControllerInPosition type)
    {
        if (type == ControllerInPosition.PARENT_WEAPON || type == ControllerInPosition.PARENT_SHILD ||
            type == ControllerInPosition.PARENT_LEFT_HAND || type == ControllerInPosition.PARENT_RIGHT_HAND || type == ControllerInPosition.PARENT_TRANSFORM)
            return true;

        return false;
    }

}
