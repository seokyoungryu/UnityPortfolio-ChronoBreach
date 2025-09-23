using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CreateAssetMenu(menuName = "TitleData", fileName = "TitleSlot_")]
public class TitleSlotData : ScriptableObject
{

    [SerializeField] private int saveSlotIndex = 0;
    private string playerInfoFileFullPath = string.Empty;
    [SerializeField] private string playerInfoFileName = "PlayerInfo";
    [SerializeField] private string fileExtension = ".csv";

    public Vector3 playerPosition;
    private QuestListSession currQuestSession;
    //public Vector3 camRotation;


    public int SaveSlotIndex { get { return saveSlotIndex; } set { saveSlotIndex = value; } }
    public QuestListSession CurrQuestSession => currQuestSession;




    public void SavePlayerInfo(PlayerStateController controller)
    {
        if(controller == null)
        {
            Debug.Log("Player Info Save Fail (Null)");
            return;
        }

        TimeManager.Instance.SetCurrTime();

        PlayerStatus stats = controller.playerStats;
        playerInfoFileFullPath = Application.persistentDataPath + playerInfoFileName + saveSlotIndex + fileExtension;
        Debug.Log("세이브 플레이어 정보 : " + playerInfoFileFullPath);

        using (StreamWriter sw = new StreamWriter(playerInfoFileFullPath, false, System.Text.Encoding.UTF8))
        {
            string line = "Lv, Current Exp, CurrentHp, CurrentStamina ,All Stats, STR, DEX, LUCK, INT, Hp, Stamina," +
                "Atk, Atk Speed, Cri Chance, Cri Dmg, Magic Defense, Defense, Evasion,Remaining StatPoint, Remaining SkillPoint," +
                "HpRegenerationPerSecond , staminaRegenPerSecond, SkillIncreaseDmgPercent, HitPerHp,HpRegenTime, staminaRegenTime," +
                "Current Scene Index, Current Position(X),Current Position(Y),Current Position(Z), Own Money, Own Reputation, Play Time," +
                "Current Rosition(X),Current Rosition(Y),Current Rosition(Z),Cam Rot X, Cam Rot Y, SaveScene,QuestSession";
            sw.WriteLine(line);


            line = $"{stats.Level},{stats.CurrentExp},{stats.CurrentHealth},{stats.CurrentStamina},{stats.OriginAllStats}," +
                $"{stats.OriginStrength},{stats.OriginDexterity},{stats.OriginLuck},{stats.OriginIntelligence},{stats.OriginHealth}," +
                $"{stats.OriginStamina},{stats.OriginAtk},{stats.OriginAtkSpeed},{stats.OriginCriticalChance},{stats.OriginCriticalDmg}," +
                $"{stats.OriginMagicDefense},{stats.OriginDefense},{stats.OriginEvasion},{stats.RemainingStatPoint},{stats.RemainingSkillPoint}," +
                $"{stats.OriginHpRegenPerSec},{stats.OriginStaminaRegenPerSec},{stats.OriginSkillIncreaseDmgPercentage},{stats.OriginHitPerHp}," +
                $"{stats.HealthRegenTime},{stats.StaminaRegenTime},{ScenesManager.Instance.CurrentSceneIndex}," +
                $"{controller.transform.position.x},{controller.transform.position.y},{controller.transform.position.z}," +
                $"{GameManager.Instance.OwnMoney},{GameManager.Instance.Reputation},{TimeManager.Instance.CurrentTimer + TimeManager.Instance.LoadTimer}," +
                $"{controller.transform.rotation.eulerAngles.x},{controller.transform.rotation.eulerAngles.y},{controller.transform.rotation.eulerAngles.z},"+
                 $"{GameManager.Instance.Cam.PivotTr.eulerAngles.x},{GameManager.Instance.Cam.PivotTr.eulerAngles.y}, {ScenesManager.Instance.CurrentSceneIndex}" +
                 $",{(int)QuestManager.Instance.currentQuestSession}";


            Debug.Log("시간 저장 : "+TimeManager.Instance.CurrentTimer  + " , " + TimeManager.Instance.LoadTimer);

            sw.WriteLine(line);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(controller);
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
#endif
    }

        
    public bool CanLoadInfo()
    {
        playerInfoFileFullPath = Application.persistentDataPath + playerInfoFileName + saveSlotIndex + fileExtension;
        if (File.Exists(playerInfoFileFullPath))
            return true;
        else return false;
    }


    public string[] LoadPlayerInfoForTitleSlot()
    {
        playerInfoFileFullPath = Application.persistentDataPath + playerInfoFileName + saveSlotIndex + fileExtension;
        if (File.Exists(playerInfoFileFullPath))
        {
            List<string> data = new List<string>();
            using (StreamReader sr = new StreamReader(playerInfoFileFullPath))
            {
                string loadData = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    loadData = sr.ReadLine();
                    string[] splitTab = loadData.Split(',');
                    int lv = int.Parse(splitTab[0]);
                    float playTime = float.Parse(splitTab[32]);

                    (int, int, int) times = TimeManager.Instance.TranslateTime((int)playTime);
                    data.Add(lv.ToString());
                    data.Add(times.Item1.ToString("00") + ":" + times.Item2.ToString("00") + ":" + times.Item3.ToString("00"));
                    Debug.Log(times.Item1.ToString("00") + ":" + times.Item2.ToString("00") + ":" + times.Item3.ToString("00"));
                }
            }
            return data.ToArray();
        }
        return null;
    }


