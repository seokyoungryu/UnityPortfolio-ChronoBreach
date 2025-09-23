using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public enum FoldOut { NONE = 0, ALL_OPEN = 1, ALL_CLOSE =2 }

/// <summary>
/// RangeAttackInfo의 실제 GUI 설정 부분
/// </summary>
[CustomEditor(typeof(RangeAttackInfo)), CanEditMultipleObjects]
public class RangeAttackInfosGUI : Editor
{
    protected static FoldOut foldoutsType = FoldOut.NONE;
    protected RangeAttackInfo clip = null;
    protected static bool foldOutTitle = true;
    protected static bool foldOutDotTitleRoot = true;
    protected static bool foldOutRootRequiredVariable = true;
    protected static bool foldOutRootDotRequiredVariable = true;
    protected static bool foldOutFlash = false;
    protected static bool foldOutCollision = false;
    protected static bool foldOutProjectile = false;


    protected static List<bool> foldOutHitEffects = new List<bool>();
    protected static List<bool> foldOutRangeRequiredVariables = new List<bool>();
    protected static List<bool> foldOutDotRequiredVariables = new List<bool>();
    protected static List<bool> foldOutDotEffectRequiredVariables = new List<bool>();

    protected GUIStyle titleGuiStyle = new GUIStyle();
    protected GUIStyle dotTitleGuiStyle = new GUIStyle();
    protected GUIStyle afterAttackInfoGuiStyle = new GUIStyle();


