using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonEntryPortal : MonoBehaviour
{
    [SerializeField] private DungeonEntryDatabase dungeonDatabase = null;

    public CoroutineForDungeon dungeonCoroutine = null;

    #region Events
    public delegate void OnEntry(DungeonEntryDatabase database);
    public delegate void OnExit();

    public event OnEntry onEntry;
    public event OnExit onExit;

    #endregion

    private void Start()
    {
        dungeonCoroutine = GetComponent<CoroutineForDungeon>();
        onEntry += MapManager.Instance.OpenDungeonSelectionUI;
        onExit += MapManager.Instance.CloseDungeonSelectionUI;
    }

    private void OnDestroy()
    {
        onEntry -= MapManager.Instance.OpenDungeonSelectionUI;
        onExit -= MapManager.Instance.CloseDungeonSelectionUI;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEntry?.Invoke(dungeonDatabase);        //여기서 dungeonPortalUI에 Open하기.
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onExit?.Invoke();
    }
}
