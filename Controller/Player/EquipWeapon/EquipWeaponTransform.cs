using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeaponTransform : MonoBehaviour
{
    [SerializeField] private Vector3 weaponPosition;
    [SerializeField] private Vector3 weaponRotation;
    [SerializeField] private Vector3 weaponScale;

    public Vector3 WeaponPosition => weaponPosition;
    public Vector3 WeaponRotation => weaponRotation;
    public Vector3 WeaponScale => weaponScale;


    [ContextMenu("Set Transform")]
    void SetPosition()
    {
        weaponPosition = transform.localPosition;
        weaponScale = transform.localScale;
    }
}
