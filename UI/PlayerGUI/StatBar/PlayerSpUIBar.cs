using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpUIBar : MonoBehaviour
{
    [SerializeField] private Image spBar_Img = null;


    public void OnSpChanged(PlayerStatus playerStatus)
    {
        spBar_Img.fillAmount = (float)playerStatus.CurrentStamina / (float)playerStatus.TotalStamina;
    }
   
}
