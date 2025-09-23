using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Useable/Function Object/Return Position", fileName = "ReturnPosition_")]
public class ReturnPositionFO : FunctionObject
{
    [SerializeField] private int sceneNum = -1;
    [SerializeField] private Vector3 returnPosition = Vector3.zero;

    public override void Apply(BaseController controller)
    {
        base.Apply(controller);
        Debug.Log("여기 실행1");

        if (!ScenesManager.Instance.IsCurrentScene(sceneNum))
            ScenesManager.Instance.ChangeScene(sceneNum);

        Debug.Log("여기 실행2 : " + playerController);
        playerController?.TranslatePosition(returnPosition);
       
    }
}