    public bool LoadPlayerInfo(PlayerStateController controller, bool canLoad, bool loadPosition)
    {
        playerInfoFileFullPath = Application.persistentDataPath + playerInfoFileName + saveSlotIndex + fileExtension;
        PlayerStatus stats = controller.playerStats;
        if (canLoad && File.Exists(playerInfoFileFullPath))
        {
            using (StreamReader sr = new StreamReader(playerInfoFileFullPath))
            {
                string loadData = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    int index = 0;
                    loadData = sr.ReadLine();
                    string[] splitTab = loadData.Split(',');
                    stats.Level = int.Parse(splitTab[index++]);
                    stats.NextExp = stats.ExpData.GetExpContainer(stats.Level + 1);
                    stats.CurrentExp = long.Parse(splitTab[index++]);
                    stats.CurrentHealth = int.Parse(splitTab[index++]);
                    stats.CurrentStamina = int.Parse(splitTab[index++]);
                    stats.OriginAllStats = int.Parse(splitTab[index++]);
                    stats.OriginStrength = int.Parse(splitTab[index++]);
                    stats.OriginDexterity = int.Parse(splitTab[index++]);
                    stats.OriginLuck = int.Parse(splitTab[index++]);
                    stats.OriginIntelligence = int.Parse(splitTab[index++]);

                    stats.OriginHealth = int.Parse(splitTab[index++]);
                    stats.OriginStamina = int.Parse(splitTab[index++]);
                    stats.OriginAtk = int.Parse(splitTab[index++]);
                    stats.OriginAtkSpeed = float.Parse(splitTab[index++]);
                    stats.OriginCriticalChance = float.Parse(splitTab[index++]);
                    stats.OriginCriticalDmg = float.Parse(splitTab[index++]);
                    stats.OriginMagicDefense = int.Parse(splitTab[index++]);
                    stats.OriginDefense = int.Parse(splitTab[index++]);
                    stats.OriginEvasion = float.Parse(splitTab[index++]);
                    stats.RemainingStatPoint = int.Parse(splitTab[index++]);
                    stats.RemainingSkillPoint = int.Parse(splitTab[index++]);

                    stats.OriginHpRegenPerSec = float.Parse(splitTab[index++]);
                    stats.OriginStaminaRegenPerSec = float.Parse(splitTab[index++]);
                    stats.OriginSkillIncreaseDmgPercentage = float.Parse(splitTab[index++]);
                    stats.OriginHitPerHp = float.Parse(splitTab[index++]);
                    stats.HealthRegenTime = float.Parse(splitTab[index++]);
                    stats.StaminaRegenTime = float.Parse(splitTab[index++]);

                    int sceneIndex = int.Parse(splitTab[index++]);
                    Vector3 position;
                    position.x = float.Parse(splitTab[index++]);
                    position.y = float.Parse(splitTab[index++]);
                    position.z = float.Parse(splitTab[index++]);
                    playerPosition = position;

                    int ownMoney = int.Parse(splitTab[index++]);
                    int ownReputation = int.Parse(splitTab[index++]);
                    GameManager.Instance.LoadOwnMoney(ownMoney);
                    GameManager.Instance.LoadReputation(ownReputation);
                    TimeManager.Instance.LoadTimer = float.Parse(splitTab[index++]);
                    Vector3 rotation;
                    rotation.x = float.Parse(splitTab[index++]);
                    rotation.y = float.Parse(splitTab[index++]);
                    rotation.z = float.Parse(splitTab[index++]);
                    Vector2 camRotation;
                    camRotation.x = float.Parse(splitTab[index++]);
                    camRotation.y = float.Parse(splitTab[index++]);
                    int currSceneIndex = int.Parse(splitTab[index++]);
                    currQuestSession = (QuestListSession)int.Parse(splitTab[index++]);

                    if (loadPosition && currSceneIndex == 1)
                    {
                        GameManager.Instance.Player.TranslatePosition(position);
                        GameManager.Instance.Player.Rotate(rotation);
                        GameManager.Instance.StartCoroutine(LoadCam(camRotation));
                        Debug.Log("Pos 0 ");
                    }
                    else if(!loadPosition || currSceneIndex != 1)
                    {
                        GameManager.Instance.Player.TranslatePosition(ScenesManager.Instance.MainSceneInitPos);
                        GameManager.Instance.Player.Rotate(ScenesManager.Instance.MainSceneInitRot);
                        GameManager.Instance.StartCoroutine(ResetCam()); 
                        Debug.Log("Pos 1 ");
                    }
                    else
                    {
                        GameManager.Instance.Player.TranslatePosition(ScenesManager.Instance.MainSceneInitPos);
                        GameManager.Instance.Player.Rotate(ScenesManager.Instance.MainSceneInitRot);
                        GameManager.Instance.StartCoroutine(ResetCam());
                    }
                    stats.UpdateStats();
                }
                return true;
            }
        }
        playerPosition = ScenesManager.Instance.MainSceneInitPos;
        GameManager.Instance.Player.TranslatePosition(ScenesManager.Instance.MainSceneInitPos);
        GameManager.Instance.Player.Rotate(ScenesManager.Instance.MainSceneInitRot);
        GameManager.Instance.StartCoroutine(ResetCam());
        return false;
    }

    private IEnumerator ResetCam()
    {
        Debug.Log($"<color=red> ResetCam </color>");
        yield return new WaitForSeconds(0f);
        GameManager.Instance.Cam.ResetRotation();
        Debug.Log($"<color=red> ResetCam Done</color>");

    }

    private IEnumerator LoadCam(Vector2 rot)
    {
        Debug.Log($"<color=red> LoadCam </color>");

        yield return new WaitForSeconds(0f);
        GameManager.Instance.Cam.CamRotation(rot);
        Debug.Log($"<color=red> LoadCam Done</color>");

    }


    public void DeleteLoadData()
    {
        playerInfoFileFullPath = Application.persistentDataPath + playerInfoFileName + saveSlotIndex + fileExtension;

        if (File.Exists(playerInfoFileFullPath))
        {
            File.Delete(playerInfoFileFullPath);
        }
    }
}