    public override void OnInspectorGUI()
    {
        clip = (RangeAttackInfo)target;

        GUILayout.BeginVertical("Box");
        {
            if (foldOutRangeRequiredVariables.Count != clip.Damage.Count)
            {
                foldOutRangeRequiredVariables.Clear();
                for (int i = 0; i < clip.Damage.Count; i++)
                    foldOutRangeRequiredVariables.Add(true);
            }
            if (clip.UseDotDamage && foldOutDotRequiredVariables.Count != clip.DotInfo.Damage.Count)
            {
                foldOutDotRequiredVariables.Clear();
                for (int i = 0; i < clip.DotInfo.Damage.Count; i++)
                {
                    foldOutDotEffectRequiredVariables.Add(true);
                    foldOutDotRequiredVariables.Add(true);
                    clip.dotInfo.isRandom.Add(true);
                }
            }

            if (foldOutHitEffects.Count != clip.Damage.Count)
            {
                foldOutHitEffects.Clear();
                for (int i = 0; i < clip.Damage.Count; i++)
                    foldOutHitEffects.Add(true);
            }

            titleGuiStyle.fontSize = 20;
            titleGuiStyle.normal.textColor = Color.white;
            titleGuiStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("원거리 공격 정보", titleGuiStyle);

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Setting FoldOut ", GUILayout.Width(150));
                foldoutsType = (FoldOut)EditorGUILayout.EnumPopup(foldoutsType);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (foldoutsType == FoldOut.ALL_OPEN)
                {
                    foldOutDotTitleRoot = true;
                    foldOutRootRequiredVariable = true;
                    foldOutRootDotRequiredVariable = true;
                    foldOutFlash = true;
                    foldOutCollision = true;
                    foldOutProjectile = true;

                    if (foldOutHitEffects.Count > 0)
                        for (int i = 0; i < foldOutHitEffects.Count; i++)
                            foldOutHitEffects[i] = true;
                    if (foldOutRangeRequiredVariables.Count > 0)
                        for (int i = 0; i < foldOutRangeRequiredVariables.Count; i++)
                            foldOutRangeRequiredVariables[i] = true;
                    if (foldOutDotRequiredVariables.Count > 0)
                        for (int i = 0; i < foldOutDotRequiredVariables.Count; i++)
                            foldOutDotRequiredVariables[i] = true;

                }
                else if (foldoutsType == FoldOut.ALL_CLOSE)
                {
                    foldOutDotTitleRoot = false;
                    foldOutRootRequiredVariable = false;
                    foldOutRootDotRequiredVariable = false;
                    foldOutFlash = false;
                    foldOutCollision = false;
                    foldOutProjectile = false;

                    if (foldOutHitEffects.Count > 0)
                        for (int i = 0; i < foldOutHitEffects.Count; i++)
                            foldOutHitEffects[i] = false;
                    if (foldOutRangeRequiredVariables.Count > 0)
                        for (int i = 0; i < foldOutRangeRequiredVariables.Count; i++)
                            foldOutRangeRequiredVariables[i] = false;
                    if (foldOutDotRequiredVariables.Count > 0)
                        for (int i = 0; i < foldOutDotRequiredVariables.Count; i++)
                            foldOutDotRequiredVariables[i] = false;
                }
                else { }


            }
            GUILayout.EndHorizontal();


            foldOutTitle = EditorGUILayout.Foldout(foldOutTitle, "Datas");

            if (foldOutTitle)
            {
                EditorGUILayout.Space(20);

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Is Skill", GUILayout.Width(100));
                    clip.IsSkill = EditorGUILayout.Toggle(clip.IsSkill);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();


                foldOutProjectile = EditorGUILayout.Foldout(foldOutProjectile, "Projectile Effect Data");
                if (foldOutProjectile)
                    EditorHelper.GUIEffectRandom(ref clip.projectileEffect, ref clip.isProjectileRandom, "Projectile");


                foldOutFlash = EditorGUILayout.Foldout(foldOutFlash, "Flash Effect Data");
                if (foldOutFlash)
                    EditorHelper.GUIEffectRandom(ref clip.flashEffect, ref clip.isFlashRandom, "Flash");


                foldOutCollision = EditorGUILayout.Foldout(foldOutCollision, "Collision Effect Data");
                if (foldOutCollision)
                    EditorHelper.GUIEffectRandom(ref clip.collisionEffect, ref clip.isCollisionRandom, "Collision");


                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 공격 타입", GUILayout.Width(100));
                    clip.RangeAttackType = (RangeAttackType)EditorGUILayout.EnumPopup(clip.RangeAttackType);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                if (clip.RangeAttackType == RangeAttackType.PENETRATE)
                {
                    GUILayout.BeginVertical("HelpBox");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("같은 적 데미지", GUILayout.Width(150));
                            clip.AllowPenetrateSameEnemy = EditorGUILayout.Toggle(clip.AllowPenetrateSameEnemy);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        if (clip.AllowPenetrateSameEnemy)
                        {

                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("같은 적 데미지 딜레이 시간", GUILayout.Width(150));
                                clip.SamePenetrateDelayTime = EditorGUILayout.FloatField(clip.SamePenetrateDelayTime);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("같은 적 최대 데미지 카운트", GUILayout.Width(150));
                                clip.MaxSamePenetrateCount = EditorGUILayout.IntField(clip.MaxSamePenetrateCount);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                        }

                    }
                    GUILayout.EndVertical();
                }
                else if (clip.RangeAttackType == RangeAttackType.POINT_EXPLOSION)
                {
                    GUILayout.BeginVertical("HelpBox");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            if (clip.DelayExplosionAttack) EditorGUILayout.LabelField("Hit Delay 마다 공격검사.", GUILayout.Width(150));
                            else EditorGUILayout.LabelField("한번 충돌시 Hit List 수 만큼 데미지 입힘.", GUILayout.Width(220));

                            clip.DelayExplosionAttack = EditorGUILayout.Toggle(clip.DelayExplosionAttack);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                    }
                    GUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 이동 타입", GUILayout.Width(100));
                    clip.RangeMoveType = (RangeMoveType)EditorGUILayout.EnumPopup(clip.RangeMoveType);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                #region MoveType
                if (clip.RangeMoveType == RangeMoveType.HOMING)
                {
                    GUILayout.BeginVertical("Box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Noise Curve", GUILayout.Width(100));
                            clip.NoiseCurve = EditorGUILayout.CurveField(clip.NoiseCurve);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Position Curve", GUILayout.Width(100));
                            clip.PositionCurve = EditorGUILayout.CurveField(clip.PositionCurve);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Min Noise", GUILayout.Width(100));
                            clip.MinNoise = EditorGUILayout.Vector2Field("", clip.MinNoise);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Max Noise", GUILayout.Width(100));
                            clip.MaxNoise = EditorGUILayout.Vector2Field("", clip.MaxNoise);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                else if (clip.RangeMoveType == RangeMoveType.TARGETING)
                {
                    GUILayout.BeginVertical("Box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            if (!clip.stillMoveToTarget) EditorGUILayout.LabelField("타겟 데미지시 이동 멈춤.", GUILayout.Width(200));
                            else EditorGUILayout.LabelField("종료 전까지 타겟한테 이동", GUILayout.Width(200));
                            clip.stillMoveToTarget = EditorGUILayout.Toggle(clip.stillMoveToTarget);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Targeting Rotate Type", GUILayout.Width(100));
                            clip.targetingRotateType = (ProjectileTargetingRotateType)EditorGUILayout.EnumPopup(clip.targetingRotateType);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();

                        if (clip.targetingRotateType == ProjectileTargetingRotateType.LOOK_TARGET)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Smooth 회전 속도", GUILayout.Width(100));
                                clip.SmoothRotateSpeed = EditorGUILayout.FloatField(clip.SmoothRotateSpeed);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                        }
                        else if (clip.targetingRotateType == ProjectileTargetingRotateType.CUSTOM_SETTING)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("회전 각도", GUILayout.Width(100));
                                clip.customRotate = EditorGUILayout.Vector3Field("", clip.customRotate);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("회전 속도", GUILayout.Width(100));
                                clip.customRotateSpeed = EditorGUILayout.FloatField(clip.customRotateSpeed);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                        }

                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Lerp 이동", GUILayout.Width(100));
                            clip.IsMoveLerp = EditorGUILayout.Toggle(clip.IsMoveLerp);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("타겟 자동 탐색", GUILayout.Width(100));
                            clip.AutoDetectTarget = EditorGUILayout.Toggle(clip.AutoDetectTarget);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("실행 딜레이 시간", GUILayout.Width(100));
                            clip.ExcuteTargetingDelay = EditorGUILayout.FloatField(clip.ExcuteTargetingDelay);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("자동 탐색 주기", GUILayout.Width(100));
                            clip.AutoDetectDelay = EditorGUILayout.FloatField(clip.AutoDetectDelay);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("자동 탐색 거리", GUILayout.Width(100));
                            clip.AutoDetectRange = EditorGUILayout.FloatField(clip.AutoDetectRange);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("타겟팅 주기", GUILayout.Width(100));
                            clip.UpdateTargetingDelay = EditorGUILayout.FloatField(clip.UpdateTargetingDelay);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                else if (clip.RangeMoveType == RangeMoveType.WAVE)
                {
                    GUILayout.BeginVertical("Box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("실행 딜레이 시간", GUILayout.Width(100));
                            clip.ExcuteWaveDelay = EditorGUILayout.FloatField(clip.ExcuteWaveDelay);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Wave 타겟까지 시간", GUILayout.Width(100));
                            clip.WaveTime = EditorGUILayout.FloatField(clip.WaveTime);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("초기 Wave Radius", GUILayout.Width(100));
                            clip.InitWaveRadius = EditorGUILayout.FloatField(clip.InitWaveRadius);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("목표 Wave Radius", GUILayout.Width(100));
                            clip.TargetWaveRadius = EditorGUILayout.FloatField(clip.TargetWaveRadius);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Wave Curve", GUILayout.Width(100));
                            clip.CurveScale = EditorGUILayout.CurveField(clip.CurveScale);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                #endregion

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 생성 위치", GUILayout.Width(100));
                    clip.ProjectileSpawnPosition = (ProjectileSpawnPosition)EditorGUILayout.EnumPopup(clip.ProjectileSpawnPosition);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                if (clip.projectileSpawnPosition == ProjectileSpawnPosition.OWNER_TARGET_MIDDLE ||
                    clip.projectileSpawnPosition == ProjectileSpawnPosition.TARGET_RIGHT ||
                    clip.projectileSpawnPosition == ProjectileSpawnPosition.TARGET_FORWARD)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("타겟위치에서 생성할 거리", GUILayout.Width(150));
                        clip.targetSpawnOffset = EditorGUILayout.FloatField(clip.targetSpawnOffset);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }

                if (clip.rangeMoveType != RangeMoveType.HOMING && clip.rangeMoveType != RangeMoveType.TARGETING && clip.rangeMoveType != RangeMoveType.WAVE)
                {
                    if (clip.rangeMoveType != RangeMoveType.POINT)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("투사체 이동 방향", GUILayout.Width(100));
                            clip.moveDirectType = (ProjectileMoveDirectType)EditorGUILayout.EnumPopup(clip.moveDirectType);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }

                }

                if (clip.rangeMoveType != RangeMoveType.HOMING)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("투사체 회전", GUILayout.Width(100));
                        clip.rotationType = (ProjectileRotationType)EditorGUILayout.EnumPopup(clip.rotationType);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    if (clip.rotationType == ProjectileRotationType.OWNER_TO_TARGET_DIRECT ||
                       clip.rotationType == ProjectileRotationType.OWNER_TO_TARGET_DIRECT_YZERO ||
                       clip.rotationType == ProjectileRotationType.PROJECTILE_TO_TARGET_ZERO)
                    {

                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("공격 위치 오프셋", GUILayout.Width(100));
                            clip.TargetDirectionOffset = EditorGUILayout.Vector3Field("", clip.TargetDirectionOffset);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                }


                //  if (clip.isProjectileRandom)
                //  {
                //      GUILayout.BeginHorizontal();
                //      {
                //          EditorGUILayout.LabelField("생성 위치 오프셋", GUILayout.Width(100));
                //          clip.SpawnPositionOffset = EditorGUILayout.Vector3Field("", clip.SpawnPositionOffset);
                //          GUILayout.FlexibleSpace();
                //      }
                //      GUILayout.EndHorizontal();
                //    //GUILayout.BeginHorizontal();
                //    //{
                //    //    EditorGUILayout.LabelField("생성 회전 오프셋", GUILayout.Width(100));
                //      clip.SpawnRotationOffset = EditorGUILayout.Vector3Field("", clip.SpawnRotationOffset);
                //    //    GUILayout.FlexibleSpace();
                //    //}
                //    //GUILayout.EndHorizontal();
                //  }

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 충돌 타입", GUILayout.Width(120));
                    clip.collisionDetectType = (ProjectileCollisionDetectType)EditorGUILayout.EnumPopup(clip.collisionDetectType);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                if (clip.collisionDetectType == ProjectileCollisionDetectType.SELECT)
                {
                    GUILayout.BeginHorizontal("HelpBox");
                    {
                        EditorGUILayout.LabelField("투사체 충돌 레이어", GUILayout.Width(120));
                        clip.collisionDetectLayer = EditorGUILayout.MaskField("", clip.collisionDetectLayer, GetLayerNames());
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 타겟 탐지 타입", GUILayout.Width(120));
                    clip.targetDetectType = (ProjectileTargetDetectType)EditorGUILayout.EnumPopup(clip.targetDetectType);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                if (clip.targetDetectType == ProjectileTargetDetectType.OWNER_OBSTACLE_SELECT ||
                    clip.targetDetectType == ProjectileTargetDetectType.OWNER_TARGET_OBSTACLE_SELECT ||
                    clip.targetDetectType == ProjectileTargetDetectType.OWNER_TARGET_SELECT ||
                    clip.targetDetectType == ProjectileTargetDetectType.SELECT)
                {
                    GUILayout.BeginHorizontal("HelpBox");
                    {
                        EditorGUILayout.LabelField("투사체 타겟 탐색 레이어", GUILayout.Width(120));
                        clip.targetDetectLayer = EditorGUILayout.MaskField("", clip.targetDetectLayer, GetLayerNames());
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }


                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("투사체 장애물 탐지 타입", GUILayout.Width(140));
                    clip.ProjectileDetectType = (ProjectileDetectType)EditorGUILayout.EnumPopup(clip.ProjectileDetectType);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                #region Detect Type



                if (clip.rangeMoveType != RangeMoveType.WAVE)
                {
                    if (clip.ProjectileDetectType == ProjectileDetectType.SPHERE)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("투사체 장애물 충돌 범위", GUILayout.Width(130));
                            clip.detectObstacleRadius = EditorGUILayout.FloatField("", clip.detectObstacleRadius);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("투사체 타겟 충돌 범위", GUILayout.Width(130));
                            clip.DetectTargetRadius = EditorGUILayout.FloatField("", clip.DetectTargetRadius);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    else if (clip.ProjectileDetectType == ProjectileDetectType.CUBE)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("탐지 위치", GUILayout.Width(100));
                            clip.ProjectileDetectCubePosition = EditorGUILayout.Vector3Field("", clip.ProjectileDetectCubePosition);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal(); GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("탐지 Size", GUILayout.Width(100));
                            clip.ProjectileCubeSize = EditorGUILayout.Vector3Field("", clip.ProjectileCubeSize);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                #endregion

                #region Base Variables

                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Max Target Count", GUILayout.Width(100));
                    clip.MaxTargetCount = EditorGUILayout.IntField(clip.MaxTargetCount);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                if (clip.RangeMoveType != RangeMoveType.WAVE)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Max Distance", GUILayout.Width(100));
                        clip.MaxDistance = EditorGUILayout.FloatField(clip.MaxDistance);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }


                if (clip.RangeMoveType != RangeMoveType.WAVE && clip.RangeMoveType != RangeMoveType.POINT)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Is Accelerate", GUILayout.Width(100));
                        clip.IsAccelerate = EditorGUILayout.Toggle(clip.IsAccelerate);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }



                if (clip.RangeMoveType == RangeMoveType.HOMING)
                    EditorGUILayout.LabelField("Homing 속도 범위 (0 ~ 1)");


                if (clip.IsAccelerate && clip.RangeMoveType != RangeMoveType.POINT && clip.RangeMoveType != RangeMoveType.WAVE)
                {
                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("초기 가속 Speed", GUILayout.Width(100));
                            clip.InitSpeed = EditorGUILayout.FloatField(clip.InitSpeed);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("최대 가속 Speed", GUILayout.Width(100));
                            clip.MaxSpeed = EditorGUILayout.FloatField(clip.MaxSpeed);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Additive Speed", GUILayout.Width(100));
                            clip.AdditiveSpeed = EditorGUILayout.FloatField(clip.AdditiveSpeed);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Additive Delay Time", GUILayout.Width(100));
                            clip.AdditiveDelayTime = EditorGUILayout.FloatField(clip.AdditiveDelayTime);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
                else if (!clip.IsAccelerate && clip.RangeMoveType != RangeMoveType.POINT && clip.rangeMoveType != RangeMoveType.WAVE)
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Speed", GUILayout.Width(100));
                        clip.Speed = EditorGUILayout.FloatField(clip.Speed);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }


                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Return OBP Time", GUILayout.Width(100));
                    clip.ReturnTime = EditorGUILayout.FloatField(clip.ReturnTime);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                #endregion


                //Buttons
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("필수 세팅들", GUILayout.Width(100));

                    if (GUILayout.Button("추가"))
                    {

                        if (clip.StrengthType == null)
                            clip.StrengthType = new List<AttackStrengthType>();
                        if (clip.Damage == null)
                            clip.Damage = new List<float>();
                        if (clip.DamageRange == null)
                            clip.DamageRange = new List<float>();
                        if (clip.HitDelay == null)
                            clip.HitDelay = new List<float>();
                        if (clip.HitEffects == null)
                            clip.HitEffects = new List<RandomEffectInfo>();
                        if (clip.IsHitRandom == null)
                            clip.IsHitRandom = new List<bool>();
                        if (clip.timeDatas == null)
                            clip.timeDatas = new List<TimeData>();
                        foldOutRangeRequiredVariables.Add(true);

                        clip.HitEffects.Add(new RandomEffectInfo());
                        foldOutHitEffects.Add(true);
                        clip.ExplosionAngle.Add(0);
                        clip.StrengthType.Add(AttackStrengthType.NONE);
                        clip.Damage.Add(0f);
                        clip.DamageRange.Add(0f);
                        clip.HitDelay.Add(0f);
                        clip.IsHitRandom.Add(true);
                        clip.timeDatas.Add(null);
                    }
                    if (GUILayout.Button("삭제"))
                    {
                        if (foldOutRangeRequiredVariables.Count > 0)
                            foldOutRangeRequiredVariables.RemoveAt(foldOutRangeRequiredVariables.Count - 1);

                        if (clip.StrengthType.Count > 0)
                            clip.StrengthType.RemoveAt(clip.StrengthType.Count - 1);
                        if (clip.Damage.Count > 0)
                            clip.Damage.RemoveAt(clip.Damage.Count - 1);
                        if (clip.DamageRange.Count > 0)
                            clip.DamageRange.RemoveAt(clip.DamageRange.Count - 1);
                        if (clip.HitDelay.Count > 0)
                            clip.HitDelay.RemoveAt(clip.HitDelay.Count - 1);
                        if (clip.IsHitRandom.Count > 0)
                            clip.IsHitRandom.RemoveAt(clip.IsHitRandom.Count - 1);
                        if (clip.ExplosionAngle.Count > 0)
                            clip.ExplosionAngle.RemoveAt(clip.ExplosionAngle.Count - 1);
                        if (foldOutHitEffects.Count > 0)
                            foldOutHitEffects.RemoveAt(foldOutHitEffects.Count - 1);
                        if (clip.HitEffects.Count > 0)
                            clip.HitEffects.RemoveAt(clip.HitEffects.Count - 1);
                        if (clip.timeDatas.Count > 0)
                            clip.timeDatas.RemoveAt(clip.timeDatas.Count - 1);
                    }
                }
                GUILayout.EndHorizontal();


                if (foldOutRangeRequiredVariables.Count > 0)
                {
                    foldOutRootRequiredVariable = EditorGUILayout.Foldout(foldOutRootRequiredVariable, "데이터");

                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Is Repeat Damage", GUILayout.Width(150));
                        clip.isRepeatDamage = EditorGUILayout.Toggle(clip.isRepeatDamage);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    if (!clip.isRepeatDamage)
                    {
                        if (foldOutRootRequiredVariable)
                        {
                            GUILayout.BeginVertical("HelpBox");
                            for (int i = 0; i < clip.Damage.Count; i++)
                            {
                                foldOutRangeRequiredVariables[i] = EditorGUILayout.Foldout(foldOutRangeRequiredVariables[i], i + "번 데이터");
                                if (foldOutRangeRequiredVariables[i])
                                {
                                    GUILayout.BeginVertical("Box");
                                    {
                                        GUILayout.BeginHorizontal();
                                        {
                                            clip.timeDatas[i] = EditorGUILayout.ObjectField($"[{i}] Hit TimeData", clip.timeDatas[i], typeof(TimeData), false) as TimeData;
                                        }
                                        GUILayout.EndHorizontal();

                                        foldOutHitEffects[i] = EditorGUILayout.Foldout(foldOutHitEffects[i], "Hit Effect Datas");
                                        if (foldOutHitEffects[i])
                                        {
                                            EditorGUILayout.BeginVertical("HelpBox");

                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Hit Sound", GUILayout.Width(80));
                                                clip.HitEffects[i].effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.HitEffects[i].effectSound, GUILayout.Width(250));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Hit Effect", GUILayout.Width(80));
                                                clip.HitEffects[i].effect = (EffectList)EditorGUILayout.EnumPopup("", clip.HitEffects[i].effect, GUILayout.Width(250));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();


                                            EditorGUILayout.BeginVertical("Box");
                                            {
                                                EditorGUILayout.BeginHorizontal();
                                                {
                                                    EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                                                    clip.IsHitRandom[i] = EditorGUILayout.Toggle(clip.IsHitRandom[i]);
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();

                                                if (clip.IsHitRandom[i])
                                                {
                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                            clip.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minPosition, GUILayout.Width(180));
                                                            GUILayout.Label(" - ");
                                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                            clip.HitEffects[i].maxPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxPosition, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();

                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.LabelField("Rotation", GUILayout.Width(150));
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                            clip.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minRotation, GUILayout.Width(180));
                                                            GUILayout.Label(" - ");
                                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                            clip.HitEffects[i].maxRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxRotation, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();

                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.LabelField("Scale", GUILayout.Width(150));
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                            clip.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minScale, GUILayout.Width(180));
                                                            GUILayout.Label(" - ");
                                                            EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                            clip.HitEffects[i].maxScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].maxScale, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();
                                                }
                                                else
                                                {
                                                    clip.HitEffects[i].maxPosition = Vector3.zero;
                                                    clip.HitEffects[i].maxRotation = Vector3.zero;
                                                    clip.HitEffects[i].maxScale = Vector3.zero;
                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                                            clip.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minPosition, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();

                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Rotation", GUILayout.Width(100));
                                                            clip.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minRotation, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();

                                                    EditorGUILayout.BeginVertical("HelpBox");
                                                    {
                                                        EditorGUILayout.BeginHorizontal();
                                                        {
                                                            EditorGUILayout.LabelField("Scale", GUILayout.Width(100));
                                                            clip.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[i].minScale, GUILayout.Width(180));
                                                            GUILayout.FlexibleSpace();
                                                        }
                                                        EditorGUILayout.EndHorizontal();
                                                    }
                                                    EditorGUILayout.EndVertical();
                                                }

                                            }
                                            EditorGUILayout.EndVertical();

                                            EditorGUILayout.EndVertical();
                                        }

                                        GUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("[" + i + "]Attack Strength Type", GUILayout.Width(100));
                                            clip.StrengthType[i] = (AttackStrengthType)EditorGUILayout.EnumPopup(clip.StrengthType[i]);
                                            GUILayout.FlexibleSpace();
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("[" + i + "]데미지 비율", GUILayout.Width(80));
                                            clip.Damage[i] = EditorGUILayout.FloatField(clip.Damage[i]);
                                            if (clip.rangeAttackType == RangeAttackType.POINT_EXPLOSION)
                                            {
                                                EditorGUILayout.LabelField("[" + i + "] 거리", GUILayout.Width(45));
                                                clip.DamageRange[i] = EditorGUILayout.FloatField(clip.DamageRange[i]);
                                                EditorGUILayout.LabelField("[" + i + "] 각도", GUILayout.Width(45));
                                                clip.ExplosionAngle[i] = EditorGUILayout.FloatField(clip.ExplosionAngle[i]);
                                            }
                                            EditorGUILayout.LabelField("[" + i + "] Hit Delay", GUILayout.Width(75));
                                            clip.HitDelay[i] = EditorGUILayout.FloatField(clip.HitDelay[i]);
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                    GUILayout.EndVertical();
                                }
                            }
                            GUILayout.EndVertical();
                        }
                    }
                    else if (clip.isRepeatDamage)
                    {
                        GUILayout.BeginVertical("HelpBox");

                        GUILayout.BeginHorizontal();
                        {
                            if (clip.hitDelay.Count > 1)
                                EditorGUILayout.LabelField("총 딜레이 시간 : " + (clip.hitDelay[1] * clip.repeatCount), GUILayout.Width(150));
                            else
                                EditorGUILayout.LabelField("총 딜레이 시간 : - ", GUILayout.Width(150));
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();


                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("반복수", GUILayout.Width(150));
                            clip.repeatCount = EditorGUILayout.IntField(clip.repeatCount);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("시작 딜레이 시간", GUILayout.Width(150));
                            clip.repeatDelayTime = EditorGUILayout.FloatField(clip.repeatDelayTime);
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginVertical("Box");
                        {

                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Excute Only Zero Index TD", GUILayout.Width(150));
                                clip.onlyTimeDataIndexZero = EditorGUILayout.Toggle(clip.onlyTimeDataIndexZero);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                clip.timeDatas[0] = EditorGUILayout.ObjectField("Hit TimeData",clip.timeDatas[0],typeof(TimeData), false) as TimeData;
                            }
                            GUILayout.EndHorizontal();

                            foldOutHitEffects[0] = EditorGUILayout.Foldout(foldOutHitEffects[0], "Hit Effect Datas");
                            if (foldOutHitEffects[0])
                            {
                                EditorGUILayout.BeginVertical("HelpBox");

                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Hit Sound", GUILayout.Width(80));
                                    clip.HitEffects[0].effectSound = (SoundList)EditorGUILayout.EnumPopup("", clip.HitEffects[0].effectSound, GUILayout.Width(250));
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Hit Effect", GUILayout.Width(80));
                                    clip.HitEffects[0].effect = (EffectList)EditorGUILayout.EnumPopup("", clip.HitEffects[0].effect, GUILayout.Width(250));
                                    GUILayout.FlexibleSpace();
                                }
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginVertical("Box");
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                                        clip.IsHitRandom[0] = EditorGUILayout.Toggle(clip.IsHitRandom[0]);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    if (clip.IsHitRandom[0])
                                    {
                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                clip.HitEffects[0].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minPosition, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                clip.HitEffects[0].maxPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[0].maxPosition, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Rotation", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                clip.HitEffects[0].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minRotation, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                clip.HitEffects[0].maxRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[0].maxRotation, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Scale", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                clip.HitEffects[0].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minScale, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                clip.HitEffects[0].maxScale = EditorGUILayout.Vector3Field("", clip.HitEffects[0].maxScale, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                    else
                                    {
                                        clip.HitEffects[0].maxPosition = Vector3.zero;
                                        clip.HitEffects[0].maxRotation = Vector3.zero;
                                        clip.HitEffects[0].maxScale = Vector3.zero;
                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                                clip.HitEffects[0].minPosition = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minPosition, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Rotation", GUILayout.Width(100));
                                                clip.HitEffects[0].minRotation = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minRotation, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Scale", GUILayout.Width(100));
                                                clip.HitEffects[0].minScale = EditorGUILayout.Vector3Field("", clip.HitEffects[0].minScale, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();
                                    }

                                }
                                EditorGUILayout.EndVertical();

                                EditorGUILayout.EndVertical();
                            }

                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Attack Strength Type", GUILayout.Width(100));
                                clip.StrengthType[0] = (AttackStrengthType)EditorGUILayout.EnumPopup(clip.StrengthType[0]);
                                GUILayout.FlexibleSpace();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("데미지 비율", GUILayout.Width(80));
                                clip.Damage[0] = EditorGUILayout.FloatField(clip.Damage[0]);
                                if (clip.rangeAttackType == RangeAttackType.POINT_EXPLOSION)
                                {
                                    EditorGUILayout.LabelField("거리", GUILayout.Width(45));
                                    clip.DamageRange[0] = EditorGUILayout.FloatField(clip.DamageRange[0]);
                                    EditorGUILayout.LabelField("각도", GUILayout.Width(45));
                                    clip.ExplosionAngle[0] = EditorGUILayout.FloatField(clip.ExplosionAngle[0]);
                                }
                                EditorGUILayout.LabelField("Hit Delay", GUILayout.Width(75));
                                if (clip.hitDelay.Count > 1)
                                    clip.HitDelay[1] = EditorGUILayout.FloatField(clip.HitDelay[1]);
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();

                        if (clip.Damage.Count != clip.repeatCount)
                        {
                            for (int i = 0; i < clip.repeatCount; i++)
                            {
                                if (clip.damage.Count == clip.repeatCount) continue;

                                if (clip.StrengthType.Count < clip.repeatCount) clip.StrengthType.Add(AttackStrengthType.NONE);
                                else clip.StrengthType.RemoveAt(clip.StrengthType.Count - 1);

                                if (clip.Damage.Count < clip.repeatCount) clip.damage.Add(0f);
                                else clip.damage.RemoveAt(clip.damage.Count - 1);

                                if (clip.DamageRange.Count < clip.repeatCount) clip.DamageRange.Add(0f);
                                else clip.DamageRange.RemoveAt(clip.DamageRange.Count - 1);

                                if (clip.HitDelay.Count < clip.repeatCount) clip.HitDelay.Add(0f);
                                else clip.HitDelay.RemoveAt(clip.HitDelay.Count - 1);

                                if (clip.IsHitRandom.Count < clip.repeatCount) clip.IsHitRandom.Add(false);
                                else clip.IsHitRandom.RemoveAt(clip.IsHitRandom.Count - 1);

                                if (clip.ExplosionAngle.Count < clip.repeatCount) clip.ExplosionAngle.Add(0f);
                                else clip.ExplosionAngle.RemoveAt(clip.ExplosionAngle.Count - 1);

                                if (foldOutHitEffects.Count < clip.repeatCount) foldOutHitEffects.Add(false);
                                else foldOutHitEffects.RemoveAt(foldOutHitEffects.Count - 1);

                                if (clip.HitEffects.Count < clip.repeatCount) clip.HitEffects.Add(new RandomEffectInfo());
                                else clip.HitEffects.RemoveAt(clip.HitEffects.Count - 1);

                                if (foldOutRangeRequiredVariables.Count < clip.repeatCount) foldOutRangeRequiredVariables.Add(false);
                                else foldOutRangeRequiredVariables.RemoveAt(foldOutRangeRequiredVariables.Count - 1);
                            }
                        }

                        for (int i = 1; i < clip.repeatCount; i++)
                        {
                            if (clip.isRepeatDamage)
                            {
                                clip.Damage[i] = clip.Damage[0];
                                clip.strengthType[i] = clip.strengthType[0];
                                clip.damageRange[i] = clip.damageRange[0];
                                clip.HitDelay[i] = clip.HitDelay[1];
                                clip.IsHitRandom[i] = clip.IsHitRandom[0];
                                clip.ExplosionAngle[i] = clip.ExplosionAngle[0];
                                clip.HitEffects[i] = clip.HitEffects[0];
                            }

                        }
                        clip.hitDelay[0] = clip.repeatDelayTime;
                        GUILayout.EndVertical();
                    }

                }

                GUILayout.Space(20f);
            }

