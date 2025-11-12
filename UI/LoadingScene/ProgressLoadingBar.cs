using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProgressLoadingBar : MonoBehaviour
{
    [SerializeField] private Image progressBar_Img = null;
    [SerializeField] private TMP_Text progressPercent_Text = null;
    private AsyncOperation asyncOp = null;
    private static int loadingSceneIndex = 2;

    private void Start()
    {
        StartCoroutine(AsyncLoading_Co(loadingSceneIndex));
    }

    public static void LoadingSetting(int sceneIndex)
    {
        Debug.Log("index : " + sceneIndex);
        loadingSceneIndex = sceneIndex;
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
       
    }



    private IEnumerator AsyncLoading_Co(int sceneIndex)
    {
        bool isUnloadScene = false;
        ScenesManager.Instance.ActiveTempLoadingUI(false);
        Debug.Log("ºñµ¿±â ½ÃÀÛ - " + SceneManager.GetActiveScene().name);
        asyncOp = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOp.completed += (op) => ScenesManager.Instance.OnExcuteDoneLoading();
        asyncOp.completed += (op) => Debug.Log("¿Ï·á - " + op + SceneManager.GetActiveScene().name);
        asyncOp.completed += (op) => GameManager.Instance.canUseCamera = true;

        asyncOp.allowSceneActivation = false;
        float randomUnloadPerc = Random.Range(0.65f, 0.9f);
        float timer = 0f;
       

        while(!asyncOp.isDone)
        {
            yield return null;
            if (!isUnloadScene && asyncOp.progress >= randomUnloadPerc)
                 isUnloadScene = true;

            if (asyncOp.progress < 0.9f)
            {
                progressBar_Img.fillAmount = asyncOp.progress;
                progressPercent_Text.text = (progressBar_Img.fillAmount * 100f).ToString("0") + "%";
            }
            else
            {
                timer += Time.deltaTime;
                progressBar_Img.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                progressPercent_Text.text = (progressBar_Img.fillAmount * 100f).ToString("0" ) + "%";

                if (progressBar_Img.fillAmount >= 1f)
                {
                    asyncOp.allowSceneActivation = true;

                    yield break;
                }
            }
            Debug.Log("·ÎµùÁß.. " + randomUnloadPerc);
        }

    }
}
