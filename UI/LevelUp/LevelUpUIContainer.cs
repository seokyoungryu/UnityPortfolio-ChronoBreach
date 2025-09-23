using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIContainer : MonoBehaviour
{
    [SerializeField] private Queue<LevelUpUI> levelUpQueue = new Queue<LevelUpUI>();
    [SerializeField] private Transform parent = null;
    [SerializeField] private float waitTerm = 0.3f;
    private bool isExcutingProcess = false;


    public void RegisterUI(LevelUpUI levelup)
    {
        levelup.ContainerActive(false);
        levelUpQueue.Enqueue(levelup);

        if (!isExcutingProcess)
            StartCoroutine(ProcessLevelUpUI_Co());

    }


    private IEnumerator ProcessLevelUpUI_Co()
    {
        if (levelUpQueue.Count <= 0) yield break;
        isExcutingProcess = true;
        SoundManager.Instance.PlayExtraSound(UISoundType.PLAYER_LEVELUP);
        LevelUpUI ui = levelUpQueue.Dequeue();
        ui.transform.parent = parent;
        ui.transform.SetAsLastSibling();
        ui.ContainerActive(true);

        yield return new WaitForSeconds(waitTerm);


        if (levelUpQueue.Count > 0)
            StartCoroutine(ProcessLevelUpUI_Co());
        else if (levelUpQueue.Count <= 0)
            isExcutingProcess = false;
    }

}
