using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SoundData : BaseData
{
    public SoundClip[] soundClips = new SoundClip[0];
    private string enumName = "SoundList";
    int realIndex = 0;


    [Header("CSV Path")]
    private string csvFilePath = "";    // application.datapath + dataDirectory + csvFolderPath + csvFileName;
    private string csvFolderPath = "/CSV/";
    private string csvFileName = "SoundData.csv";
    private string dataPath = "Data/CSV/SoundData";



    public void LoadData()
    {
        string assetTemp;
        TextAsset asset = Resources.Load(dataPath) as TextAsset;
        if (asset == null)
            return;
        else
            assetTemp = asset.text;

        soundClips = new SoundClip[0];
        string[] splitEnter = assetTemp.Split('\n');

        for (int i = 1; i < splitEnter.Length-1; i++)
        {
            string[] splitTab = splitEnter[i].Split(',');
            SoundClip clip = new SoundClip();
            clip.id = int.Parse(splitTab[0]);
            clip.clipName = splitTab[1];
            clip.playType = (SoundPlayType)int.Parse(splitTab[2]);
            clip.isAwakePlay = bool.Parse(splitTab[3]);
            clip.isLoop = bool.Parse(splitTab[4]);
            clip.pitch = float.Parse(splitTab[5]);
            clip.spatialBlend = float.Parse(splitTab[6]);
            clip.minDistance = float.Parse(splitTab[7]);
            clip.maxDistance = float.Parse(splitTab[8]);
            clip.clipPath = splitTab[9];
            clip.playTypeBgm = (SoundPlayTypeBGM)int.Parse(splitTab[10]);
            clip.playTypeEffect = (SoundPlayTypeEffect)int.Parse(splitTab[11]);
            clip.playTypeUI = (SoundPlayTypeUI)int.Parse(splitTab[12]);
            clip.playTypeETC = (SoundPlayTypeETC)int.Parse(splitTab[13]);
            clip.volume = float.Parse(splitTab[14]);
            clip.LoadClipData();
            soundClips = ArrayHelper.Add(clip, soundClips);
        }

    }

    public void SaveData()
    {
        csvFilePath = Application.dataPath + dataDirectory + csvFolderPath + csvFileName;
        using (StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.UTF8))
        {
            string line1 = "ID, Name, PlayType, IsAwakePlay, IsLoop, Pitch, SpatialBlend, Min Distance, Max Distance, ClipPath , BGMType,EffectType,UIType,ETCType, Volume";
            sw.WriteLine(line1);

            for (int i = 0; i < soundClips.Length; i++)
            {
                line1 = soundClips[i].id.ToString() + "," + soundClips[i].clipName + "," + (int)soundClips[i].playType + "," + soundClips[i].isAwakePlay + ","
                    + soundClips[i].isLoop + "," + soundClips[i].pitch.ToString() + "," + soundClips[i].spatialBlend.ToString() + "," + soundClips[i].minDistance + ","
                    + soundClips[i].maxDistance + "," + soundClips[i].clipPath + "," + (int)soundClips[i].playTypeBgm + "," + (int)soundClips[i].playTypeEffect + "," 
                    + (int)soundClips[i].playTypeUI  + "," + (int)soundClips[i].playTypeETC + "," + soundClips[i].volume;

                sw.WriteLine(line1);
            }
        }

    }

    public void AddData(SoundClip clip)
    {
        if (soundClips == null || soundClips.Length < 0)
        {
            realIndex = 0;
            soundClips = new SoundClip[] { clip };
            realIndex += 1;
        }
        else
        {
            soundClips = ArrayHelper.Add(clip, soundClips);
            realIndex += 1;
        }

    }


    public override void AddData(string newName)
    {
        if (soundClips == null || soundClips.Length < 0)
        {
            realIndex = 0;
            soundClips = new SoundClip[] { new SoundClip(realIndex, newName) };
            realIndex += 1;
        }
        else
        {
            soundClips = ArrayHelper.Add(new SoundClip(realIndex, newName), soundClips);
            realIndex += 1;
        }

    }

    public override void Copy(int index)
    {
        if (index < 0 || index > soundClips.Length)
            return;

        SoundClip originClip = soundClips[index];
        SoundClip newClip = new SoundClip(realIndex, originClip.clipName);
        newClip.clipPath = originClip.clipPath;
        newClip.isAwakePlay = originClip.isAwakePlay;
        newClip.isLoop = originClip.isLoop;
        newClip.minDistance = originClip.minDistance;
        newClip.maxDistance = originClip.maxDistance;
        newClip.playType = originClip.playType;
        newClip.spatialBlend = originClip.spatialBlend;
        newClip.volume = originClip.volume;
        newClip.pitch = originClip.pitch;

        newClip.LoadClipData();
        Debug.Log(newClip.clipFullPath);

        realIndex += 1;
        soundClips = ArrayHelper.Add(newClip, soundClips);

    }

    public override void RemoveData(int index)
    {
        if (index < 0 || index > soundClips.Length)
            return;

        soundClips = ArrayHelper.Remove(index, soundClips);

    }


    public void UpdateRealIndex()
    {
        if (soundClips == null || soundClips.Length < 0)
            return;

        realIndex = 0;
        for (int i = 0; i < soundClips.Length; i++)
        {
            soundClips[i].id = i;
        }
        realIndex = soundClips.Length;
    }

    public string[] ReturnSoundClipName(SoundClip[] clips)
    {
        string[] names = new string[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            names[i] = clips[i].clipName;
        }

        return names;
    }