            GUILayout.BeginVertical();
            {
                dotTitleGuiStyle.fontStyle = FontStyle.Bold;
                dotTitleGuiStyle.normal.textColor = Color.white;
                dotTitleGuiStyle.fontSize = 15;
                EditorGUILayout.LabelField("Dot Datas", dotTitleGuiStyle);

                //Use Dot
                GUILayout.BeginVertical("HelpBox");
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Dot 데미지 사용", GUILayout.Width(100));
                        clip.UseDotDamage = EditorGUILayout.Toggle(clip.UseDotDamage);
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    //Dot 변수들
                    if (clip.UseDotDamage)
                    {
                        foldOutDotTitleRoot = EditorGUILayout.Foldout(foldOutDotTitleRoot, "Dot 데이터");
                        if (foldOutDotTitleRoot)
                        {
                            DotAttackInfo dotInfo = clip.DotInfo;
                            GUILayout.BeginVertical("HelpBox");
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Is Skill", GUILayout.Width(100));
                                    clip.dotInfo.isSkill = EditorGUILayout.Toggle(clip.dotInfo.isSkill);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("개별 영역에 쿨타임", GUILayout.Width(130));
                                    clip.dotInfo.eachDotCoolTime = EditorGUILayout.Toggle(clip.dotInfo.eachDotCoolTime);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();

                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Draw Dot Line Color", GUILayout.Width(150));
                                    dotInfo.DrawDotLineColor = EditorGUILayout.ColorField(dotInfo.DrawDotLineColor);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("Draw End Point Color", GUILayout.Width(150));
                                    dotInfo.DrawDotEndColor = EditorGUILayout.ColorField(dotInfo.DrawDotEndColor);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("최대 타겟 수", GUILayout.Width(150));
                                    dotInfo.MaxTargetCount = EditorGUILayout.IntField(dotInfo.MaxTargetCount);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("유지 시간", GUILayout.Width(150));
                                    dotInfo.StayTime = EditorGUILayout.FloatField(dotInfo.StayTime);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("등록 타입", GUILayout.Width(150));
                                    dotInfo.RegisterDotType = (DotAttackInfo.DotRegisterType)EditorGUILayout.EnumPopup(dotInfo.RegisterDotType);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();

                                if (dotInfo.RegisterDotType == DotAttackInfo.DotRegisterType.MULTI)
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("타겟 재등록 시간", GUILayout.Width(150));
                                        dotInfo.MultiDelayCoolTime = EditorGUILayout.FloatField(dotInfo.MultiDelayCoolTime);
                                        GUILayout.FlexibleSpace();
                                    }
                                    GUILayout.EndHorizontal();
                                }

                                EditorGUILayout.Space(10f);
                                EditorGUILayout.LabelField("Detect 부분");
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("위치 등록 딜레이 시간", GUILayout.Width(150));
                                    dotInfo.PointDelayTime = EditorGUILayout.FloatField(dotInfo.PointDelayTime);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                {
                                    EditorGUILayout.LabelField("위치 Box Size", GUILayout.Width(150));
                                    dotInfo.DetectBoxSize = EditorGUILayout.Vector2Field("", dotInfo.DetectBoxSize);
                                    GUILayout.FlexibleSpace();
                                }
                                GUILayout.EndHorizontal();

                                EditorGUILayout.BeginVertical("Box");
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("파티클 유지 시간 타입", GUILayout.Width(150));
                                        dotInfo.particleType = (ParticleLifeType)EditorGUILayout.EnumPopup(dotInfo.particleType);
                                        GUILayout.FlexibleSpace();
                                    }
                                    GUILayout.EndHorizontal();

