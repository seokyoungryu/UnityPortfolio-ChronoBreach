using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum MapPositionDlayName
{
    NONE = -1,
    TRIGGER = 0,
    SPAWN = 1,
    WAYPOINT = 2,
}


public class MapDrawPositions : MonoBehaviour
{
    [SerializeField] private DrawGizmosPositionInfo[] positions;

    public float offsetY = 0.5f;
    private GUIStyle style = new GUIStyle();
    public DrawGizmosPositionInfo[] Positions => positions;

    public Vector3 fontOffset = Vector3.zero;

#if UNITY_EDITOR
    [ContextMenu("Positions Setting")]
    private void SettingPositions()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].SettingPositions();

        }
    }

    private void OnDrawGizmos()
    {
        if (positions.Length <= 0) return;


        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].SettingPositions();
            if (!positions[i].DrawGizmo) continue;

            for (int x = 0; x < positions[i].Positions.Count; x++)
            {
                Gizmos.color = positions[i].StringColor;
                Gizmos.DrawSphere(positions[i].Positions[x].position, positions[i].Radius);

                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = positions[i].StringColor;
                Handles.color = positions[i].StringColor;
                Handles.Label(positions[i].Positions[x].position + (Vector3.up * (offsetY * i+1)) + fontOffset, positions[i].DisplayName + "" + x, style);

            }
        }
    }
#endif

}


[System.Serializable]
public class DrawGizmosPositionInfo
{
    [SerializeField] private Transform rootTransform = null;
    [SerializeField] private List<Transform> positions = new List<Transform>();
    [SerializeField] private MapPositionDlayName displayNameType = MapPositionDlayName.NONE;
    [SerializeField] private string displayName = "";
    [SerializeField] private float radius = 2f;
    [SerializeField] private Color stringColor = Color.white;
    [SerializeField] private bool drawGizmo = true;


    public Transform RootTransform => rootTransform;
    public List<Transform> Positions => positions;
    public MapPositionDlayName DisplayNameType => displayNameType;
    public string DisplayName => displayName;
    public float Radius => radius;
    public Color StringColor => stringColor;
    public bool DrawGizmo => drawGizmo;


    public void SettingPositions()
    {
        positions.Clear();
        int i = 0;
        foreach (Transform child in rootTransform)
        {
            child.name = displayName + "" + i;
            positions.Add(child);
            i++;
        }
    }
}

