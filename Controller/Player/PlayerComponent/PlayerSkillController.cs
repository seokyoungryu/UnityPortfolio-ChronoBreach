using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : SkillController
{
    [Header("기본 소유 스킬 등록")]
    [SerializeField] private OwnSkillDatabase ownSkillDatabase = null;

    private string saveFileName = "PlayerOwnSkillData_";
    private PlayerStateController playerController;

    public OwnSkillDatabase OwnSkillDatabase => ownSkillDatabase;
    
    protected override void Awake()
    {
        playerController = GetComponent<PlayerStateController>();

    }


    public override void CheckEnableBuffTime(BaseController controller) => base.CheckEnableBuffTime(playerController);
    public override void CheckEnableDeBuffTime(BaseController controller) => base.CheckEnableDeBuffTime(playerController);
    protected override void CreateOwnSkill()
    {
        if (ownSkillDatabase == null || ownSkillDatabase.OwnSkills.Count <= 0) return;

        for (int i = 0; i < ownSkillDatabase.OwnSkills.Count; i++)
        {
            BaseSkillClip skillClip = Instantiate(ownSkillDatabase.OwnSkills[i]);
            skillClip.UpgradeSkill(playerController,true);
            skillClip.UpdateUpgradeType();
            ownSkills.Add(MakeClipToCloneSkillData(skillClip));
        }
    }

    #region Save & Load

    public void SaveSkillDataToExcel()
    {
        string path = Application.persistentDataPath + saveFileName + SaveManager.Instance.SaveSlotIndex + ".csv";
        Debug.Log("Skill Save :" +path);

        using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.UTF8))
        {
            string line  = "Skill ID, Skill Name,Current Skill Lv, Skill State";
            sw.WriteLine(line);

            if (ownSkills.Count <= 0) return;

            for (int i = 0; i < ownSkills.Count; i++)
            {
                if (ownSkills[i].skillClip == null) continue;

                line = ownSkills[i].skillClip.ID + ","
                    + ownSkills[i].skillClip.displayName + ","
                    + ownSkills[i].skillClip.currentSkillIndex + ","
                    + (int)ownSkills[i].skillClip.skillState;
                sw.WriteLine(line);
            }
        }
    }

    public void LoadPlayerSkill(bool isNewData)
    {
        if (isNewData)
        {
            CreateOwnSkill();
            Debug.Log("스킬 new Load 성공");
            return;
        }
        else
        {
            if (LoadExcelToSkillData())
                Debug.Log("스킬 Load 성공");
            else
                CreateOwnSkill();
        }

    }


    private bool LoadExcelToSkillData()
    {
        string path = Application.persistentDataPath + saveFileName + SaveManager.Instance.SaveSlotIndex + ".csv";
        Debug.Log("Skjill Load :" + path);
        if (File.Exists(path))
        {
            ownSkills.Clear();
            using (StreamReader sr = new StreamReader(path))
            {
                string loadData = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    loadData = sr.ReadLine();
                    string[] splitTap = loadData.Split(',');

                    int skillID = int.Parse(splitTap[0]);
                    int currentSkillLv = int.Parse(splitTap[2]);
                    CurrentSkillState skillState = (CurrentSkillState)int.Parse(splitTap[3]);

                    BaseSkillClip clone = skillDatabase.GetSkillClone(skillID);
                    if (clone is AttackSkillClip)
                    {
                        AttackSkillClip clip = new AttackSkillClip(clone);
                        LoadClip(clip, currentSkillLv, skillState);
                    }
                    else if (clone is MagicSkillClip)
                    {
                        MagicSkillClip clip = new MagicSkillClip(clone);
                        LoadClip(clip, currentSkillLv,skillState);
                    }
                    else if (clone is BuffSkillClip)
                    {
                        BuffSkillClip clip = new BuffSkillClip(clone);
                        LoadClip(clip, currentSkillLv,skillState);
                    }
                    else if (clone is ComboSkillClip)
                    {
                        ComboSkillClip clip = new ComboSkillClip(clone);
                        LoadClip(clip, currentSkillLv,skillState);
                    }
                    else if (clone is PassiveSkillClip)
                    {
                        PassiveSkillClip clip = new PassiveSkillClip(clone);
                        LoadClip(clip, currentSkillLv,skillState);
                    }
                    else if (clone is CounterSkillClip)
                    {
                        CounterSkillClip clip = new CounterSkillClip(clone);
                        LoadClip(clip, currentSkillLv,skillState);
                    } 
                    else if (clone is DashSkillClip)
                    {
                        DashSkillClip clip = new DashSkillClip(clone);
                        LoadClip(clip, currentSkillLv, skillState);
                    }
                }
            }
            Debug.Log("스킬 로드 완");
            return true;
        }

        return false;
    }

    #endregion

    private void LoadClip<T>(T clip, int currentSkillLv, CurrentSkillState skillState) where T : BaseSkillClip
    {
        Debug.Log("Load! : " + clip.displayName + ", type" + clip.GetType());
        clip.currentSkillIndex = currentSkillLv;
        clip.skillState = skillState;
        clip.LoadSkill(playerController);
        clip.UpdateUpgradeType();
        ownSkills.Add(MakeClipToCloneSkillData(clip));
    }


    public T[] GetOwnSkillTypes<T>() where T : BaseSkillClip
    {
        List<T> tmpRet = new List<T>();
        for (int i = 0; i < ownSkills.Count; i++)
            if (ownSkills[i].skillClip is T)
                tmpRet.Add(ownSkills[i].skillClip as T);

        return tmpRet.ToArray();
    }

    public void AddSkillToOwnSkillList(BaseSkillClip clip)
    {
        for (int i = 0; i < ownSkills.Count; i++)
            if (ownSkills[i].skillClip.ID == clip.ID)
                return;

        ownSkills.Add(new SkillData(Instantiate(clip)));
        //SaveSkillDataToExcel();
    }

    public T GetOwnSkillClip<T>(int skillID) where T : BaseSkillClip
    {
        foreach (SkillData data in ownSkills)
            if (data.skillClip.ID == skillID)
                return data.skillClip as T;
        return null;
    }


    public T GetUseSkillClip<T>() where T : BaseSkillClip
    {
        T[] clips = GetOwnSkillTypes<T>();
        foreach (BaseSkillClip clip in clips)
            if (clip.skillState == CurrentSkillState.USE)
                return clip as T;
        return null;
    }

   public bool HaveSkillClip(int skillID)
    {
        for (int i = 0; i < ownSkillDatabase.OwnSkills.Count; i++)
            if (skillID == ownSkillDatabase.OwnSkills[i].ID)
                return true;
        return false;
    }
}