                                    if (dotInfo.particleType != ParticleLifeType.NONE && dotInfo.particleType != ParticleLifeType.DURATION_LIFETIME_EACH)
                                    {
                                        GUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("파티클 유지 시간", GUILayout.Width(150));
                                            dotInfo.particleStayTime = EditorGUILayout.FloatField(dotInfo.particleStayTime);
                                            GUILayout.FlexibleSpace();
                                        }
                                        GUILayout.EndHorizontal();
                                    }
                                    else if (dotInfo.particleType == ParticleLifeType.DURATION_LIFETIME_EACH)
                                    {
                                        GUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("파티클 Duration 유지 시간", GUILayout.Width(150));
                                            dotInfo.particleDurationTime = EditorGUILayout.FloatField(dotInfo.particleDurationTime);
                                            GUILayout.FlexibleSpace();
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.BeginHorizontal();
                                        {
                                            EditorGUILayout.LabelField("파티클 LifeTime 유지 시간", GUILayout.Width(150));
                                            dotInfo.particleStayTime = EditorGUILayout.FloatField(dotInfo.particleStayTime);
                                            GUILayout.FlexibleSpace();
                                        }
                                        GUILayout.EndHorizontal();
                                    }

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("Sound", GUILayout.Width(100));
                                        dotInfo.dotEffect.effectSound = (SoundList)EditorGUILayout.EnumPopup(dotInfo.dotEffect.effectSound);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("Effect", GUILayout.Width(100));
                                        dotInfo.dotEffect.effect = (EffectList)EditorGUILayout.EnumPopup(dotInfo.dotEffect.effect);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                                        dotInfo.isHitRandom = EditorGUILayout.Toggle(dotInfo.isHitRandom);
                                        GUILayout.FlexibleSpace();
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    if (dotInfo.isHitRandom)
                                    {
                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                dotInfo.dotEffect.minPosition = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minPosition, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                dotInfo.dotEffect.maxPosition = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.maxPosition, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Rotation", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                dotInfo.dotEffect.minRotation = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minRotation, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                dotInfo.dotEffect.maxRotation = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.maxRotation, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.LabelField("Scale", GUILayout.Width(150));
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                dotInfo.dotEffect.minScale = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minScale, GUILayout.Width(180));
                                                GUILayout.Label(" - ");
                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                dotInfo.dotEffect.maxScale = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.maxScale, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();
                                    }
                                    else
                                    {
                                        dotInfo.dotEffect.maxPosition = Vector3.zero;
                                        dotInfo.dotEffect.maxRotation = Vector3.zero;
                                        dotInfo.dotEffect.maxScale = Vector3.zero;
                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                                dotInfo.dotEffect.minPosition = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minPosition, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Rotation", GUILayout.Width(100));
                                                dotInfo.dotEffect.minRotation = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minRotation, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();

                                        EditorGUILayout.BeginVertical("HelpBox");
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Scale", GUILayout.Width(100));
                                                dotInfo.dotEffect.minScale = EditorGUILayout.Vector3Field("", dotInfo.dotEffect.minScale, GUILayout.Width(180));
                                                GUILayout.FlexibleSpace();
                                            }
                                            EditorGUILayout.EndHorizontal();
                                        }
                                        EditorGUILayout.EndVertical();
                                    }

                                }
                                EditorGUILayout.EndVertical();

                                //Dot Data 추가 삭제 버튼
                                GUILayout.BeginHorizontal();
                                {
                                    if (GUILayout.Button("추가"))
                                    {
                                        foldOutDotEffectRequiredVariables.Add(true);
                                        foldOutDotRequiredVariables.Add(true);
                                        dotInfo.HitSounds.Add(SoundList.None);
                                        dotInfo.HitEffects.Add(new RandomEffectInfo());
                                        dotInfo.StrengthTypes.Add(AttackStrengthType.NONE);
                                        dotInfo.Damage.Add(0);
                                        dotInfo.HitDelay.Add(0);
                                        dotInfo.isRandom.Add(false);
                                    }
                                    if (GUILayout.Button("삭제"))
                                    {
                                        if (foldOutDotRequiredVariables.Count > 0)
                                            foldOutDotRequiredVariables.RemoveAt(foldOutDotRequiredVariables.Count - 1);
                                        if (foldOutDotEffectRequiredVariables.Count > 0)
                                            foldOutDotEffectRequiredVariables.RemoveAt(foldOutDotEffectRequiredVariables.Count - 1);

                                        if (dotInfo.HitSounds.Count > 0)
                                            dotInfo.HitSounds.RemoveAt(dotInfo.HitSounds.Count - 1);
                                        if (dotInfo.HitEffects.Count > 0)
                                            dotInfo.HitEffects.RemoveAt(dotInfo.HitEffects.Count - 1);
                                        if (dotInfo.StrengthTypes.Count > 0)
                                            dotInfo.StrengthTypes.RemoveAt(dotInfo.StrengthTypes.Count - 1);
                                        if (dotInfo.Damage.Count > 0)
                                            dotInfo.Damage.RemoveAt(dotInfo.Damage.Count - 1);
                                        if (dotInfo.HitDelay.Count > 0)
                                            dotInfo.HitDelay.RemoveAt(dotInfo.HitDelay.Count - 1);
                                        if (dotInfo.isRandom.Count > 0)
                                            dotInfo.isRandom.RemoveAt(dotInfo.isRandom.Count - 1);

                                    }
                                }
                                GUILayout.EndHorizontal();

                                foldOutRootDotRequiredVariable = EditorGUILayout.Foldout(foldOutRootDotRequiredVariable, "Dot 데이터");
                                if (foldOutRootDotRequiredVariable && foldOutDotRequiredVariables.Count > 0)
                                {
                                    GUILayout.BeginVertical("Box");
                                    {
                                        for (int i = 0; i < foldOutDotRequiredVariables.Count; i++)
                                        {
                                            foldOutDotEffectRequiredVariables[i] = EditorGUILayout.Foldout(foldOutDotEffectRequiredVariables[i], "Hit Effect Datas");
                                            if (foldOutDotEffectRequiredVariables[i])
                                            {
                                                EditorGUILayout.BeginVertical("HelpBox");

                                                EditorGUILayout.BeginHorizontal();
                                                {
                                                    EditorGUILayout.LabelField("Hit Sound", GUILayout.Width(80));
                                                    dotInfo.HitEffects[i].effectSound = (SoundList)EditorGUILayout.EnumPopup("", dotInfo.HitEffects[i].effectSound, GUILayout.Width(250));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();
                                                EditorGUILayout.BeginHorizontal();
                                                {
                                                    EditorGUILayout.LabelField("Hit Effect", GUILayout.Width(80));
                                                    dotInfo.HitEffects[i].effect = (EffectList)EditorGUILayout.EnumPopup("", dotInfo.HitEffects[i].effect, GUILayout.Width(250));
                                                    GUILayout.FlexibleSpace();
                                                }
                                                EditorGUILayout.EndHorizontal();


                                                EditorGUILayout.BeginVertical("Box");
                                                {
                                                    EditorGUILayout.BeginHorizontal();
                                                    {
                                                        EditorGUILayout.LabelField("Is Random", GUILayout.Width(100));
                                                        dotInfo.isRandom[i] = EditorGUILayout.Toggle(dotInfo.isRandom[i]);
                                                        GUILayout.FlexibleSpace();
                                                    }
                                                    EditorGUILayout.EndHorizontal();

                                                    if (dotInfo.isRandom[i])
                                                    {
                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.LabelField("Position", GUILayout.Width(150));
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minPosition, GUILayout.Width(180));
                                                                GUILayout.Label(" - ");
                                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].maxPosition = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].maxPosition, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();

                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.LabelField("Rotation", GUILayout.Width(150));
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minRotation, GUILayout.Width(180));
                                                                GUILayout.Label(" - ");
                                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].maxRotation = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].maxRotation, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();

                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.LabelField("Scale", GUILayout.Width(150));
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Min", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minScale, GUILayout.Width(180));
                                                                GUILayout.Label(" - ");
                                                                EditorGUILayout.LabelField("Max", GUILayout.Width(30));
                                                                dotInfo.HitEffects[i].maxScale = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].maxScale, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();
                                                    }
                                                    else
                                                    {
                                                        dotInfo.HitEffects[i].maxPosition = Vector3.zero;
                                                        dotInfo.HitEffects[i].maxRotation = Vector3.zero;
                                                        dotInfo.HitEffects[i].maxScale = Vector3.zero;
                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Position", GUILayout.Width(100));
                                                                dotInfo.HitEffects[i].minPosition = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minPosition, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();

                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Rotation", GUILayout.Width(100));
                                                                dotInfo.HitEffects[i].minRotation = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minRotation, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();

                                                        EditorGUILayout.BeginVertical("HelpBox");
                                                        {
                                                            EditorGUILayout.BeginHorizontal();
                                                            {
                                                                EditorGUILayout.LabelField("Scale", GUILayout.Width(100));
                                                                dotInfo.HitEffects[i].minScale = EditorGUILayout.Vector3Field("", dotInfo.HitEffects[i].minScale, GUILayout.Width(180));
                                                                GUILayout.FlexibleSpace();
                                                            }
                                                            EditorGUILayout.EndHorizontal();
                                                        }
                                                        EditorGUILayout.EndVertical();
                                                    }

                                                }
                                                EditorGUILayout.EndVertical();

                                                EditorGUILayout.EndVertical();
                                            }

                                            GUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Attack Strength Type", GUILayout.Width(100));
                                                dotInfo.StrengthTypes[i] = (AttackStrengthType)EditorGUILayout.EnumPopup(dotInfo.StrengthTypes[i]);
                                                GUILayout.FlexibleSpace();
                                            }
                                            GUILayout.EndHorizontal();
                                            GUILayout.BeginHorizontal();
                                            {
                                                EditorGUILayout.LabelField("Damage", GUILayout.Width(100));
                                                dotInfo.Damage[i] = EditorGUILayout.FloatField(dotInfo.Damage[i]);
                                                GUILayout.FlexibleSpace();
                                                EditorGUILayout.LabelField("Hit Delay", GUILayout.Width(100));
                                                dotInfo.HitDelay[i] = EditorGUILayout.FloatField(dotInfo.HitDelay[i]);
                                            }
                                            GUILayout.EndHorizontal();
                                        }
                                        GUILayout.EndVertical();
                                    }
                                }
                            }
                            GUILayout.EndVertical();
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                afterAttackInfoGuiStyle.fontStyle = FontStyle.Bold;
                afterAttackInfoGuiStyle.normal.textColor = Color.white;
                afterAttackInfoGuiStyle.fontSize = 15;
                EditorGUILayout.LabelField("After Attack Info", afterAttackInfoGuiStyle);

                clip.AfterAttackInfo = EditorGUILayout.ObjectField("AfterAttackInfo So", clip.AfterAttackInfo, typeof(AfterColisionRangeAttackInfo), false) as AfterColisionRangeAttackInfo;
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();

    
    }



    private string[] GetLayerNames()
    {
        List<string> names = new List<string>();

        for (int i = 0; i < 32; i++)
        {
            if (LayerMask.LayerToName(i) != string.Empty)
                names.Add(LayerMask.LayerToName(i));
        }

        return names.ToArray();
    }
}

