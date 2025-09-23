using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public List<SkinnedMeshRenderer> skinnedRenderers;
    public List<MeshRenderer> meshRenderers;

    private SkinnedMeshRenderer[] tempRenderer;
    private MeshRenderer[] tempMeshRenderer;

    public Color damagedColor;
    public Color defenseColor;
    public Color standingColor;
    public Color evasionColor;

    public float startIntensity = 3f; // 시작할 강도 (0 ~ 1 범위)
    public float endIntensity = 1f; // 도달할 강도 (0 ~ 1 범위)
    public float duration = 2f; // 전체 변화에 걸릴 시간 (초)
    public float elapsedTime = 0f; // 경과 시간

    public bool isSkinnedNormalMaterial = false;


    public float TestIntensity = 0f;
    private void Start()
    {
        tempRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        tempMeshRenderer = GetComponentsInChildren<MeshRenderer>();

        foreach (SkinnedMeshRenderer skinned in tempRenderer)
        {
            if (skinned.gameObject.activeInHierarchy)
                skinnedRenderers.Add(skinned);
        }
        foreach (MeshRenderer skinned in tempMeshRenderer)
        {
            if (skinned.gameObject.activeInHierarchy)
                meshRenderers.Add(skinned);
        }
    }

    public void ExcuteDamagedColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(damagedColor));
    }

    public void ExcuteDefenseColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(defenseColor));
    }

    public void ExcuteStandinfColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(standingColor));
    }

    public void ExcuteEvasionColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorChange(evasionColor));
    }

    IEnumerator ColorChange(Color color)
    {
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // 경과 시간을 [0, 1] 범위로 정규화

            float lerp = Mathf.Lerp(startIntensity, endIntensity, t); // 선형 보간을 통해 현재 강도 계산
            Color TestColor = color * lerp;

            for (int i = 0; i < skinnedRenderers.Count; i++)
            {
                if (!isSkinnedNormalMaterial)
                    skinnedRenderers[i].material.SetColor("_AllColor", TestColor);
                else
                    skinnedRenderers[i].material.color = TestColor;
            }
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].material.SetColor("_AllColor", TestColor);
                meshRenderers[i].material.color = TestColor;
            }

            yield return null;
        }

        for (int i = 0; i < skinnedRenderers.Count; i++)
        {
            if (!isSkinnedNormalMaterial)
                skinnedRenderers[i].material.SetColor("_AllColor", Color.white);
            else
                skinnedRenderers[i].material.color = Color.white;
        }

        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.SetColor("_AllColor", Color.white);
            meshRenderers[i].material.color = Color.white;
        }
    }

    public void ResetEffect()
    {
        for (int i = 0; i < skinnedRenderers.Count; i++)
        {
            if (!isSkinnedNormalMaterial)
                skinnedRenderers[i].material.SetColor("_AllColor", Color.white);
            else
            {
                skinnedRenderers[i].material.color = Color.white;
            }
        }
        for (int i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i].material.SetColor("_AllColor", Color.white);
            meshRenderers[i].material.color = Color.white;
        }
    }

}


