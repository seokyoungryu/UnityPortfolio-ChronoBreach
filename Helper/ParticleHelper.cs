using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleHelper 
{
  
    public static void SettingParticleLifeTime(GameObject root, ParticleLifeType type, float stayTime, float duration)
    {
        ParticleSystem[] particels = root.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < particels.Length; i++)
            particels[i].Stop();

        for (int i = 0; i < particels.Length; i++)
        {
            var main = particels[i].main;
            main.loop = false;
            if (type == ParticleLifeType.DURATION)
                main.duration = duration;
            else if (type == ParticleLifeType.LIFE_TIME)
                main.startLifetime = stayTime;
            else if (type == ParticleLifeType.DURATION_LIFETIME_SAME)
            {
                main.duration = stayTime;
                main.startLifetime = stayTime;
            }
            else if (type == ParticleLifeType.DURATION_LIFETIME_EACH)
            {
                main.duration = duration;
                main.startLifetime = stayTime;
            }
        }

        for (int i = 0; i < particels.Length; i++)
            particels[i].Play();
    }
}
