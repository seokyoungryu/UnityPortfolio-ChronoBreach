using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBossDetect : MonoBehaviour
{
    [SerializeField] private AIController controller = null;

    [SerializeField] private string detectTag = TagAndLayerDefine.Tags.Player;
    [SerializeField] private bool isAlReadyIntro = false;

    private void OnEnable()
    {
        if (controller == null)
            GetComponentInParent<AIController>();

        if (controller != null)
            controller.onConstDead += ResetData;
    }

    private void OnDisable()
    {
        if (controller == null)
            GetComponentInParent<AIController>();
        if (controller != null)
            controller.onConstDead -= ResetData;
    }

    public void ResetData()
    {
        isAlReadyIntro = false;
    }

    private void OnTriggerEnter(Collider other)
    {
       if (controller == null || isAlReadyIntro || controller.IsPlayableObject) return;
      
       if(other.CompareTag(detectTag))
       {
            if (controller.IsDead())
                return;
           AIManager.Instance.AppearBossList.Enqueue(controller);
           AIManager.Instance.ExcuteBossIntro();
           isAlReadyIntro = true;
       }
    }


}
