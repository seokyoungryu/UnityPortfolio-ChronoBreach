using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper : MonoBehaviour
{
    public static float RandomPercentage0To100()
    {
        return Random.Range(0f, 101f);
    }

    public static int GetRandom(int min, int max)
    {
        return Random.Range(min, max);
    }
    public static float GetRandom(float min, float max)
    {
        return Random.Range(min, max);
    }


    public static Vector3 GetRandomVector3(Vector3 min, Vector3 max)
    {
        Vector3 randomVec = Vector3.zero;
        randomVec.x = Random.Range(min.x, max.x);
        randomVec.y = Random.Range(min.y, max.y);
        randomVec.z = Random.Range(min.z, max.z);
        return randomVec;
    }
}
