using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshController : MonoBehaviour
{
    public Transform rootBoneTranform = null;
    public Transform meshParent = null;

    [Header("생성된 SkinnedMesh")]
    [Header("[ 0 : Head, 1 : Upper, 2 : Lower, 3 : Hand, 4 : Leg , 5 : Cloak")]
    public Transform[] equipmentSkinnedMesh = new Transform[6];

    private Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();


    private void Awake()
    {
        if (rootBoneTranform != null)
            SetBoneHierachy(rootBoneTranform);
        equipmentSkinnedMesh = new Transform[6];
    }


    public void CreateSkinnedMesh(Item item)
    {
        if (item.id < 0 || !item.itemClip.havePrefab || item.itemClip.itemPrefab == null || item.itemType == SlotAllowType.TITLE) return;

        SlotAllowType type = item.itemType;

        switch (type)
        {
            case SlotAllowType.HEAD:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[0] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            case SlotAllowType.UPPER:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[1] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            case SlotAllowType.LOWER:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[2] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            case SlotAllowType.HAND:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[3] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            case SlotAllowType.LEG:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[4] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            case SlotAllowType.CLOAK:
                DeletSkinnedMesh(type);
                equipmentSkinnedMesh[5] = CreateMesh(item.itemClip.itemPrefab, meshParent);
                break;
            default:
                break;
        }

    }
    public void DeletSkinnedMesh(SlotAllowType inputType)
    {
        switch (inputType)
        {
            case SlotAllowType.HEAD:
                if (equipmentSkinnedMesh[0] != null)
                    Destroy(equipmentSkinnedMesh[0].gameObject);
                equipmentSkinnedMesh[0] = null;
                break;
            case SlotAllowType.UPPER:
                if (equipmentSkinnedMesh[1] != null)
                    Destroy(equipmentSkinnedMesh[1].gameObject);
                equipmentSkinnedMesh[1] = null;
                break;
            case SlotAllowType.LOWER:
                if (equipmentSkinnedMesh[2] != null)
                    Destroy(equipmentSkinnedMesh[2].gameObject);
                equipmentSkinnedMesh[2] = null;
                break;
            case SlotAllowType.HAND:
                if (equipmentSkinnedMesh[3] != null)
                    Destroy(equipmentSkinnedMesh[3].gameObject);
                equipmentSkinnedMesh[3] = null;
                break;
            case SlotAllowType.LEG:
                if (equipmentSkinnedMesh[4] != null)
                    Destroy(equipmentSkinnedMesh[4].gameObject);
                equipmentSkinnedMesh[4] = null;
                break;
            case SlotAllowType.CLOAK:
                if (equipmentSkinnedMesh[5] != null)
                    Destroy(equipmentSkinnedMesh[5].gameObject);
                equipmentSkinnedMesh[5] = null;
                break;
            default:
                break;
        }

    }

    public Transform CreateMesh(GameObject model, Transform parent)
    {
        SkinnedMeshContainer container = model.GetComponent<SkinnedMeshContainer>();
        Transform parentFolder = new GameObject(container.meshType.ToString()).transform;

        Transform[] meshs = new Transform[container.meshInfos.Length];

        for (int i = 0; i < container.meshInfos.Length; i++)
        {
            meshs[i] = CreateBoneSkinnedMeshRenderer(container.meshInfos[i]);
            meshs[i].SetParent(parentFolder);
        }

        parentFolder.SetParent(parent);

        return parentFolder;
    }


    private Transform CreateBoneSkinnedMeshRenderer(SkinnedMeshInfo meshInfo)
    {
        Transform newGo = new GameObject(meshInfo.model.name).transform;
        SkinnedMeshRenderer newSkinMesh = newGo.gameObject.AddComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer meshInfoRenderer = meshInfo.model.GetComponent<SkinnedMeshRenderer>();

        Transform[] boneTransform = new Transform[meshInfo.boneNames.Length];
        for (int i = 0; i < meshInfo.boneNames.Length; i++)
        {
            if (rootBoneDictionary.ContainsKey(meshInfo.boneNames[i].GetHashCode()))
                boneTransform[i] = rootBoneDictionary[meshInfo.boneNames[i].GetHashCode()];
        }


        for (int i = 0; i < meshInfoRenderer.sharedMaterials.Length; i++)
            meshInfo.colorInfo.SetMaterialsColor(newSkinMesh);

        newSkinMesh.bones = boneTransform;
        newSkinMesh.rootBone = rootBoneDictionary[meshInfo.rootBoneName.GetHashCode()];
        newSkinMesh.sharedMaterials = meshInfoRenderer.sharedMaterials;
        newSkinMesh.sharedMesh = meshInfoRenderer.sharedMesh;
        newSkinMesh.localBounds = meshInfoRenderer.localBounds;

        return newGo;
    }

    private void SetBoneHierachy(Transform bone)
    {
        foreach (Transform child in bone)
        {
            rootBoneDictionary.Add(child.name.GetHashCode(), child);

            SetBoneHierachy(child);
        }
    }
}
