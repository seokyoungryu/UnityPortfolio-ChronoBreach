using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    public bool drawGizemo = true;
    public bool isDetect = false;
    public float enemyDetectRange = 0f;
    public float wallDetectRange = 0f;

    public LayerMask detectObject = TagAndLayerDefine.LayersIndex.Player;
    public LayerMask detectWallObject = TagAndLayerDefine.LayersIndex.StaticObject;
    private Collider coll = null;
    private Animator anim = null;
    private AIController controller = null;
    private AIConditions aIConditions = null;
    private int attackDetectCount = 0;
    private int wallDetectCount = 0;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        coll = GetComponent<Collider>();
        aIConditions = GetComponentInParent<AIConditions>();
        controller = GetComponentInParent<AIController>();
    }

    Collider[] colls = new Collider[10];

    private void Update()
    {
        if (aIConditions.IgnoreDetectCollider)
        {
            isDetect = false;
        }
        else if (aIConditions.IsAttacking || aIConditions.IsSkilling || controller.nav.velocity != Vector3.zero)
        {
            wallDetectCount = Physics.OverlapSphereNonAlloc(transform.position, wallDetectRange, colls, detectWallObject);
            attackDetectCount = Physics.OverlapSphereNonAlloc(transform.position, enemyDetectRange, colls, detectObject);

            if ((wallDetectCount > 0 || attackDetectCount > 0) )
            {
                isDetect = true;
            }
            else if ((wallDetectCount == 0 && attackDetectCount == 0))
                isDetect = false;

            if (!aIConditions.IsAttacking && !aIConditions.IsSkilling)
                isDetect = false;
        }
        else
            isDetect = false;

        aIConditions.detectedOn = isDetect;
    }


    private void OnDrawGizmosSelected()
    {
        if (!drawGizemo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetectRange);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wallDetectRange);


        if (aIConditions == null) return;
        if (aIConditions.detectedOn)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + Vector3.up * 4f, 0.5f);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up * 4f, 0.5f);
        }
    }

}
