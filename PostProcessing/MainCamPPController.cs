using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.PostProcessing;

public enum PPType
{
    COLOR_GRADING = 0,
    MOTIONBLUR = 10,
    MOTIONBLUR_DAMAGED_WEAK ,
    MOTIONBLUR_DAMAGED_NORMAL,
    MOTIONBLUR_DAMAGED_STRONG,

    BLOOM = 20,
    AMBIENT_OCCLUSION = 30,
    CHROMATIC_ABERRATION= 40,
    DEPTH_OF_FIELD = 50,
    DEPTH_OF_FIELD_WEAK,
    DEPTH_OF_FIELD_NORMAL,
    DEPTH_OF_FIELD_STRONG,

    VIGNETTE_FADE_IN = 60,
    VIGENTTE_FADE_OUT,
}

public class MainCamPPController : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;

    [SerializeField] private float initMotionBlur = 200f;
    [SerializeField] private float weakDmgMotionBlur = 230f;
    [SerializeField] private float normalDmgMotionBlur = 250f;
    [SerializeField] private float strongDmgMotionBlur = 285f;

    private float originInitMotionBlur;
    private float originWeakDmgMotionBlur;
    private float originNormalDmgMotionBlur;
    private float originStrongDmgMotionBlur;


    private ColorGrading colorGrading;
    private MotionBlur motionBlur;
    private Bloom bloom;
    private AmbientOcclusion ambientOcclusion;
    private ChromaticAberration chromaticAberration;
    private DepthOfField depthOf;
    private Vignette vignette;

    public PPInfo[] infos;

    private Coroutine motionBlurCoroutine;
    private Coroutine depthOfFieldCoroutine;
    private Coroutine chromaticAberrationCoroutine;
    private Coroutine vignetteCoroutine;


    public float CurrentInitMB => initMotionBlur;
    public float CurrentWeakMB => weakDmgMotionBlur;
    public float CurrentNormalMB => normalDmgMotionBlur;
    public float CurrentStrongMB => strongDmgMotionBlur;


    private void Awake()
    {
        volume.profile.TryGetSettings(out colorGrading);
        volume.profile.TryGetSettings(out motionBlur);
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out ambientOcclusion);
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out depthOf);
        volume.profile.TryGetSettings(out vignette);

        originInitMotionBlur = initMotionBlur;
        originWeakDmgMotionBlur = weakDmgMotionBlur;
        originNormalDmgMotionBlur = normalDmgMotionBlur;
        originStrongDmgMotionBlur = strongDmgMotionBlur;

        SetInitMotionBlurShutterAngle(initMotionBlur);
    }


    public void ResetInitMotionBlur() => initMotionBlur = originInitMotionBlur;
    public void ResetWeakMotionBlur() => weakDmgMotionBlur = originInitMotionBlur;
    public void ResetNormalMotionBlur() => normalDmgMotionBlur = originInitMotionBlur;
    public void ResetStrongMotionBlur() => strongDmgMotionBlur = originInitMotionBlur;
    public void SetInitMotionBlur(float value) => initMotionBlur = value;
    public void SetWeakMotionBlur(float value) => weakDmgMotionBlur = value;
    public void SetNormalMotionBlur(float value) => normalDmgMotionBlur = value;
    public void SetStrongMotionBlur(float value) => strongDmgMotionBlur = value;


    private IEnumerator AnimateCurve_Co(AnimationCurve curve, float duration, Action<float> onUpdate)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float value = curve.Evaluate(timer / duration);
            onUpdate?.Invoke(value);
            yield return null;
        }
    }

    public void SetInitMotionBlurShutterAngle(float value)
    {
        initMotionBlur = value;
        motionBlur.shutterAngle.value = initMotionBlur;
    }

    public void AnimateMotionBlurAngle(AnimationCurve curve, PPType ppType, float duration)
    {
        if (motionBlurCoroutine != null)
            StopCoroutine(motionBlurCoroutine);

        Keyframe[] key = curve.keys;
        key[0].value = key[2].value = initMotionBlur;

        if (ppType == PPType.MOTIONBLUR) key[1].value = initMotionBlur;
        else if (ppType == PPType.MOTIONBLUR_DAMAGED_WEAK) key[1].value = weakDmgMotionBlur ;
        else if (ppType == PPType.MOTIONBLUR_DAMAGED_NORMAL) key[1].value = normalDmgMotionBlur;
        else if (ppType == PPType.MOTIONBLUR_DAMAGED_STRONG) key[1].value = strongDmgMotionBlur;

        motionBlurCoroutine = StartCoroutine(AnimateCurve_Co(curve, duration, (value) => motionBlur.shutterAngle.value = value));
    }


    public void AnimateDepthOfAperture(AnimationCurve curve, float duration)
    {
        if (depthOfFieldCoroutine != null)
            StopCoroutine(depthOfFieldCoroutine);

        depthOfFieldCoroutine = StartCoroutine(AnimateCurve_Co(curve, duration, (value) => depthOf.aperture.value = value));
    }

    public void AnimateChromaticAmbIntensity(AnimationCurve curve, float duration)
    {
        if (chromaticAberrationCoroutine != null)
            StopCoroutine(chromaticAberrationCoroutine);

        chromaticAberrationCoroutine = StartCoroutine(AnimateCurve_Co(curve, duration, (value) => chromaticAberration.intensity.value = value));
    }

    public void AnimateVignette(AnimationCurve curve, float duration)
    {
        if (vignetteCoroutine != null)
            StopCoroutine(vignetteCoroutine);

        vignetteCoroutine = StartCoroutine(AnimateCurve_Co(curve, duration, (value) => vignette.intensity.value = value));
    }


    public void ExcuteAnimate(PPType type)
    {
        PPInfo info = GetInfo(type);
        if (info == null) return;

        switch (info.ppType)
        {
            case PPType.COLOR_GRADING:
                break;
            case PPType.MOTIONBLUR:
            case PPType.MOTIONBLUR_DAMAGED_WEAK:
            case PPType.MOTIONBLUR_DAMAGED_NORMAL:
            case PPType.MOTIONBLUR_DAMAGED_STRONG:
                AnimateMotionBlurAngle(info.curve,type, info.duration);
                break;
            case PPType.BLOOM:
                break;
            case PPType.AMBIENT_OCCLUSION:
                break;
            case PPType.CHROMATIC_ABERRATION:
                AnimateChromaticAmbIntensity(info.curve, info.duration);
                break;
            case PPType.DEPTH_OF_FIELD:
            case PPType.DEPTH_OF_FIELD_WEAK:
            case PPType.DEPTH_OF_FIELD_NORMAL:
            case PPType.DEPTH_OF_FIELD_STRONG:
                AnimateDepthOfAperture(info.curve, info.duration);
                break;
            case PPType.VIGNETTE_FADE_IN:
            case PPType.VIGENTTE_FADE_OUT:
                AnimateVignette(info.curve, info.duration);
                break;
        }
    }


    private PPInfo GetInfo(PPType type)
    {
        for (int i = 0; i < infos.Length; i++)
            if (infos[i].ppType == type)
                return infos[i];
        return null;
    }

}


[System.Serializable]
public class PPInfo
{
    public PPType ppType;
    public AnimationCurve curve;
    public float duration;
}
