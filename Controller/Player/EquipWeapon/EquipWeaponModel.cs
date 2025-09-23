using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeaponModel : MonoBehaviour
{
    public Transform weaponTransform = null;
    public EquipWeaponTransform equipWeaponModel = null;


    private void Awake()
    {
        weaponTransform = this.transform;
    }

    public void EquipModel(GameObject weaponModel)
    {
        if (equipWeaponModel != null) DeleteModel();

        GameObject model = Instantiate(weaponModel);
        equipWeaponModel = model.GetComponent<EquipWeaponTransform>();
        model.transform.parent = weaponTransform;
        model.transform.localPosition = equipWeaponModel.WeaponPosition;
        model.transform.localRotation = Quaternion.Euler(equipWeaponModel.WeaponRotation);
        model.transform.localScale = equipWeaponModel.WeaponScale;
    }

    public void DeleteModel()
    {
        Destroy(equipWeaponModel.gameObject);
        equipWeaponModel = null;
    }
}
