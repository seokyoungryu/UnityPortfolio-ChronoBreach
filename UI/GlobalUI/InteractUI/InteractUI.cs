using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTr = null;
    [SerializeField] private TMP_Text description_Text = null;
    [SerializeField] private Vector2 activeAnchorPosition = Vector2.zero;

    public InteractObject currentInteract = null;
    private List<InteractObject> interacts = new List<InteractObject>();
    public NpcController currNpcController;


  
    private void Start()
    {
        rectTr.anchoredPosition = activeAnchorPosition;
        gameObject.SetActive(false);
    }


    private void Update()
    {
        if (QuestManager.Instance.isDialoging)
        {
            Debug.Log("<color=red> 여기1 </color>");
            gameObject.SetActive(false);
            return;
        }

        if(Input.GetKeyDown(KeyCode.F) && currentInteract != null)
        {
            Debug.Log("<color=red> 여기2 </color>");
            if (currentInteract.AutoExcute) return;
            Debug.Log("<color=red> 여기3 </color>");
            currentInteract.ExcuteInteract();
            gameObject.SetActive(false);
        }
    }

    public void Active(bool isActive) => gameObject.SetActive(isActive);


    public void SettingDescription(string interctDescription)
    {
        description_Text.text = interctDescription;
       
    }

    public void RegisterInteractUI(InteractObject target)
    {
        if (!interacts.Contains(target))
        {
            interacts.Add(target);
            SoundManager.Instance.PlayUISound(UISoundType.INTERACT);
        }

        if (interacts.Count > 0 && !gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        SetCurrentInteract(target);
        if (currentInteract.AutoExcute)
            currentInteract.ExcuteInteract();

    }

    public void RemoveInteractUI(InteractObject target)
    {
        if (interacts.Contains(target)) interacts.Remove(target);
        if (currentInteract == target)
        {
            currentInteract = null;
            currNpcController = null;
        }

        if(interacts.Count > 0)
            SetCurrentInteract(interacts[interacts.Count - 1]);
        else if (interacts.Count <= 0)
            gameObject.SetActive(false);

    }


    public void SetCurrentInteract(InteractObject target)
    {
        currentInteract = target;
        currNpcController = target.GetComponent<NpcController>();
        currentInteract.UIDescripSetting();
        if (currentInteract.AutoExcute)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

    }
    public void RemoveCurrentInteractUI()
    {
        if (currentInteract != null)
            currentInteract.IsExcuteInteractUI = false;
        if (interacts.Contains(currentInteract)) interacts.Remove(currentInteract);
        currentInteract = null;

        if (interacts.Count > 0)
            SetCurrentInteract(interacts[interacts.Count - 1]);
        else if (interacts.Count <= 0)
            gameObject.SetActive(false);

    }


}
