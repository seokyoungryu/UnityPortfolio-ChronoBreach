using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUIByKey : MonoBehaviour
{
    [SerializeField] private RectTransform rootRect = null;
    [SerializeField] private RectTransform targetRect = null;

    [SerializeField] private RectTransform[] taskLists ;
    [SerializeField] private bool setHeightByTasks = false;
    [SerializeField] private float moveLerp = 1f;
    private int currentSelectIndex = -1;
    private float rootHeight = 0f;
    private float maxHeight = 0f;
    private float targetHeight = 0f;



    private void Awake()
    {
        if (rootRect != null)
            rootHeight = rootRect.rect.height;

        SetMaxHeightValue();
    }

    public void OnEnable()
    {
        targetRect.anchoredPosition = Vector3.zero;
        targetHeight = 0f;
        currentSelectIndex = -1;
        //index 초기화
    }

    //이동 인덱스시 -> 해당 task의 y 위치로. 
    //조건, rootRect의 height 보다 커야지, Conatiner 이동가능. 

    private void Update()
    {
        targetRect.anchoredPosition = Vector3.Lerp(targetRect.anchoredPosition, Vector3.down * targetHeight, Time.deltaTime * moveLerp);
        if (targetRect.anchoredPosition.y < 0)
            targetHeight = 0f;
    }


    public void SelectTaskIndex(int index)
    {
        //여기서 index task의 y값이 rootRect Height보다 커야지 이동. 
        if (index >= taskLists.Length || index <= -1 || currentSelectIndex == index) return;

        int previousIndex = currentSelectIndex;
        int currIndex = index;
        currentSelectIndex = currIndex;

        if (previousIndex <= -1 || previousIndex < currIndex)
            SelectTaskDown(currentSelectIndex);
        else if (previousIndex > currIndex)
            SelectTaskUP(currentSelectIndex);
    }

     
    
    public void SelectTaskUP(int index)
    {
        RectTransform task = taskLists[index];
        float taskY = Mathf.Abs(task.anchoredPosition.y);
        float taskHeight = Mathf.Abs(task.rect.height);
        float sumTask = taskY + taskHeight;
        float targetRectY = Mathf.Abs(targetRect.anchoredPosition.y);

        if (targetRectY <= 0)
            return;

        if (targetRectY > taskY)
        {
            this.targetHeight = -(targetRectY - Mathf.Abs((targetRectY - taskY)));
            Debug.Log("Up1 !");
            return;
        }

       //if (maxHeight > taskY)
       //{
       //    this.targetHeight = -(targetRectY - Mathf.Abs(taskHeight));
       //    Debug.Log("Up2");
       //}
    }

    public void SelectTaskDown(int index)
    {
        RectTransform task = taskLists[index];
        float targetY = Mathf.Abs(task.anchoredPosition.y);
        float targetHeight = Mathf.Abs(task.rect.height);
        float sumTask = targetY + targetHeight;

        if (sumTask > rootHeight)
            this.targetHeight = rootHeight - sumTask;
    }


    private void SetMaxHeightValue()
    {
        if (setHeightByTasks)
        {
            if (taskLists == null || taskLists.Length <= 0) maxHeight = 0f;
            else
            {
                float tasksHeight = 0f;
                for (int i = 0; i < taskLists.Length; i++)
                    if (taskLists[i] != null)
                        tasksHeight += taskLists[i].rect.height;

                maxHeight = tasksHeight - rootRect.rect.height;
            }
        }
        else
        {
            if (rootRect == null || targetRect == null) maxHeight = 0f;
            else  maxHeight = targetRect.rect.height - rootRect.rect.height;
        }

    }

    public void SettingTask(RectTransform[] tasks)
    {
        taskLists = tasks;
        targetHeight = 0f;
        targetRect.anchoredPosition = Vector3.zero;
        currentSelectIndex = -1;
        SetMaxHeightValue();
    }
}
