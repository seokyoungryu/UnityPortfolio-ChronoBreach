using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaticleGroupSetting : MonoBehaviour
{
    public GameObject[] particleGos;

    public bool vec3Zero = true;
    public bool playOnAwake = false;


    [ContextMenu("¼öÁ¤!")]
    public void Edit()
    {
        for (int i = 0; i < particleGos.Length; i++)
        {
            
            particleGos[i].transform.localPosition = new Vector3(0, 0, 0);

            ParticleSystem particleSys = particleGos[i].GetComponent<ParticleSystem>();
            if (particleSys == null) continue;

            if (playOnAwake)
                particleSys.playOnAwake = playOnAwake;
        }

    }

}