#if UNITY_EDITOR
    public void CreateEnum()
    {
        StringBuilder sb = new StringBuilder();
        string inspectorName = string.Empty;
        sb.AppendLine();
        for (int i = 0; i < soundClips.Length; i++)
        {
            sb.AppendLine("       " + GetInspectorName(soundClips[i]));
            sb.AppendLine("       " + soundClips[i].clipName + " = " + i + ",");
        }

        EditorHelper.CreateEnumList(enumName, sb);

    }
#endif



    private string GetInspectorName(SoundClip clip)
    {
        string retName = "[InspectorName(|*|)]";

        switch (clip.playType)
        {
            case SoundPlayType.BGM:
                if (clip.playTypeBgm == SoundPlayTypeBGM.NONE) retName = retName.Replace("*", "BGM/NONE/" + clip.clipName);
                else if (clip.playTypeBgm == SoundPlayTypeBGM.TITLE) retName = retName.Replace("*", "BGM/Title/" + clip.clipName);
                else if (clip.playTypeBgm == SoundPlayTypeBGM.MAIN) retName = retName.Replace("*", "BGM/Main/" + clip.clipName);
                else if (clip.playTypeBgm == SoundPlayTypeBGM.BATTLE) retName = retName.Replace("*", "BGM/Battle/" + clip.clipName);
                else if (clip.playTypeBgm == SoundPlayTypeBGM.DUNGEON) retName = retName.Replace("*", "BGM/Dungeon/" + clip.clipName);

                break;
            case SoundPlayType.Effect:
                if (clip.playTypeEffect == SoundPlayTypeEffect.NONE) retName = retName.Replace("*", "Effect/NONE/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.MONSTER_ROAR) retName = retName.Replace("*", "Effect/Monster_Roar/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.MONSTER_HURT) retName = retName.Replace("*", "Effect/Monster_Hurt/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.MONSTER_ATTACK_VOICE) retName = retName.Replace("*", "Effect/Monster_AttackVoice/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.MONSTER_DIE) retName = retName.Replace("*", "Effect/Monster_Die/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.MONSTER_FINDNEAR) retName = retName.Replace("*", "Effect/Monster_FindNear/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.ATTACK_BOW) retName = retName.Replace("*", "Effect/Attack_Bow/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.ATTACK_MAGIC_SLASH) retName = retName.Replace("*", "Effect/Attack_MagicSlash/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.ATTACK_SWING) retName = retName.Replace("*", "Effect/Attack_Swing/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.BUFF) retName = retName.Replace("*", "Effect/Buff/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.COUNTER) retName = retName.Replace("*", "Effect/Counter/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.DEFENSE) retName = retName.Replace("*", "Effect/Defense/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.DOORS) retName = retName.Replace("*", "Effect/Doors/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.HIT) retName = retName.Replace("*", "Effect/Hit/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.PROJECTILE) retName = retName.Replace("*", "Effect/Projectile/" + clip.clipName);
                else if (clip.playTypeEffect == SoundPlayTypeEffect.EXPLOSION) retName = retName.Replace("*", "Effect/Explosion/" + clip.clipName);
                break;
            case SoundPlayType.UI:
                if (clip.playTypeUI == SoundPlayTypeUI.NONE) retName = retName.Replace("*", "UI/NONE/" + clip.clipName);
                else if (clip.playTypeUI == SoundPlayTypeUI.BUTTON) retName = retName.Replace("*", "UI/Buttons/" + clip.clipName);
                else if (clip.playTypeUI == SoundPlayTypeUI.CHARACTERS) retName = retName.Replace("*", "UI/Characters/" + clip.clipName);
                else if (clip.playTypeUI == SoundPlayTypeUI.COLLECT) retName = retName.Replace("*", "UI/Collects/" + clip.clipName);
                else if (clip.playTypeUI == SoundPlayTypeUI.HUD) retName = retName.Replace("*", "UI/HUD/" + clip.clipName);
                break;
            case SoundPlayType.ETC:
                if (clip.playTypeETC == SoundPlayTypeETC.NONE) retName = retName.Replace("*", "ETC/NONE/" + clip.clipName);
                if (clip.playTypeETC == SoundPlayTypeETC.BACKGROUND) retName = retName.Replace("*", "ETC/BackGround/" + clip.clipName);
                break;
        }

        return retName.Replace('|', '"');
    }


    public SoundClip GetSoundClip(int index)
    {
        if (index < 0 || index >= soundClips.Length)
            return null;

        return soundClips[index];
    }
}
