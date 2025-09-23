using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    


    public void CloseUIWindow(GameObject window)
    {
        window.gameObject.SetActive(false);
        //그리고 esc로 끄는 목록에 해당 ui 제거하기.
    }
}
