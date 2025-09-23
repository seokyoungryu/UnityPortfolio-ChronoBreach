using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RescuerCheck : MonoBehaviour
{
    [SerializeField] private string compareTag = "Player";
    [SerializeField] private bool isRescued = false;
    private AIController controller = null;

    public delegate void OnActivcRescuer();

    public event OnActivcRescuer onActiveRescuer;

    private void Start()
    {
        if (controller == null)
            controller = GetComponentInParent<AIController>();
    }


    private void OnDisable()
    {
        isRescued = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRescued) return;
        if (other.CompareTag(compareTag))
        {
            isRescued = true;
            controller.aiConditions.CanState = true;
            onActiveRescuer?.Invoke();
        }
    }

}
