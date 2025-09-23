using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReturnObjectToObjectPooling : MonoBehaviour
{
    public string objectPoolName = string.Empty;
    public float returnTime = 0f;
    private float timer = 0f;
    [Header("충돌시 returnTime 동안 disable할 GOs (자식오브젝트만 가능)")]
    public GameObject[] diablesChilds = null;
    [Header("충돌시 False X")]
    public bool isCollisionFalse = false;
    public bool isSetOriginParent = false;

   [Header("Exit 애니메이션")]
   [SerializeField] private string exitClipName = string.Empty;
   [SerializeField] private float doExitTime = 0f;
    [SerializeField] private Animator anim;
    private bool startExit = false;

    #region Events
    public delegate void OnResetData();
    public event OnResetData resetData;
    public event OnResetData onResetData
    {
        add
        {
            if (resetData == null || !resetData.GetInvocationList().Contains(value))
                resetData += value;
        }
        remove
        {
            resetData -= value;
        }
    }

    public event OnResetData onSetPollingData;
    #endregion



    private void OnEnable()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        StopAllCoroutines();
        timer = 0f;
        startExit = false;
        if (diablesChilds != null && diablesChilds.Length > 0)
            for (int i = 0; i < diablesChilds.Length; i++)
                diablesChilds[i].SetActive(true);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        timer = 0f;
        startExit = false;
        if (diablesChilds != null && diablesChilds.Length > 0)
            for (int i = 0; i < diablesChilds.Length; i++)
                diablesChilds[i].SetActive(true);
    }



    void Update()
    {
        if (returnTime <= 0) return;

        timer += Time.deltaTime;

        if (doExitTime > 0)
        {
            if (exitClipName != string.Empty && timer >= doExitTime && !startExit)
            {
                anim.CrossFade(exitClipName, 0.2f);
                startExit = true;
            }
        }


        if (timer >= returnTime)
        {
            resetData?.Invoke();
            SetOBP();
        }
    }

    public void DelaySetOBP(float delay)
    {
        StartCoroutine(DelaySetOBP_Co(delay));
    }

    private IEnumerator DelaySetOBP_Co(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetOBP();
    }

    public void SetOBP()
    {
        if(isSetOriginParent)
        {
            transform.parent = ObjectPooling.Instance.GetEffectOBPParentContainer(objectPoolName).transform;
        }

        timer = 0f;
        onSetPollingData?.Invoke();
        ObjectPooling.Instance.SetOBP(objectPoolName, this.gameObject);
        onSetPollingData = null;
    }

    public void TimeSetting(float returnTime, float exitTime)
    {
        this.returnTime = returnTime;
        this.doExitTime = exitTime;
    }


    public void ActiveChild(bool isActive)
    {
        if (diablesChilds.Length > 0)
            for (int i = 0; i < diablesChilds.Length; i++)
                diablesChilds[i].SetActive(isActive);
    }


    public void ExcuteCollision(float returnTime)
    {
        if(returnTime <= 0)
            SetOBP();

        if (isCollisionFalse) return;

        this.returnTime = returnTime;
        if (diablesChilds.Length > 0)
            for (int i = 0; i < diablesChilds.Length; i++)
                diablesChilds[i].SetActive(false);

    }


    [ContextMenu("Set Disable")]
    private void SetDisable()
    {
        List<GameObject> child = new List<GameObject>();
        foreach (Transform childTr in transform)
        {
            if (childTr != transform)
                child.Add(childTr.gameObject);
        }

        diablesChilds = child.ToArray();
    }


}
