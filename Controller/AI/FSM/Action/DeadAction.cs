using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Dead")]
public class DeadAction : Action
{
    public override void OnEnterAction(AIController controller)
    {
        controller.StopAllCoroutines();
        if (controller.gameObject.layer == 11)
        {
            Debug.Log("Dead In!");
        }
        controller.SetNavSpeed(0f);
        controller.nav.velocity = Vector3.zero;
        controller.myColl.enabled = false;
        controller.aiConditions.IsDead = true;
        controller.aiAnim.applyRootMotion = true;
        controller.aiAnim.Play(controller.aIFSMVariabls.deadAnimataionName);
        controller.Dead();

        controller.questReporter.ReceiveReport("KILL");

        if (controller.aiConditions.CanDropItem)
            controller.StartCoroutine(AfterDeadProcess(controller));
    }

    public override void Act(AIController controller, float deltaTime)
    {
    }

    private IEnumerator AfterDeadProcess(AIController controller)
    {
        float waitTime = (1f / 30f) * controller.aIFSMVariabls.afterDeadProcessFrame;

        yield return new WaitForSeconds(waitTime);
        controller.myColl.isTrigger = true;

        if (controller.aiStatus.DropItems.Length > 0)
        {
            ItemSpwanObject spawnItem = ObjectPooling.Instance.GetOBP("ItemSpawnObject").GetComponent<ItemSpwanObject>();
            spawnItem.SetItemInfo(controller.aiStatus.DropItems);
            spawnItem.transform.position = controller.transform.position + Vector3.up * 1.5f;
        }

        Debug.Log("Dead ½ÇÇà!!!!");

        yield return new WaitForSeconds(controller.aIVariables.deadOffesetTime);
        ObjectPooling.Instance.SetOBP(controller.OBPName, controller.transform.parent.gameObject);

    }
}
