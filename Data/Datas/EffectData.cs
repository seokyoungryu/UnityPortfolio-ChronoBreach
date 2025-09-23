using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class EffectData : BaseData
{
    public EffectClip[] effectClips = new EffectClip[0];
    private string enumName = "EffectList";
    public int realIndex = 0;


    [Header("CSV Path")]
    private string csvFilePath = "";    // application.datapath + dataDirectory + csvFolderPath + csvFileName;
    private string csvFolderPath = "/CSV/";
    private string csvFileName = "EffectData.csv";
    private string dataPath = "Data/CSV/EffectData";

    public void LoadData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        string loadText;
        TextAsset asset = Resources.Load(dataPath) as TextAsset;
        effectClips = new EffectClip[0];
        if (asset == null)
            return;
        else
            loadText = asset.text;

        string[] splitEnter = loadText.Split('\n');

        for (int i = 1; i < splitEnter.Length - 1; i++)
        {
            string[] splitTab = splitEnter[i].Split(',');
            EffectClip clip = new EffectClip();
            clip.id = int.Parse(splitTab[0]);
            clip.effectName = splitTab[1];
            clip.effectType = (EffectType)int.Parse(splitTab[2]);
            clip.effectPath = splitTab[3];
            clip.applyChildScale = bool.Parse(splitTab[4]);
            clip.LoadEffectPrefab();
            effectClips = ArrayHelper.Add(clip, effectClips);
        }

        for (int i = 0; i < effectClips.Length; i++)
        {
            effectClips[i].LoadEffectPrefab();
        }


    }

    public void SaveData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        using (StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
        {
            string line = "ID, EffectName, EffectType, EffectPath,ApplyChildScale";
            sw.WriteLine(line);

            for (int i = 0; i < effectClips.Length; i++)
            {
                line = effectClips[i].id + "," + effectClips[i].effectName + "," + (int)(effectClips[i].effectType) + "," + effectClips[i].effectPath + "," + effectClips[i].applyChildScale;
                sw.WriteLine(line);
            }

        }
    }


    public override void AddData(string newName)
    {
        if (effectClips == null || effectClips.Length == 0)
        {
            realIndex = 0;
            effectClips = new EffectClip[] { new EffectClip(realIndex, newName) };
            realIndex += 1;
        }
        else
        {
            effectClips = ArrayHelper.Add(new EffectClip(realIndex, newName), effectClips);
            realIndex += 1;
        }
        UpdateRealId();
    }

    public override void Copy(int index)
    {
        if (index < 0 || index > effectClips.Length)
            return;

        EffectClip copyClip = effectClips[index];
        EffectClip tmpClip = new EffectClip(realIndex , copyClip.effectName);
        tmpClip.effectType = copyClip.effectType;
        tmpClip.effectPath = copyClip.effectPath;
        tmpClip.effectName = copyClip.effectName;
        tmpClip.applyChildScale = copyClip.applyChildScale;

        realIndex += 1;
        tmpClip.LoadEffectPrefab();
        effectClips = ArrayHelper.Add(tmpClip, effectClips);
        UpdateRealId();
    }

    public override void RemoveData(int index)
    {
        if (index > effectClips.Length)
            return;

        effectClips = ArrayHelper.Remove(index, effectClips);
        UpdateRealId();
    }


    public void UpdateRealId()
    {
        if (effectClips == null || effectClips.Length < 0)
            return;

        for (int i = 0; i < effectClips.Length; i++)
        {
            effectClips[i].id = i;
        }
        realIndex = effectClips.Length;
    }

    public string[] ReturnClipsName(EffectClip[] clips)
    {
        string[] names = new string[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            names[i] = clips[i].effectName;
        }

        return names;
    }

#if UNITY_EDITOR
    public void CreateEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();
        string inspectorName = string.Empty;
        for (int i = 0; i < effectClips.Length; i++)
        {
            if (effectClips[i].effectName != string.Empty)
            {
                sb.AppendLine("         " + GetInspectorName(effectClips[i]));
                sb.AppendLine("         " + effectClips[i].effectName + " = " + i + ",");
            }

        }

        EditorHelper.CreateEnumList(enumName, sb);
    }

#endif
    private string GetInspectorName(EffectClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.effectType)
        {
            case EffectType.NONE: retName = string.Empty; break;
            case EffectType.HIT: retName = retName.Replace("*", "Hit/" + clip.effectName); break;
            case EffectType.PROJECTILE: retName = retName.Replace("*", "Projectile/" + clip.effectName); break;
            case EffectType.COLLISION: retName = retName.Replace("*", "Collision/" + clip.effectName); break;
            case EffectType.SLASH: retName = retName.Replace("*", "Slash/" + clip.effectName); break;
            case EffectType.ATTACKSKILL: retName = retName.Replace("*", "AttackSkill/" + clip.effectName); break;
            case EffectType.EXPLOSION: retName = retName.Replace("*", "Explosion/" + clip.effectName); break;
            case EffectType.MAGIC: retName = retName.Replace("*", "Magic/" + clip.effectName); break;
            case EffectType.CASTING: retName = retName.Replace("*", "Casting/" + clip.effectName); break;
            case EffectType.FLASH: retName = retName.Replace("*", "Flash/" + clip.effectName); break;
            case EffectType.BUFF: retName = retName.Replace("*", "Buff/" + clip.effectName); break;
            case EffectType.ETC: retName = retName.Replace("*", "Etc/" + clip.effectName); break;

        }

        return retName.Replace('|','"');
    }

}
